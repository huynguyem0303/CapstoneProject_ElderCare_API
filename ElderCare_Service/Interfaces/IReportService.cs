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
    public interface IReportService
    {
        IEnumerable<Report> GetAll();
        Task<Report?> GetById(int id);
        Task<IEnumerable<Report>> FindAsync(Expression<Func<Report, bool>> expression, params Expression<Func<Report, object>>[] includes);
        Task<Report> AddReportAsync(AddReportDto model);
        Task UpdateReport(UpdateReportDto model);
        Task DeleteReport(int id);
        Task<bool> ReportExists(int id);
    }
}
