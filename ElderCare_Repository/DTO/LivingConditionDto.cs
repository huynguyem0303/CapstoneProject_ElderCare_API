namespace ElderCare_Repository.DTO
{
    public class LivingConditionDto
    {
        public int ElderlyId { get; set; }

        public int LivingconId { get; set; }

        public bool? LiveWithRelative { get; set; }

        public string Regions { get; set; }

        public bool? HaveSeperateRoom { get; set; }

        public string? Others { get; set; }
    }
}
