using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.FeedbackDTO
{
    public class CreateFeedbackDTO
    {
        public int OrderDetailId { get; set; }
        public int Rating { get; set; }
        public string Feedback { get; set; }
    }
}
