using Application.DTOs.Students;

namespace Application.Interfaces
{
    public interface IStudentService
    {
        Task<List<StudentDto>> GetAllAsync();

        Task<StudentDto> GetByIdAsync(int id);

        Task<StudentDto> CreateAsync(CreateStudentDto dto);

        Task UpdateStudentAsync(int id, UpdateStudentDto dto);

        Task DeleteStudentAsync(int id);

        Task<ImportStudentsResultDto> ImportStudentsFromXmlAsync(Stream stream);
    }
}
