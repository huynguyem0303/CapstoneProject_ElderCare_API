using ElderCare_Domain.Validation;

namespace ElderCare_Repository.DTO
{
    public class AddTrackingDto
    {
        public int? TimetableId { get; set; }

        [RequiredWhenOtherIsNull("ContractServicesId")]
        public int? PackageServicesId { get; set; }

        [RequiredWhenOtherIsNull("PackageServicesId")]
        public int? ContractServicesId { get; set; }
    }
}
