using API.DTO;
using API.Ultils;
using AutoMapper;
using ElderCare_Domain.Models;
using ElderCare_Repository;
using ElderCare_Repository.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ODataController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpPost]
        [EnableQuery]
        [Authorize]
        public async Task<IActionResult> CreateTransaction([FromBody] TrasactionDto dto)
        {
            
                var idClaim = _unitOfWork.AccountRepository.GetMemberIdFromToken(HttpContext.User);
                var userid = await _unitOfWork.AccountRepository.GetByIdAsync(idClaim);
                dto.DateTime = DateTime.Now;
             
                var id = _unitOfWork.TransactionRepo.GetAll().OrderByDescending(i=>i.TransactionId).FirstOrDefault().TransactionId;
            Transaction obj = _mapper.Map<Transaction>(dto);
            obj.AccountId = userid.AccountId+1;
                await _unitOfWork.TransactionRepo.AddAsync(obj);

            try
            {
                await _unitOfWork.SaveChangeAsync();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Create success",
                }); ;
            }
            catch
            {
                return BadRequest("Error!");
            }
        }
    }
}
