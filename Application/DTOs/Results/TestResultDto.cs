namespace Application.DTOs.Results
{
    public class TestResultDto
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public int MathTestId { get; set; }

        public int Score { get; set; }

        public int TotalQuestions { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime StartedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public List<TestResultTaskDto> Tasks { get; set; } = new List<TestResultTaskDto>();
    }

    public class TestResultTaskDto
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public string Expression { get; set; } = string.Empty;

        public decimal ExpectedResult { get; set; }

        public decimal StudentResult { get; set; }

        public bool IsCorrect { get; set; }
    }
}