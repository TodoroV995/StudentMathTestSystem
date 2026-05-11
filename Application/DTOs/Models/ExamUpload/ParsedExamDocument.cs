namespace Application.Models.ExamUpload
{
    public class ParsedExamDocument
    {
        public int TeacherId { get; set; }

        public List<ParsedStudentExam> StudentExams { get; set; } = new List<ParsedStudentExam>();
    }

    public class ParsedStudentExam
    {
        public int StudentId { get; set; }

        public int ExamId { get; set; }

        public List<ParsedExamTask> Tasks { get; set; } = new List<ParsedExamTask>();
    }

    public class ParsedExamTask
    {
        public int TaskId { get; set; }

        public string Expression { get; set; } = string.Empty;

        public decimal StudentResult { get; set; }
    }
}