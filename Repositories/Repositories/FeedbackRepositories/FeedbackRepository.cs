using AutoMapper;
using BusinessObject.Models;
using DataAccess.DAOs;
using Repositories.DTOs.FeedbackDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.FeedbackRepositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly IMapper _mapper;

        public FeedbackRepository(IMapper mapper) => _mapper = mapper;
        public void Create(CreateFeedbackDTO feedbackDTO)
        {
            var feedback = new Feedback
            {
                OrderDetailId = feedbackDTO.OrderDetailId,
                Rating = feedbackDTO.Rating,
                Feedback1 = feedbackDTO.Feedback
            };
            FeedbackDAO.Create(feedback);
        }

        public object GetListFeedbackByMeald(int mealId)
        {
            return FeedbackDAO.GetListFeedbackByMeald(mealId);
        }

        public object GetListFeedbackByProductId(int productId)
        {
            return FeedbackDAO.GetListFeedbackByProductId(productId);
        }
    }
}
