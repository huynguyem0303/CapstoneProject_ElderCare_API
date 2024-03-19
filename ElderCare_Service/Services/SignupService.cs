using AutoMapper;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Service.Interfaces;

namespace ElderCare_Service.Services
{
    public class SignupService : ISignupService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SignupService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Carer> SignInCarer(CarerSignInDto carerDto)
        {
            var carer = _mapper.Map<Carer>(carerDto);
            //var account = _mapper.Map<Account>(carerDto);
            carer.Status = (int)CarerStatus.Pending;
            await _unitOfWork.CarerRepository.AddAsync(carer);
            //account.CarerId = carer.CarerId;
            //account.RoleId = (int)AccountRole.Carer;
            //account.Status = (int)CarerStatus.Pending;
            //string randomString = Guid.NewGuid().ToString("N").Substring(0, 10);
            //account.Password = randomString;
            //await _unitOfWork.AccountRepository.AddAsync(account);
            await _unitOfWork.SaveChangeAsync();
            return carer;
        }

        public async Task<Account> SignInCustomer(CustomerSignInDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            var account = _mapper.Map<Account>(customerDto);
            customer = await _unitOfWork.CustomerRepository.AddAsync(customer);
            account.CustomerId = customer.CustomerId;
            account.RoleId = (int)AccountRole.Customer;
            account.Status = (int)AccountStatus.Active;
            await _unitOfWork.AccountRepository.AddAsync(account);
            await _unitOfWork.SaveChangeAsync();
            return account;
        }
    }
}
