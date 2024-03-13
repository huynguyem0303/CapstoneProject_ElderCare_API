namespace ElderCare_Repository.DTO
{
    public class UpdateHobbyDto
    {
        public int HobbyId { get; set; }
        
        public int ElderlyId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool? Status { get; set; }
    }
}
