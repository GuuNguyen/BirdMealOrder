using DataAccess.DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.SearchRepositories
{
    public class SearchRepository : ISearchRepository
    {
        public object Search(string key)
        {
            return SearchDAO.SearchAll(key);
        }
    }
}
