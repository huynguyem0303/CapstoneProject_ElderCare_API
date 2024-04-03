using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;

namespace ElderCare_Service.Interfaces
{
    public interface ISignupService
    {
        Task<Carer> SignUpCarer(CarerSignUpDto carerDto);
        Task<Account> SignUpCustomer(CustomerSignUpDto customerDto);
    }
}
