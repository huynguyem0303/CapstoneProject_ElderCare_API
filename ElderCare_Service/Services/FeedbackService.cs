using AutoMapper;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ElderCare_Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Service.Services
{
    public class FeedbackService : IFeedbackService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task DeleteFeedback(int id)
        {
            var report = await _unitOfWork.FeedbackRepo.GetByIdAsync(id);
            if (report != null)
            {
                _unitOfWork.FeedbackRepo.Delete(report);
            }
            await _unitOfWork.SaveChangeAsync();
        }

        public IEnumerable<Feedback> GetAll()
        {
            return _unitOfWork.FeedbackRepo.GetAll();
        }

        public async Task<IEnumerable<Feedback>> FindAsync(Expression<Func<Feedback, bool>> expression, params Expression<Func<Feedback, object>>[] includes)
        {
            return await _unitOfWork.FeedbackRepo.FindAsync(expression, includes);
        }
        public async Task<bool> FeedbackExists(int id)
        {
            return await _unitOfWork.FeedbackRepo.GetByIdAsync(id) != null;
        }
    }
}
