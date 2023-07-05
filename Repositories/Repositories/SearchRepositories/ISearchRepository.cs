using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.SearchRepositories
{
    public interface ISearchRepository
    {
        object Search(string key);
    }
}
