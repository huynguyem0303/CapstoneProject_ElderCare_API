namespace ElderCare_Repository.DTO
{
    public class UpdatePsychomotorDto
    {
        public int PsychomotorHealthId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? Status { get; set; }
    }
}
