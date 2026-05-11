using Application.Interfaces;
using Application.Models.ExamUpload;
using System.Globalization;
using System.Xml.Linq;

namespace Infrastructure.Xml
{
    public class ExamXmlParser : IExamXmlParser
    {
        public Task<ParsedExamDocument> ParseAsync(Stream xmlStream)
        {
            if (xmlStream == null)
            {
                throw new ArgumentNullException(nameof(xmlStream));
            }

            var document = XDocument.Load(xmlStream);

            var root = document.Root;

            if (root == null)
            {
                throw new InvalidOperationException("XML document does not have a root element.");
            }

            var teacherIdAttribute = root.Attribute("ID") ?? root.Attribute("Id") ?? root.Attribute("id");

            if (teacherIdAttribute == null)
            {
                throw new InvalidOperationException("Teacher ID is missing in XML.");
            }

            var parsedDocument = new ParsedExamDocument
            {
                TeacherId = int.Parse(teacherIdAttribute.Value, CultureInfo.InvariantCulture)
            };

            var studentsElement = root.Element("Students");

            if (studentsElement == null)
            {
                throw new InvalidOperationException("Students element is missing in XML.");
            }

            foreach (var studentElement in studentsElement.Elements("Student"))
            {
                var studentIdAttribute = studentElement.Attribute("ID") ?? studentElement.Attribute("Id") ?? studentElement.Attribute("id");

                if (studentIdAttribute == null)
                {
                    throw new InvalidOperationException("Student ID is missing in XML.");
                }

                int studentId = int.Parse(studentIdAttribute.Value, CultureInfo.InvariantCulture);

                foreach (var examElement in studentElement.Elements("Exam"))
                {
                    var examIdAttribute = examElement.Attribute("ID") ?? examElement.Attribute("Id") ?? examElement.Attribute("id");

                    if (examIdAttribute == null)
                    {
                        throw new InvalidOperationException("Exam ID is missing in XML.");
                    }

                    var parsedExam = new ParsedStudentExam
                    {
                        StudentId = studentId,
                        ExamId = int.Parse(examIdAttribute.Value, CultureInfo.InvariantCulture)
                    };

                    foreach (var taskElement in examElement.Elements("Task"))
                    {
                        var taskIdAttribute = taskElement.Attribute("ID") ?? taskElement.Attribute("Id") ?? taskElement.Attribute("id");

                        if (taskIdAttribute == null)
                        {
                            throw new InvalidOperationException("Task ID is missing in XML.");
                        }

                        var taskText = taskElement.Value;

                        var parsedTask = ParseTask(
                            int.Parse(taskIdAttribute.Value, CultureInfo.InvariantCulture),
                            taskText);

                        parsedExam.Tasks.Add(parsedTask);
                    }

                    parsedDocument.StudentExams.Add(parsedExam);
                }
            }

            return Task.FromResult(parsedDocument);
        }

        private static ParsedExamTask ParseTask(int taskId, string taskText)
        {
            if (string.IsNullOrWhiteSpace(taskText))
            {
                throw new InvalidOperationException("Task text cannot be empty.");
            }

            var parts = taskText.Split('=');

            if (parts.Length != 2)
            {
                throw new InvalidOperationException("Task must contain exactly one '=' sign.");
            }

            var expression = parts[0].Trim();
            var studentResultText = parts[1].Trim();

            if (string.IsNullOrWhiteSpace(expression))
            {
                throw new InvalidOperationException("Task expression cannot be empty.");
            }

            if (!decimal.TryParse(
                    studentResultText,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out decimal studentResult))
            {
                throw new InvalidOperationException("Student result is not a valid number.");
            }

            return new ParsedExamTask
            {
                TaskId = taskId,
                Expression = expression,
                StudentResult = studentResult
            };
        }
    }
}