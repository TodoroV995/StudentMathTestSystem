using Application.DTOs.Tests;

namespace Application.Interfaces
{
    public interface ITestService
    {
        Task<List<MathTestDto>> GetAllAsync();

        Task<MathTestDto> GetByIdAsync(int id);

        Task<MathTestDto> CreateAsync(CreateMathTestDto dto);
    }
}