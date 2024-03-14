using System;
using System.Collections.Generic;

namespace ElderCare_Domain.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public int? Status { get; set; }

    public int? BankinfoId { get; set; }

    public virtual Bankinformation Bankinfo { get; set; } = new Bankinformation();

    public virtual ICollection<CarersCustomer> CarersCustomers { get; set; } = new List<CarersCustomer>();

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<Elderly> Elderlies { get; set; } = new List<Elderly>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}
