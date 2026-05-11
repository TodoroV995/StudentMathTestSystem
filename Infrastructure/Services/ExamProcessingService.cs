using Application.DTOs.ExamUpload;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using MathProcessor.Interfaces;

namespace Infrastructure.Services
{
    public class ExamProcessingService : IExamProcessingService
    {
        private readonly AppDbContext _dbContext;
        private readonly IExamXmlParser _examXmlParser;
        private readonly IMathExpressionProcessor _mathExpressionProcessor;

        public ExamProcessingService(
            IExamXmlParser examXmlParser,
            IMathExpressionProcessor mathExpressionProcessor,
            AppDbContext dbContext)
        {
            _examXmlParser = examXmlParser;
            _mathExpressionProcessor = mathExpressionProcessor;
            _dbContext = dbContext;
        }

        public async Task<ExamUploadResultDto> ProcessXmlAsync(Stream xmlStream)
        {
            var parsedDocument = await _examXmlParser.ParseAsync(xmlStream);

            var result = new ExamUploadResultDto
            {
                TeacherId = parsedDocument.TeacherId,
                TotalStudents = parsedDocument.StudentExams.Select(x => x.StudentId).Distinct().Count(),
                TotalExams = parsedDocument.StudentExams.Count
            };

            foreach (var exam in parsedDocument.StudentExams)
            {
                var examResult = new ExamResultDto
                {
                    StudentId = exam.StudentId,
                    ExamId = exam.ExamId
                };

                foreach (var task in exam.Tasks)
                {
                    decimal expectedResult =
                        await _mathExpressionProcessor.EvaluateAsync(task.Expression);

                    bool isCorrect = expectedResult == task.StudentResult;

                    examResult.Tasks.Add(new ExamTaskResultDto
                    {
                        TaskId = task.TaskId,
                        Expression = task.Expression,
                        ExpectedResult = expectedResult,
                        StudentResult = task.StudentResult,
                        IsCorrect = isCorrect
                    });
                }

                examResult.TotalTasks = examResult.Tasks.Count;

                examResult.CorrectTasks =
                    examResult.Tasks.Count(t => t.IsCorrect);

                examResult.ScorePercentage =
                    examResult.TotalTasks == 0
                        ? 0
                        : (decimal)examResult.CorrectTasks / examResult.TotalTasks * 100;

                var studentExists = _dbContext.Students.Any(s => s.Id == exam.StudentId);

                if (!studentExists)
                {
                    throw new InvalidOperationException($"Student with ID {exam.StudentId} does not exist.");
                }

                var mathTestExists = _dbContext.MathTests.Any(t => t.Id == exam.ExamId);

                if (!mathTestExists)
                {
                    throw new InvalidOperationException($"Exam/Test with ID {exam.ExamId} does not exist.");
                }

                var testResult = new TestResult
                {
                    StudentId = exam.StudentId,
                    MathTestId = exam.ExamId,
                    Score = (int)Math.Round(examResult.ScorePercentage),
                    TotalQuestions = examResult.TotalTasks,
                    Status = TestStatus.Completed,
                    CompletedAt = DateTime.Now
                };

                foreach (var taskResult in examResult.Tasks)
                {
                    testResult.Tasks.Add(new TestResultTask
                    {
                        TaskId = taskResult.TaskId,
                        Expression = taskResult.Expression,
                        ExpectedResult = taskResult.ExpectedResult,
                        StudentResult = taskResult.StudentResult,
                        IsCorrect = taskResult.IsCorrect
                    });
                }

                _dbContext.TestResults.Add(testResult);

                result.Results.Add(examResult);
            }

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}