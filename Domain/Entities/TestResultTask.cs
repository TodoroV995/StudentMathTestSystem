namespace Domain.Entities
{
    public class TestResultTask
    {
        public int Id { get; set; }

        public int TestResultId { get; set; }

        public int TaskId { get; set; }

        public string Expression { get; set; } = string.Empty;

        public decimal ExpectedResult { get; set; }

        public decimal StudentResult { get; set; }

        public bool IsCorrect { get; set; }

        public TestResult TestResult { get; set; }
    }
}