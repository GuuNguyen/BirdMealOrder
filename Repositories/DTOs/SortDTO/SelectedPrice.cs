using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.SortDTO
{
    public class SelectedPrice
    {
        public int? Min { get; set; }
        public int? Max { get; set; }
        public int? Only { get; set; }
    }
}
