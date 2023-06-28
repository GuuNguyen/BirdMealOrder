using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Bird
    {
        public int BirdId { get; set; }
        public string BirdName { get; set; } = null!;
        public string? BirdImage { get; set; }
    }
}
