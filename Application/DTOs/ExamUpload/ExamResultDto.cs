namespace Application.DTOs.ExamUpload
{
    public class ExamResultDto
    {
        public int StudentId { get; set; }

        public int ExamId { get; set; }

        public int TotalTasks { get; set; }

        public int CorrectTasks { get; set; }

        public decimal ScorePercentage { get; set; }

        public List<ExamTaskResultDto> Tasks { get; set; } = new List<ExamTaskResultDto>();
    }
}