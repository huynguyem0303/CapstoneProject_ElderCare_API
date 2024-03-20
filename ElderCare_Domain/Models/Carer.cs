using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Carer
{
    public int CarerId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Gender { get; set; }

    public string? Age { get; set; }

    public int? Status { get; set; }

    public string? Image { get; set; }
    public string? Address { get; set; }

    //public int? CertificateId { get; set; }

    public int? BankinfoId { get; set; }

    public virtual Bankinformation? Bankinfo { get; set; }

    public virtual ICollection<CarerService> CarerServices { get; set; } = new List<CarerService>();

    public virtual ICollection<CarersCustomer> CarersCustomers { get; set; } = new List<CarersCustomer>();

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual ICollection<CertificationCarer> Certifications { get; set; }
}
