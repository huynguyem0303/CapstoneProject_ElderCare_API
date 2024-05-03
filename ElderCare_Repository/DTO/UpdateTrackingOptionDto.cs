using ElderCare_Domain.Validation;

namespace ElderCare_Repository.DTO
{
    public class UpdateTrackingOptionDto
    {
        public int TrackingOptionId { get; set; }
        public int ServiceId { get; set; }
        [StringRange(AllowableValues = new[] { "text", "audio", "image" }, AllowNull = false)]
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
