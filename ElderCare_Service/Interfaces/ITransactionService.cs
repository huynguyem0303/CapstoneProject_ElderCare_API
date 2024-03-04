using ElderCare_Repository.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Service.Interfaces
{
    public interface ITransactionService
    {
        Task<string> CreateTransaction(TrasactionDto dto, int accountId);
        string LinkPayment(int accountId);
        Task<string> ProcessPayment();
    }
}
