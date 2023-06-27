using BusinessObject.Models;
using DataAccess.DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.BirdRepositories
{
    public class BirdRepository : IBirdRepository
    {
        public List<Bird> GetAll() => BirdDAO.GetBirds();
    }
}
