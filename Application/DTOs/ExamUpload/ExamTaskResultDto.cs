namespace Application.DTOs.ExamUpload
{
    public class ExamTaskResultDto
    {
        public int TaskId { get; set; }

        public string Expression { get; set; } = string.Empty;

        public decimal ExpectedResult { get; set; }

        public decimal StudentResult { get; set; }

        public bool IsCorrect { get; set; }
    }
}