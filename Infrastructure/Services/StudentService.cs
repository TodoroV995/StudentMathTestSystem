using Application.DTOs.Students;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Infrastructure.Services
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudentDto>> GetAllAsync()
        {
            return await _context.Students
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Grade = s.Grade
                })
                .ToListAsync();
        }

        public async Task<StudentDto> GetByIdAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return null;

            return new StudentDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Grade = student.Grade
            };
        }

        public async Task<StudentDto> CreateAsync(CreateStudentDto dto)
        {
            var student = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Grade = dto.Grade
            };

            _context.Students.Add(student);

            await _context.SaveChangesAsync();

            return new StudentDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Grade = student.Grade
            };
        }

        public async Task UpdateStudentAsync(int id, UpdateStudentDto dto)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                throw new Exception("Student not found.");

            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;
            student.Grade = dto.Grade;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                throw new Exception("Student not found.");

            _context.Students.Remove(student);

            await _context.SaveChangesAsync();
        }

        public async Task<ImportStudentsResultDto> ImportStudentsFromXmlAsync(Stream stream)
        {
            var result = new ImportStudentsResultDto();

            var document = XDocument.Load(stream);

            var studentElements = document.Root?.Elements("Student").ToList();

            if (studentElements == null || !studentElements.Any())
            {
                result.Errors.Add("No students found in XML file.");
                return result;
            }

            foreach (var studentElement in studentElements)
            {
                var idText = studentElement.Element("Id")?.Value;
                var firstName = studentElement.Element("FirstName")?.Value;
                var lastName = studentElement.Element("LastName")?.Value;
                var gradeText = studentElement.Element("Grade")?.Value;

                if (!int.TryParse(idText, out int xmlStudentId))
                {
                    result.SkippedCount++;
                    result.Errors.Add("Student has invalid or missing Id.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(firstName) ||
                    string.IsNullOrWhiteSpace(lastName) ||
                    !int.TryParse(gradeText, out int grade))
                {
                    result.SkippedCount++;
                    result.Errors.Add($"Student with ID {xmlStudentId} is missing required fields.");
                    continue;
                }

                var alreadyExists = await _context.Students.AnyAsync(s => s.Id == xmlStudentId);

                if (alreadyExists)
                {
                    result.SkippedCount++;
                    result.Errors.Add($"Student with ID {xmlStudentId} already exists.");
                    continue;
                }

                var student = new Student
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Grade = grade
                };

                _context.Students.Add(student);
                result.ImportedCount++;
            }

            await _context.SaveChangesAsync();

            return result;
        }
    }
}