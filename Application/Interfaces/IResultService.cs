using Application.DTOs.Results;

namespace Application.Interfaces
{
    public interface IResultService
    {
        Task<List<TestResultDto>> GetResultsForStudentAsync(int studentId);
        Task<List<TestResultDto>> GetAllResultsAsync();
    }
}