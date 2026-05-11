namespace Application.DTOs.ExamUpload
{
    public class ExamUploadResultDto
    {
        public int TeacherId { get; set; }

        public int TotalStudents { get; set; }

        public int TotalExams { get; set; }

        public List<ExamResultDto> Results { get; set; } = new List<ExamResultDto>();
    }
}