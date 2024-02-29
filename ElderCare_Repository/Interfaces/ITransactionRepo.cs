using DataAccess.Interfaces;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Interfaces
{
    public interface ITransactionRepo:IGenericRepo<Transaction>
    {
        Task<List<Transaction>> GetAllTransactions();
        Task<Transaction> GetLastestTransaction(int id);
        Task<Transaction> GetTransaction(long id);
        Task UpdateOrderInfoInDatabase(Transaction transaction);

    }
}
