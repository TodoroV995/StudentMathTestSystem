namespace Application.DTOs.Students
{
    public class ImportStudentsResultDto
    {
        public int ImportedCount { get; set; }

        public int SkippedCount { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}