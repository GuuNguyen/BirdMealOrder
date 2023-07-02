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
        public List<Bird> GetAllBirds()
        {
            return BirdDAO.GetAllBirds();
        }

        public Bird GetBirdById(int id)
        {
            var check = BirdDAO.GetBirdById(id);
            if(check == null)
            {
                return null;
            }
            return check;
        }

        public bool CreateBird(Bird bird)
        {
            BirdDAO.CreateBird(bird);
            return true;
        }

        public bool DeleteBird(int id)
        {
            var bird = BirdDAO.GetBirdById(id);
            if(bird == null)
            {
                return false;
            }
            BirdDAO.DeleteBird(id);
            return true;
        }

        public bool UpdateBird(Bird bird)
        {
            var check = BirdDAO.GetBirdById(bird.BirdId);
            if (check == null)
            {
                return false;
            }
            BirdDAO.UpdateBird(bird);
            return true;
        }
    }
}
