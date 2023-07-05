using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.OrderDetailRepositories
{
    public interface IOrderDetailRepository
    {
        List<object> GetAll();
        List<OrderDetail> ListOderDetailByOderId(int id);
    }
}
