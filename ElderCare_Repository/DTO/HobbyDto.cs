namespace ElderCare_Repository.DTO
{
    public class HobbyDto
    {
        public int HobbyId { get; set; }

        public int ElderlyId { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public int? Status { get; set; }
    }
}
