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
            var feedback = _mapper.Map<Feedback>(feedbackDTO);
            FeedbackDAO.Create(feedback);
        }
    }
}
