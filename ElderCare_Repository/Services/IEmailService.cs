using ElderCare_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Services
{
    public interface IEmailService
    {
        void sendEmail(EmailDto request);
    }
}
