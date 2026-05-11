namespace Domain.Entities
{
    public class MathTest
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;


        public int NumberOfQuestions { get; set; }

        public int TimeLimitMinutes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
}
