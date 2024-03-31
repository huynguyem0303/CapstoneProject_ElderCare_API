using AutoMapper;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Service.Services
{
    public class ReportService : IReportService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Report> AddReportAsync(AddReportDto model)
        {
            var report = _mapper.Map<Report>(model);
            report.ReportId = _unitOfWork.ReportRepo.GetAll().OrderBy(e => e.ReportId).Last().ReportId + 1;
            await _unitOfWork.ReportRepo.AddAsync(report);
            await _unitOfWork.SaveChangeAsync();
            return report;
        }

        public async Task DeleteReport(int id)
        {
            var report = await _unitOfWork.ReportRepo.GetByIdAsync(id);
            if (report != null)
            {
                _unitOfWork.ReportRepo.Delete(report);
            }
            await _unitOfWork.SaveChangeAsync();
        }

        public IEnumerable<Report> GetAll()
        {
            return _unitOfWork.ReportRepo.GetAll();
        }

        public async Task<IEnumerable<Report>> FindAsync(Expression<Func<Report, bool>> expression, params Expression<Func<Report, object>>[] includes)
        {
            return await _unitOfWork.ReportRepo.FindAsync(expression, includes);
        }

        public async Task UpdateReport(UpdateReportDto model)
        {
            _unitOfWork.ReportRepo.Update(_mapper.Map<Report>(model));
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<Report?> GetById(int id)
        {
            return await _unitOfWork.ReportRepo.GetByIdAsync(id);
        }
        public async Task<bool> ReportExists(int id)
        {
            return await _unitOfWork.ReportRepo.GetByIdAsync(id) != null;
        }
    }
}
