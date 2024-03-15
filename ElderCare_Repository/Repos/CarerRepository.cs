using DataAccess.Repositories;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
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
            List<Carer> duplicate = new List<Carer>();
            List<CarerService> services = await _context.Set<CarerService>().Include(e => e.Service).ToListAsync();
            List<CarerService> carerService = new List<CarerService>();
            List<CarerShilft> shift = await _context.Set<CarerShilft>().ToListAsync();
            List<CarerShilft> CarerShilft = new List<CarerShilft>();
            List<CarerCategory> cate = await _context.Set<CarerCategory>().ToListAsync();
            List<CarerCategory> CarerCategory = new List<CarerCategory>();
            string separator = " ";
            string servicelist = String.Join(separator, dto.ServiceDes);
            string genderlist = String.Join(separator, dto.Gender);
            string timelist = String.Join(separator, dto.TimeShift);
            string agelist = String.Join(separator, dto.Age);
            string catelist = String.Join(separator, dto.Cate);
            if (!servicelist.IsNullOrEmpty())
            {
                for (int i = 0; i < services.Count; i++)
                {
                    if (servicelist.Contains(services[i].Service.Name))
                        carerService.Add(services[i]);
                }
                for (int i = 0; i < carerService.Count; i++)
                {
                    var carerList = await _context.Set<Carer>().Where(x => (x.CarerId == carerService[i].CarerId)).Distinct().ToListAsync(); 
                    servicecarer.AddRange(carerList);
                }
            }
                if (!timelist.IsNullOrEmpty())
            {
                for (int i = 0; i < shift.Count; i++)
                {
                    if (timelist.Contains(shift[i].Shilft.Name))
                        CarerShilft.Add(shift[i]);
                }
                for (int i = 0; i < CarerShilft.Count; i++)
                {
                    carershift = servicecarer.Where(x => (x.CarerId == CarerShilft[i].CarerId)).ToList();
                    List<Carer> duplicates = carershift.GroupBy(x => x.CarerId)
                                   .SelectMany(g => g.Skip(1)).ToList();
                }

            }
            if (!catelist.IsNullOrEmpty())
            {
                for (int i = 0; i < cate.Count; i++)
                {
                    if (catelist.Contains(cate[i].Cate.Description))
                        CarerCategory.Add(cate[i]);
                }
                for (int i = 0; i < CarerCategory.Count; i++)
                {
                    carercate = servicecarer.Where(x => (x.CarerId == CarerCategory[i].Carerid)).ToList(); ;
                }

            }


            if (!agelist.IsNullOrEmpty() && !genderlist.IsNullOrEmpty())
                for (int i = 0; i < servicecarer.Count; i++)
                {
                    if (genderlist.Contains(servicecarer[i].Gender) && agelist.Contains(servicecarer[i].Age))
                        carer.Add(servicecarer[i]);
                }
            if (genderlist.IsNullOrEmpty() && !genderlist.IsNullOrEmpty())
                for (int i = 0; i < servicecarer.Count; i++)
                {
                    if (agelist.Contains(servicecarer[i].Age))
                        carer.Add(servicecarer[i]);
                }
            if (!agelist.IsNullOrEmpty() && genderlist.IsNullOrEmpty())
                for (int i = 0; i < servicecarer.Count; i++)
                {
                    if (agelist.Contains(servicecarer[i].Age))
                        carer.Add(servicecarer[i]);
                }

            var combine = carer.Union(carershift).ToList();
            var result = combine.Union(carercate).ToList();
            return result;
        }
        public new async Task<Carer> AddAsync(Carer entity)
        {
            try
            {
                //CheckIfNullException();
                entity.CarerId = _dbSet.OrderBy(e => e.CarerId).Last().CarerId + 1;
                entity.Status = (int)AccountStatus.Active;
                if(entity.Bankinfo == null)
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
            _ = _dbSet.Select(e => e.CertificateId).ToList();
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
            return await _context.CarersCustomers.FirstOrDefaultAsync(x=>x.CarercusId == carercusId);
        }

        public async Task<CarersCustomer?> GetLastest()
        {
            return await _context.CarersCustomers.OrderByDescending(x=>x.CarercusId).FirstOrDefaultAsync();
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
    }
}
