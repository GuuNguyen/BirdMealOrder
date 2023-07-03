﻿using BusinessObject.Models;
using DataAccess.DAOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.DTOs.FeedbackDTO;
using Repositories.Repositories.FeedbackRepositories;
using Repositories.Repositories.OrderRepositories;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private IFeedbackRepository _feedbackRepository;
        public FeedbackController(IFeedbackRepository repo) => _feedbackRepository = repo;
        [HttpPost]
        public IActionResult Create(CreateFeedbackDTO feedback)
        {
            if (OrderDetailDAO.GetOrderDetail(feedback.OrderDetailId) == null) return NotFound();
            if (!OrderDAO.CheckOrderCompleteByOrderDetailId(feedback.OrderDetailId)) return NotFound();
            _feedbackRepository.Create(feedback);
            return Ok("Create Sucessfull!");
        }
    }
}