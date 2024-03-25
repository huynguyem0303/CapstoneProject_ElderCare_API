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
    public class PackagesService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PackagesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<Package> GetAll()
        {
            return _unitOfWork.PackageRepo.GetAll();
        }

        public async Task<Package?> GetById(int id)
        {
            return await _unitOfWork.PackageRepo.GetByIdAsync(id);
        }
        public async Task<IEnumerable<Package>> FindAsync(Expression<Func<Package, bool>> expression, params Expression<Func<Package, object>>[] includes)
        {
            return await _unitOfWork.PackageRepo.FindAsync(expression, includes);
        }
    }

}
