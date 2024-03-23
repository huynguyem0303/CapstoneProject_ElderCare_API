using AutoMapper;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Service.Interfaces;
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

        public async Task<Psychomotor> AddPsychomotorAsync(PsychomotorDto model)
        {
            var account = _mapper.Map<Psychomotor>(model);
            await _unitOfWork.PsychomotorRepo.AddAsync(account);
            await _unitOfWork.SaveChangeAsync();
            return account;
        }

        public async Task DeletePsychomotor(int id)
        {
            var account = await _unitOfWork.PsychomotorRepo.GetByIdAsync(id);
            if (account != null)
            {
                _unitOfWork.PsychomotorRepo.Delete(account);
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

        public async Task UpdatePsychomotor(Psychomotor account)
        {
            _unitOfWork.PsychomotorRepo.Update(account);
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
