using Repositories.DTOs.SortDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.SortRepositories
{
    public interface ISortRepository
    {
        List<object> SortList(SortInputDTO values);
    }
}
