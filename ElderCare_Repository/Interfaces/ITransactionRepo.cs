using DataAccess.Interfaces;
using ElderCare_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Interfaces
{
    public interface ITransactionRepo:IGenericRepo<Transaction>
    {
        Task CreateTrans(int cusid, int carerid, decimal amount);
    }
}
