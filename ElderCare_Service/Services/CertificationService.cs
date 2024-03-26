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
    public class CertificationService : ICertificationService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CertificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Certification> AddCertificationAsync(AddCertificationTypeDto model)
        {
            var certification = _mapper.Map<Certification>(model);
            certification.CertId = _unitOfWork.CertificationRepo.GetAll().OrderBy(e => e.CertId).Last().CertId + 1;
            await _unitOfWork.CertificationRepo.AddAsync(certification);
            await _unitOfWork.SaveChangeAsync();
            return certification;
        }

        public async Task DeleteCertification(int id)
        {
            var certification = await _unitOfWork.CertificationRepo.GetByIdAsync(id);
            if (certification != null)
            {
                _unitOfWork.CertificationRepo.Delete(certification);
            }
            await _unitOfWork.SaveChangeAsync();
        }

        public IEnumerable<Certification> GetAll()
        {
            return _unitOfWork.CertificationRepo.GetAll();
        }

        public async Task<IEnumerable<Certification>> FindAsync(Expression<Func<Certification, bool>> expression, params Expression<Func<Certification, object>>[] includes)
        {
            return await _unitOfWork.CertificationRepo.FindAsync(expression, includes);
        }

        public async Task UpdateCertification(UpdateCertificationTypeDto model)
        {
            _unitOfWork.CertificationRepo.Update(_mapper.Map<Certification>(model));
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<Certification?> GetById(int id)
        {
            return await _unitOfWork.CertificationRepo.GetByIdAsync(id);
        }
        public async Task<bool> CertificationExists(int id)
        {
            return await _unitOfWork.CertificationRepo.GetByIdAsync(id) != null;
        }
    }
}
