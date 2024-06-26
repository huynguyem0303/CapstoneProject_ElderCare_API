﻿using DataAccess.Repositories;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensions.Msal;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ElderCare_Repository.Repos
{
    public class CarerRepository : GenericRepo<Carer>, ICarerRepository
    {
        public CarerRepository(ElderCareContext context) : base(context)
        {

        }

        public async Task<List<Carer?>> searchCarer(SearchCarerDto dto)
        {
            var list = GetAll().ToList();

            List<Carer> carer = new List<Carer>();
            List<Carer> carershift = new List<Carer>();
            List<Carer> carercate = new List<Carer>();
            List<Carer> servicecarer = new List<Carer>();
            List<Carer> duplicatecarer = new List<Carer>();
            List<CarerService> services = await _context.Set<CarerService>().Include(e => e.Service).ToListAsync();
            List<CarerService> carerService = new List<CarerService>();
            List<CarerShilft> shift = await _context.Set<CarerShilft>().Include(e => e.Shilft).Include(e => e.Carer).ToListAsync();
            List<CarerShilft> CarerShilft = new List<CarerShilft>();
            List<CarerShilft> duplicatescarershilf = new List<CarerShilft>();
            List<CarerCategory> duplicatescarercate = new List<CarerCategory>();
            List<CarerCategory> cate = await _context.Set<CarerCategory>().Include(e => e.Cate).Include(e => e.Carer).ToListAsync();
            List<CarerCategory> CarerCategory = new List<CarerCategory>();
            string separator = " ";
            string service = String.Join(separator, dto.ServiceDes);
            string genderlist = String.Join(separator, dto.Gender);
            string timelist = String.Join(separator, dto.TimeShift);
            string agelist = String.Join(separator, dto.Age);
            string catelist = String.Join(separator, dto.Cate);
            string districtlist = String.Join(separator, dto.District);
            if (service.IsNullOrEmpty() && genderlist.IsNullOrEmpty() && timelist.IsNullOrEmpty() && agelist.IsNullOrEmpty() && catelist.IsNullOrEmpty())
            {
                return list;
            }
            if (!service.IsNullOrEmpty())
            {
                for (int i = 0; i < services.Count; i++)
                {
                    if (dto.ServiceDes.Contains(services[i].Service.Name))
                        carerService.Add(services[i]);

                }
                for (int i = 0; i < carerService.Count; i++)
                {
                    var checkcarer = await _context.Set<Carer>().Where(x => (x.CarerId == carerService[i].CarerId)).ToListAsync();
                    if (!checkcarer.IsNullOrEmpty())
                    {
                        servicecarer.AddRange(checkcarer);
                    }
                }
            }
            if (genderlist.IsNullOrEmpty() && timelist.IsNullOrEmpty() && agelist.IsNullOrEmpty() && catelist.IsNullOrEmpty())
            {
                return servicecarer;
            }
            if (!timelist.IsNullOrEmpty())
            {
                for (int i = 0; i < shift.Count; i++)
                {
                    if (timelist.Contains(shift[i].Shilft.Name))
                        CarerShilft.Add(shift[i]);
                    for (int j = 1; j < dto.TimeShift.Length; j++)
                    {
                        var duplicateExists = CarerShilft.GroupBy(n => n.CarerId).Any(g => g.Count() == j);
                        if (duplicateExists)
                        {
                            if (duplicatescarershilf.IsNullOrEmpty())
                            {
                                duplicatescarershilf.Add(shift[i]);
                            }
                            duplicatescarershilf.Insert(0, shift[i]);
                        }
                    }

                }
                var priorityshilft = duplicatescarershilf.UnionBy(CarerShilft, x => x.CarerId).ToList();
                for (int i = 0; i < priorityshilft.Count; i++)
                {
                    var checkcarer = servicecarer.Where(x => (x.CarerId == priorityshilft[i].CarerId)).ToList();
                    if (!checkcarer.IsNullOrEmpty())
                    {
                        carershift.AddRange(checkcarer);
                    }
                }
            }
            if (!catelist.IsNullOrEmpty())
            {
                for (int i = 0; i < cate.Count; i++)
                {
                    if (catelist.Contains(cate[i].Cate.Description))
                        CarerCategory.Add(cate[i]);
                    for (int j = 1; j < dto.Cate.Length; j++)
                    {
                        var duplicateExists = CarerCategory.GroupBy(n => n.Carerid).Any(g => g.Count() == j);
                        if (duplicateExists)
                        {
                            if (duplicatescarercate.IsNullOrEmpty())
                            {
                                duplicatescarercate.Add(cate[i]);
                            }
                            duplicatescarercate.Insert(0, cate[i]);
                        }
                    }
                }
                var prioritycate = duplicatescarercate.UnionBy(CarerCategory, x => x.Carerid).ToList();
                for (int i = 0; i < prioritycate.Count; i++)
                {
                    var checkcarer = servicecarer.Where(x => (x.CarerId == prioritycate[i].Carerid)).ToList();
                    if (!checkcarer.IsNullOrEmpty())
                    {
                        carercate.AddRange(checkcarer);
                    }
                }
            }


            if (!agelist.IsNullOrEmpty() && !genderlist.IsNullOrEmpty() && !districtlist.IsNullOrEmpty())
            {
                for (int i = 0; i < servicecarer.Count; i++)
                {
                    if (genderlist.Contains(servicecarer[i].Age) && districtlist.Contains(servicecarer[i].Address) && agelist.Contains(servicecarer[i].Gender))
                        carer.Add(servicecarer[i]);
                }
            }
            if (!agelist.IsNullOrEmpty() && !genderlist.IsNullOrEmpty())
            {
                for (int i = 0; i < servicecarer.Count; i++)
                {
                    if (genderlist.Contains(servicecarer[i].Gender) && agelist.Contains(servicecarer[i].Age))
                        carer.Add(servicecarer[i]);
                }

            }
            if (!genderlist.IsNullOrEmpty() && !districtlist.IsNullOrEmpty())
            {
                for (int i = 0; i < servicecarer.Count; i++)
                {
                    if (genderlist.Contains(servicecarer[i].Gender) && districtlist.Contains(servicecarer[i].Address))
                        carer.Add(servicecarer[i]);
                }
            }
            if (!agelist.IsNullOrEmpty() && !districtlist.IsNullOrEmpty())
            {
                for (int i = 0; i < servicecarer.Count; i++)
                {
                    if (districtlist.Contains(servicecarer[i].Address) && agelist.Contains(servicecarer[i].Age))
                        carer.Add(servicecarer[i]);
                }
            }



            if (!genderlist.IsNullOrEmpty())
            {
                for (int i = 0; i < servicecarer.Count; i++)
                {
                    if (genderlist.Contains(servicecarer[i].Gender))
                        carer.Add(servicecarer[i]);
                }

            }
            if (!districtlist.IsNullOrEmpty())
            {
                for (int i = 0; i < servicecarer.Count; i++)
                {
                    if (districtlist.Contains(servicecarer[i].Address))
                        carer.Add(servicecarer[i]);
                }
            }
            if (!agelist.IsNullOrEmpty())
            {
                for (int i = 0; i < servicecarer.Count; i++)
                {
                    if (agelist.Contains(servicecarer[i].Age))
                        carer.Add(servicecarer[i]);
                }
            }
            var combine1 = carercate.Concat(carershift).ToList();
            if (!combine1.IsNullOrEmpty())
            {
                var duplicate = new List<Carer>();
                for (int j = 1; j < combine1.Count; j++)
                {

                    var duplicateExists = combine1.GroupBy(n => n.CarerId).Any(g => g.Count() == j);
                    if (duplicateExists)
                    {

                        duplicate.Insert(0, combine1[j]);

                    }
                }
                combine1 = duplicate.Union(combine1).ToList();

            }
            var combine2 = carercate.Concat(carer).ToList();
            if (!combine2.IsNullOrEmpty())
            {
                var duplicate = new List<Carer>();
                for (int j = 1; j < combine1.Count; j++)
                {
                    var duplicateExists = combine2.GroupBy(n => n.CarerId).Any(g => g.Count() == j);
                    if (duplicateExists)
                    {
                        duplicate.Insert(0, combine2[j]);
                    }
                }
                combine2 = duplicate.Union(combine2).ToList();
            }
            var combine3 = carershift.Concat(carer).ToList();
            if (!combine3.IsNullOrEmpty())
            {
                var duplicate = new List<Carer>();
                for (int j = 1; j < combine3.Count; j++)
                {
                    var duplicateExists = combine3.GroupBy(n => n.CarerId).Any(g => g.Count() == j);
                    if (duplicateExists)
                    {

                        duplicate.Insert(0, combine3[j]);
                    }
                }
                combine3 = duplicate.Union(combine3).ToList();
            }
            var result = combine1.Concat(combine2).Concat(combine3).ToList();
            if (!result.IsNullOrEmpty())
            {
                var duplicate = new List<Carer>();
                for (int j = 1; j < result.Count; j++)
                {
                    var duplicateExists = result.GroupBy(n => n.CarerId).Any(g => g.Count() == j);
                    if (duplicateExists)
                    {

                        duplicate.Insert(0, result[j]);
                    }
                }
                result = duplicate.Union(result).ToList();
                return result;
            }

            return null;
        }
        public new async Task<Carer> AddAsync(Carer entity)
        {
            try
            {
                //CheckIfNullException();
                if (await _dbSet.AnyAsync(e => e.Email == entity.Email!.Trim()) || await _context.Accounts.AnyAsync(e => e.Email == entity.Email!.Trim()))
                {
                    throw new DuplicateNameException("This email has already been registered");
                }
                entity.CarerId = _dbSet.OrderBy(e => e.CarerId).Last().CarerId + 1;
                entity.Status = (int)CarerStatus.Pending;
                if (entity.Bankinfo == null)
                {
                    throw new Exception("Missing Bank Info");
                }
                entity.Bankinfo.BankinfoId = _context.Bankinformations.OrderBy(e => e.BankinfoId).Last().BankinfoId + 1;
                await _dbSet.AddAsync(entity);
            }
            catch (DbUpdateException)
            {
                throw new Exception(message: "This has already been added");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return entity;
        }
        private void CheckIfNullException()
        {
            _ = _dbSet.Select(e => e.Name).ToList();
            _ = _dbSet.Select(e => e.Email).ToList();
            _ = _dbSet.Select(e => e.Phone).ToList();
            _ = _dbSet.Select(e => e.Gender).ToList();
            _ = _dbSet.Select(e => e.Age).ToList();
            _ = _dbSet.Select(e => e.Status).ToList();
            _ = _dbSet.Select(e => e.Image).ToList();
            //_ = _dbSet.Select(e => e.CertificateId).ToList();
            _ = _dbSet.Select(e => e.BankinfoId).ToList();
            //_ = _dbSet.Select(e => e.TransactionId).ToList();
            _ = _dbSet.Select(e => e.Bankinfo).ToList();
            _ = _context.Bankinformations.Select(e => e.BankinfoId).ToList();
            _ = _context.Bankinformations.Select(e => e.AccountName).ToList();
            _ = _context.Bankinformations.Select(e => e.AccountNumber).ToList();
            _ = _context.Bankinformations.Select(e => e.BankName).ToList();
            _ = _context.Bankinformations.Select(e => e.Branch).ToList();
        }

        public async Task<List<Transaction>> GetCarerTransaction(int carerId)
        {
            var carerCusIdList = await _context.CarersCustomers.Where(x => x.CarerId == carerId).Select(x => x.CarercusId).ToListAsync();
            if (carerCusIdList.IsNullOrEmpty())
            {
                throw new Exception("Empty transaction history");
            }
            var transactionList = await _context.Transactions.Where(x => carerCusIdList.Contains((int)x.CarercusId!)).ToListAsync();
            return transactionList;
        }
        public async Task<List<Customer>> GetCarerCusByCarerId(int carerId)
        {
            var result = new List<Customer>();
            var carerCusIdList = await _context.CarersCustomers.Where(x => x.CarerId == carerId).Select(x => x.CarercusId).ToListAsync();
            if (carerCusIdList.IsNullOrEmpty())
            {
                throw new Exception("Empty list");
            }
            var transactionList = await _context.Transactions.Where(x => carerCusIdList.Contains((int)x.CarercusId!)).ToListAsync();
            for (int i = 0; i < transactionList.Count; i++)
            {
                if (transactionList[i].Status.Equals("APPROVE"))
                {
                    var approvecarerCusIdList = await _context.CarersCustomers.Where(x => x.CarercusId == transactionList[i].CarercusId).Select(x => x.CustomerId).ToListAsync();
                    var cuslist = await _context.Customers.Where(x => approvecarerCusIdList.Contains((int)x.CustomerId!)).FirstOrDefaultAsync();
                    var anyDuplicate = result.Where(x => x.CustomerId == cuslist.CustomerId).FirstOrDefault();
                    if (anyDuplicate == null)
                    {
                        result.Add(cuslist);
                    }
                }

            }
            return result;
        }
        public async Task<List<Carer>> GetCarerCusByCusId(int cusId)
        {
            var result = new List<Carer>();
            var carerCusIdList = await _context.CarersCustomers.Where(x => x.CustomerId == cusId).Select(x => x.CarercusId).ToListAsync();
            if (carerCusIdList.IsNullOrEmpty())
            {
                throw new Exception("Empty list");
            }
            var transactionList = await _context.Transactions.Where(x => carerCusIdList.Contains((int)x.CarercusId!)).ToListAsync();
            for (int i = 0; i < transactionList.Count; i++)
            {
                if (transactionList[i].Status.Equals("APPROVE"))
                {
                    var approvecarerCusIdList = await _context.CarersCustomers.Where(x => x.CarercusId == transactionList[i].CarercusId).Select(x => x.CarerId).ToListAsync();
                    var carerlist = await _context.Carers.Where(x => approvecarerCusIdList.Contains((int)x.CarerId!)).FirstOrDefaultAsync();
                    var anyDuplicate = result.Where(x => x.CarerId == carerlist.CarerId).FirstOrDefault();
                    if (anyDuplicate == null)
                    {
                        result.Add(carerlist);
                    }
                }

            }
            return result;
        }
        public async Task<List<Transaction>> GetCustomerTransaction(int customerId)
        {
            var carerCusIdList = await _context.CarersCustomers.Where(x => x.CustomerId == customerId).Select(x => x.CarercusId).ToListAsync();
            if (carerCusIdList.IsNullOrEmpty())
            {
                throw new Exception("Empty transaction history");
            }
            var transactionList = await _context.Transactions.Where(x => carerCusIdList.Contains((int)x.CarercusId!)).ToListAsync();
            return transactionList;
        }

        public async Task<CarersCustomer?> GetCarerCustomerFromIdAsync(int? carercusId)
        {
            return await _context.CarersCustomers.FirstOrDefaultAsync(x => x.CarercusId == carercusId);
        }

        public async Task<CarersCustomer?> GetLastest()
        {
            return await _context.CarersCustomers.OrderByDescending(x => x.CarercusId).FirstOrDefaultAsync();
        }

        public async Task<CarersCustomer> AddCarerCusAsync(CarersCustomer entity)
        {
            try
            {
                await _context.CarersCustomers.AddAsync(entity);
            }
            catch (DbUpdateException)
            {
                throw new Exception(message: "This has already been added");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return entity;
        }

        public async Task<CarersCustomer?> FindAsync(int carerid, int cusid)
        {
            return await _context.CarersCustomers.FirstOrDefaultAsync(x => x.CarerId == carerid && x.CustomerId == cusid);
        }

        public async Task<List<Carer>> GetPendingCarerAsync()
        {
            return await _context.Carers.Where(x => x.Status == 0).ToListAsync();
        }

        public async Task<List<CarerService>> AddCarerService(int carerId, string[] serviceName)
        {
            var errors = new List<string>();
            var list = new List<CarerService>();
            int lastId = (_context.CarerServices.OrderBy(e => e.CarerServiceId).Last() ?? new CarerService()).CarerServiceId;
            for (var i = 0; i < serviceName.Length; i++)
            {
                var service = await _context.Services.FirstOrDefaultAsync(e => e.Name == serviceName[i]);
                if (service == null)
                {
                    errors.Add($"serviceName[{i}]:'{serviceName[i]}' is incorrect");
                }
                else
                {
                    if (_context.CarerServices.Where(e => e.CarerId == carerId && e.ServiceId == service.ServiceId).Any())
                    {
                        errors.Add($"serviceName[{i}]:'{serviceName[i]}' existed in the carer");
                    }
                    else
                    {
                        list.Add(new CarerService()
                        {
                            CarerServiceId = ++lastId,
                            ServiceId = service.ServiceId,
                            CarerId = carerId,
                        });
                        
                    }
                }
            }
            if (errors.Count > 0)
            {
                throw new DbUpdateException(message: String.Join(",\n", errors));
            }
            list = list.DistinctBy(e => e.ServiceId).ToList();
            if (list.Count > 0)
            {
                await _context.CarerServices.AddRangeAsync(list);
            }
            return list;
        }

        public async Task RemoveCarerService(int carerId, int serviceId)
        {
            var carerService = await _context.CarerServices.FirstOrDefaultAsync(e => e.ServiceId == serviceId && e.CarerId == carerId);
            if (carerService != null)
            {
                _context.CarerServices.Remove(carerService);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public async Task<CarerService?> GetCarerService(int carerId, int serviceId)
        {
            return await _context.CarerServices.FirstOrDefaultAsync(e => e.CarerId == carerId && e.ServiceId == serviceId);
        }

        public IEnumerable<Carer> GetCarerByServiceId(int serviceId)
        {
            var result = from carer in _dbSet
                         join carerService in _context.CarerServices
                         on carer.CarerId equals carerService.CarerId
                         where carerService.ServiceId == serviceId
                         select carer;
            return result;
        }

        public IEnumerable<Carer> GetCarersByCustomerId(int customerId)
        {
            var result = from carerCustomer in _context.CarersCustomers
                         where carerCustomer.CustomerId == customerId
                         join carer in _dbSet on carerCustomer.CarerId equals carer.CarerId
                         select carer;
            return result;
        }
    }
}
