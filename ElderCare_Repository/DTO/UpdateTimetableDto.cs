namespace ElderCare_Repository.DTO
{
    public class UpdateTimetableDto
    {
        public int TimetableId { get; set; }

        public DateTime? ReportDate { get; set; }

        public string? Timeframe { get; set; }

        public int? Status { get; set; }

        public int? CarerId { get; set; }
    }
}
