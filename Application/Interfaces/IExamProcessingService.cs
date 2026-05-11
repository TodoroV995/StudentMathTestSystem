using Application.DTOs.ExamUpload;

namespace Application.Interfaces
{
    public interface IExamProcessingService
    {
        Task<ExamUploadResultDto> ProcessXmlAsync(Stream xmlStream);
    }
}