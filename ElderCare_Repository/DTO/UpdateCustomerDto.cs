﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.DTO
{
    public class UpdateCustomerDto
    {
        public int CustomerId { get; set; }

        public string? CustomerName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public int? Status { get; set; }
    }
}
