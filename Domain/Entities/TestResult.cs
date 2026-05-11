using Domain.Enums;

namespace Domain.Entities
{
    public class TestResult
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public Student Student { get; set; }

        public int MathTestId { get; set; }

        public MathTest MathTest { get; set; }

        public int Score { get; set; }

        public int TotalQuestions { get; set; }

        public TestStatus Status { get; set; } = TestStatus.NotStarted;

        public DateTime StartedAt { get; set; } = DateTime.Now;

        public DateTime? CompletedAt { get; set; }

        public ICollection<TestResultTask> Tasks { get; set; } = new List<TestResultTask>();
    }
}
