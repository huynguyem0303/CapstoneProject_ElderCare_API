﻿using AutoMapper;
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
    public class PackagesService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PackagesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Package> AddPackageAsync(AddPackageDto model)
        {
            var package = _mapper.Map<Package>(model);
            package.PackageId = _unitOfWork.PackageRepo.GetAll().OrderBy(e => e.PackageId).Last().PackageId + 1;
            await _unitOfWork.PackageRepo.AddAsync(package);
            await _unitOfWork.SaveChangeAsync();
            return package;
        }

        public async Task DeletePackage(int id)
        {
            var package = await _unitOfWork.PackageRepo.GetByIdAsync(id);
            if (package != null)
            {
                _unitOfWork.PackageRepo.Delete(package);
            }
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<IEnumerable<PackageDto>> GetAllAsync()
        {
            var packages = await _unitOfWork.PackageRepo.FindAsync(e => true, e => e.PackageServices);
            var services = _unitOfWork.ServiceRepo.GetAll();
            var result = _mapper.Map<List<PackageDto>>(packages);
            foreach (var item in result)
            {
                var packageServices = (from packageService in item.PackageServices
                                       join service in services on packageService.ServiceId equals service.ServiceId
                                       select service).ToList();
                item.PackageServices = _mapper.Map<List<ServiceDto>>(packageServices);
            }
            return result;
        }

        public async Task<IEnumerable<Package>> FindAsync(Expression<Func<Package, bool>> expression, params Expression<Func<Package, object>>[] includes)
        {
            return await _unitOfWork.PackageRepo.FindAsync(expression, includes);
        }

        public async Task UpdatePackage(UpdatePackageDto model)
        {
            _unitOfWork.PackageRepo.Update(_mapper.Map<Package>(model));
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<PackageDto?> GetById(int id)
        {
            var package = (await _unitOfWork.PackageRepo.FindAsync(e => e.PackageId == id, e => e.PackageServices)).FirstOrDefault();
            var result = _mapper.Map<PackageDto>(package);
            if(package != null)
            {
                var packageServices = (from packageService in package.PackageServices
                                       join service in _unitOfWork.ServiceRepo.GetAll() on packageService.ServiceId equals service.ServiceId
                                       select service).ToList();
                result.PackageServices = _mapper.Map<List<ServiceDto>>(packageServices);
            }
            return result;
        }
        public async Task<bool> PackageExists(int id)
        {
            return await _unitOfWork.PackageRepo.GetByIdAsync(id) != null;
        }
    }
}
