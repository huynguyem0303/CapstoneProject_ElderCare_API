namespace ElderCare_Repository.DTO
{
    public class UpdatePackageDto
    {
        public int PackageId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public double? Price { get; set; }
    }
}
