using AutoMapper;
using ElderCare_Domain.Models;
using ElderCare_Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Service.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IEnumerable<Service> GetAll()
        {
            return _unitOfWork.ServiceRepo.GetAll();
        }
        public async Task<Service?> GetById(int id)
        {
            return await _unitOfWork.ServiceRepo.GetByIdAsync(id);
        }
        public async Task<IEnumerable<Service>> FindAsync(Expression<Func<Service, bool>> expression, params Expression<Func<Service, object>>[] includes)
        {
            return await _unitOfWork.ServiceRepo.FindAsync(expression, includes);
        }

    }
}
