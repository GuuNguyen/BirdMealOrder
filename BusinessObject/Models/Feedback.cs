using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public int OrderDetailId { get; set; }
        public int Rating { get; set; }
        public string Feedback1 { get; set; } = null!;
        [JsonIgnore]
        public virtual OrderDetail OrderDetail { get; set; } = null!;
    }
}
