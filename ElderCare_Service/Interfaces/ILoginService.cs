using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Service.Interfaces
{
    public interface ILoginService
    {
        Task<string> LoginCusAsync(string email, string password, string FCMToken);
        Task<string> LoginCarerAsync(string email, string password, string FCMToken);
        Task<string> LoginStaffAsync(string email, string password, string FCMToken);

    }
}
