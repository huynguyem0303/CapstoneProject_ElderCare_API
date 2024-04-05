using System;
using System.Collections.Generic;

namespace ElderCare_Repository.DTO
{
    public partial class CarerServiceDto
    {
        public int CarerServiceId { get; set; }

        public int? CarerId { get; set; }

        public int? ServiceId { get; set; }

        public string? ServiceName { get; set; }
    }
}
