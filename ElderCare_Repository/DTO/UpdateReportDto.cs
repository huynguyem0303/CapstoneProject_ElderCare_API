namespace ElderCare_Repository.DTO
{
    public class UpdateReportDto
    {
        public int ReportId { get; set; }

        public int? CarerId { get; set; }

        public int? CustomerId { get; set; }

        public string? Description { get; set; }
    }
}
