using Repositories.DTOs.ShippingAddressDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.ShippingAddressRepositories
{
    public interface IShippingAddressRepository
    {
        List<GetSADTO> GetAllByUserId(int userId);
        GetSADTO GetById(int id);
        bool CreateSA(CreateSADTO createSADTO);
        bool UpdateSA(UpdateSADTO updateSADTO);
        bool DeleteSA(int id);
    }
}
