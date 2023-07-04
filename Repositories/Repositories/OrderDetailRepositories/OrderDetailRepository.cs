using DataAccess.DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.OrderDetailRepositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public List<object> GetAll()
        {
            return OrderDetailDAO.GetOrderDetails();
        }
    }
}
