using Application.DTOs.Tests;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class TestService : ITestService
    {
        private readonly AppDbContext _context;

        public TestService( AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MathTestDto>> GetAllAsync()
        {
            return await _context.MathTests
                .Select(t => new MathTestDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    NumberOfQuestions = t.NumberOfQuestions,
                    TimeLimitMinutes = t.TimeLimitMinutes
                })
                .ToListAsync();
        }

        public async Task<MathTestDto> GetByIdAsync(int id)
        {
            var test = await _context.MathTests.FindAsync(id);

            if (test == null)
                return null;

            return new MathTestDto
            {
                Id = test.Id,
                Title = test.Title,
                NumberOfQuestions = test.NumberOfQuestions,
                TimeLimitMinutes = test.TimeLimitMinutes
            };
        }

        public async Task<MathTestDto> CreateAsync(CreateMathTestDto dto)
        {
            var test = new MathTest
            {
                Title = dto.Title,
                NumberOfQuestions = dto.NumberOfQuestions,
                TimeLimitMinutes = dto.TimeLimitMinutes
            };

            _context.MathTests.Add(test);

            await _context.SaveChangesAsync();

            return new MathTestDto
            {
                Id = test.Id,
                Title = test.Title,
                NumberOfQuestions = test.NumberOfQuestions,
                TimeLimitMinutes = test.TimeLimitMinutes
            };
        }
    }
}