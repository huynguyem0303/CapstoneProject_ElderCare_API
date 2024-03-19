using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;

namespace ElderCare_Service.Interfaces
{
    public interface ISignupService
    {
        Task<Carer> SignInCarer(CarerSignInDto carerDto);
        Task<Account> SignInCustomer(CustomerSignInDto customerDto);
    }
}
