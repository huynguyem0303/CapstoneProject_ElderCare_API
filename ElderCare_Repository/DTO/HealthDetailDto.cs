namespace ElderCare_Repository.DTO
{
    public class HealthDetailDto
    {
        public int HealthDetailId { get; set; }

        public double? Height { get; set; }

        public double? Weight { get; set; }

        public string MedicalCondition { get; set; }

        public string BloodPressure { get; set; }

        public string HeartProblems { get; set; }

        public string DiabetesType { get; set; }

        public string StomachAche { get; set; }

        public string VestibularDisorders { get; set; }

        public string Allergy { get; set; }

        public class PsychomotorHealth
        {
            public int? PsychomotorHealthId { get; set; }

            public string Description { get; set; }

            public int? Status { get; set; }

            public class Psychomotor
            {
                public string Name { get; set; }

                public string Description { get; set; }

                public int? Status { get; set; }
            }
        }
    }
}
