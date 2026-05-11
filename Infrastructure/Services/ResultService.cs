using Application.DTOs.Results;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ResultService : IResultService
    {
        private readonly AppDbContext _context;

        public ResultService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TestResultDto>> GetResultsForStudentAsync(int studentId)
        {
            return await _context.TestResults
                .Include(r => r.Tasks)
                .Where(r => r.StudentId == studentId)
                .OrderByDescending(r => r.CompletedAt)
                .Select(r => new TestResultDto
                {
                    Id = r.Id,
                    StudentId = r.StudentId,
                    MathTestId = r.MathTestId,
                    Score = r.Score,
                    TotalQuestions = r.TotalQuestions,
                    Status = r.Status.ToString(),
                    StartedAt = r.StartedAt,
                    CompletedAt = r.CompletedAt,
                    Tasks = r.Tasks.Select(t => new TestResultTaskDto
                    {
                        Id = t.Id,
                        TaskId = t.TaskId,
                        Expression = t.Expression,
                        ExpectedResult = t.ExpectedResult,
                        StudentResult = t.StudentResult,
                        IsCorrect = t.IsCorrect
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<TestResultDto>> GetAllResultsAsync()
        {
            return await _context.TestResults
                .Include(r => r.Tasks)
                .OrderByDescending(r => r.CompletedAt)
                .Select(r => new TestResultDto
                {
                    Id = r.Id,
                    StudentId = r.StudentId,
                    MathTestId = r.MathTestId,
                    Score = r.Score,
                    TotalQuestions = r.TotalQuestions,
                    Status = r.Status.ToString(),
                    StartedAt = r.StartedAt,
                    CompletedAt = r.CompletedAt,
                    Tasks = r.Tasks.Select(t => new TestResultTaskDto
                    {
                        Id = t.Id,
                        TaskId = t.TaskId,
                        Expression = t.Expression,
                        ExpectedResult = t.ExpectedResult,
                        StudentResult = t.StudentResult,
                        IsCorrect = t.IsCorrect
                    }).ToList()
                })
                .ToListAsync();
        }
    }
}