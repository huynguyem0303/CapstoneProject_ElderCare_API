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
    public interface ICertificationService
    {
        IEnumerable<Certification> GetAll();
        Task<Certification?> GetById(int id);
        Task<IEnumerable<Certification>> FindAsync(Expression<Func<Certification, bool>> expression, params Expression<Func<Certification, object>>[] includes);
        Task<Certification> AddCertificationAsync(AddCertificationTypeDto model);
        Task UpdateCertification(UpdateCertificationTypeDto model);
        Task DeleteCertification(int id);
        Task<bool> CertificationExists(int id);
    }
}
