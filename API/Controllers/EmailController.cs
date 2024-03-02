using AutoMapper;
using ElderCare_Service;
using ElderCare_Repository.DTO;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
      

        public EmailController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
          
        }
     
        [HttpPost]
        public IActionResult SendEmail(EmailDto request)
        {
            _unitOfWork.emailService.sendEmail(request);
            return Ok();
        }
    }
}
