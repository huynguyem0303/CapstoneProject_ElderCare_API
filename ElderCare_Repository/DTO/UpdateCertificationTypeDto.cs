﻿namespace ElderCare_Repository.DTO
{
    public class UpdateCertificationTypeDto
    {
        public int CertId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? Status { get; set; }
    }
}
