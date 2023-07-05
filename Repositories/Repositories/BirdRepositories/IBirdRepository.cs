using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.BirdRepositories
{
    public interface IBirdRepository
    {
        List<Bird> GetAllBirds();
        List<Bird> GetBirdsByMealId(int id);
        Bird GetBirdById(int id);
        bool CreateBird(Bird bird);
        bool UpdateBird(Bird bird);
        bool DeleteBird(int id);
    }
}
