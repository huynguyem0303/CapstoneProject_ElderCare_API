using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Service.Interfaces
{
    public interface IPsychomotorService
    {
        IEnumerable<Psychomotor> GetAll();
        Task<Psychomotor?> GetById(int id);
        Task<IEnumerable<Psychomotor>> FindAsync(Expression<Func<Psychomotor, bool>> expression, params Expression<Func<Psychomotor, object>>[] includes);
        Task<Psychomotor> AddPsychomotorAsync(PsychomotorDto model);
        Task UpdatePsychomotor(Psychomotor account);
        Task DeletePsychomotor(int id);
        Task<bool> PsychomotorExists(int id);
    }
}
