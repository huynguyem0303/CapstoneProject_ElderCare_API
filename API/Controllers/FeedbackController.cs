using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElderCare_Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ElderCare_Repository.DTO;
using System.Data;
using ElderCare_Service.Interfaces;
using ElderCare_Service.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ODataController
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbacksController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // GET: api/Feedbacks
        [HttpGet]
        [EnableQuery]
        [Authorize]
        public IActionResult GetFeedbacks()
        {
            var list = _feedbackService.GetAll();

            return Ok(list);
        }

    }
}
