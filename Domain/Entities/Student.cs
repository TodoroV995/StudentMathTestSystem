namespace Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public int Grade { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
}
