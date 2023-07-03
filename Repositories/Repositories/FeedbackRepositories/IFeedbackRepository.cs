using BusinessObject.Models;
using Repositories.DTOs.FeedbackDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.FeedbackRepositories
{
    public interface IFeedbackRepository
    {
        void Create(CreateFeedbackDTO feedbackDTO);
    }
}
