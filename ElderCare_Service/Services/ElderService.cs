using AutoMapper;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Service.Interfaces;
using System.Linq.Expressions;

namespace ElderCare_Service.Services
{
    public class ElderService : IElderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ElderService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Elderly> AddELderlyAsync(AddElderDto model)
        {
            var elder = _mapper.Map<Elderly>(model);
            var id = _unitOfWork.ElderRepo.GetAll().OrderByDescending(i => i.ElderlyId).FirstOrDefault().ElderlyId;
            elder.ElderlyId = id + 1;
            await _unitOfWork.ElderRepo.AddAsync(elder);
            await _unitOfWork.SaveChangeAsync();
            return elder;
        }

        public async Task DeleteElderly(int id)
        {
            var elder = await _unitOfWork.ElderRepo.GetByIdAsync(id);
            if (elder != null)
            {
                _unitOfWork.ElderRepo.Delete(elder);
            }
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<bool> ElderExists(int id)
        {
            return await _unitOfWork.ElderRepo.GetByIdAsync(id) != null;
        }

        public async Task<IEnumerable<Elderly>> FindAsync(Expression<Func<Elderly, bool>> expression, params Expression<Func<Elderly, object>>[] includes)
        {
            return await _unitOfWork.ElderRepo.FindAsync(expression, includes);
        }

        public IEnumerable<Elderly> GetAll()
        {
            return _unitOfWork.ElderRepo.GetAll();
        }

        public async Task<Elderly?> GetById(int id)
        {
            return await _unitOfWork.ElderRepo.GetByIdAsync(id);
        }

        public async Task UpdateElderly(Elderly elderly)
        {
            _unitOfWork.ElderRepo.Update(elderly);
            await _unitOfWork.SaveChangeAsync();
        }
    }
}
