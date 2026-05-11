using Application.Models.ExamUpload;

namespace Application.Interfaces
{
    public interface IExamXmlParser
    {
        Task<ParsedExamDocument> ParseAsync(Stream xmlStream);
    }
}