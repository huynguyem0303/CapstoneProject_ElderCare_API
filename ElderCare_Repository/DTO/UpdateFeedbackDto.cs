using System.ComponentModel.DataAnnotations;

namespace ElderCare_Repository.DTO
{
    public class UpdateFeedbackDto
    {
        public int FeedbackId { get; set; }

        public int? CustomerId { get; set; }

        [Range(0, 5)]
        public int? Ratng { get; set; }

        public string Description { get; set; }

        public int? CarerId { get; set; }

        public int? ServiceId { get; set; }
    }
}
