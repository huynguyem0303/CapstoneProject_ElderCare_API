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
    public class PsychomotorService : IPsychomotorService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PsychomotorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Psychomotor> AddPsychomotorAsync(AddPsychomotorDto model)
        {
            var psychomotor = _mapper.Map<Psychomotor>(model);
            psychomotor.PsychomotorHealthId = _unitOfWork.PsychomotorRepo.GetAll().OrderBy(e => e.PsychomotorHealthId).Last().PsychomotorHealthId + 1;
            await _unitOfWork.PsychomotorRepo.AddAsync(psychomotor);
            await _unitOfWork.SaveChangeAsync();
            return psychomotor;
        }

        public async Task DeletePsychomotor(int id)
        {
            var psychomotor = await _unitOfWork.PsychomotorRepo.GetByIdAsync(id);
            if (psychomotor != null)
            {
                _unitOfWork.PsychomotorRepo.Delete(psychomotor);
            }
            await _unitOfWork.SaveChangeAsync();
        }

        public IEnumerable<Psychomotor> GetAll()
        {
            return _unitOfWork.PsychomotorRepo.GetAll();
        }

        public async Task<IEnumerable<Psychomotor>> FindAsync(Expression<Func<Psychomotor, bool>> expression, params Expression<Func<Psychomotor, object>>[] includes)
        {
            return await _unitOfWork.PsychomotorRepo.FindAsync(expression, includes);
        }

        public async Task UpdatePsychomotor(UpdatePsychomotorDto model)
        {
            _unitOfWork.PsychomotorRepo.Update(_mapper.Map<Psychomotor>(model));
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<Psychomotor?> GetById(int id)
        {
            return await _unitOfWork.PsychomotorRepo.GetByIdAsync(id);
        }
        public async Task<bool> PsychomotorExists(int id)
        {
            return await _unitOfWork.PsychomotorRepo.GetByIdAsync(id) != null;
        }
    }
}
