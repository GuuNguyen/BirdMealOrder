using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTOs.SortDTO
{
    public class SortInputDTO
    {
        public string? PageType { get; set; }
        public SelectedPrice? SelectedPrice { get; set; }
        public List<int>? SelectedBirds { get; set; }
    }
}
