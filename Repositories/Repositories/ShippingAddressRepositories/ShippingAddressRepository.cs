using AutoMapper;
using BusinessObject.Models;
using DataAccess.DAOs;
using Repositories.DTOs.ShippingAddressDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Repositories.Repositories.ShippingAddressRepositories
{
    public class ShippingAddressRepository : IShippingAddressRepository
    {
        private readonly IMapper _mapper;

        public ShippingAddressRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public List<GetSADTO> GetAllByUserId(int userId)
        {
            var list = ShippingAddressDAO.GetListSAByUserId(userId);
            return _mapper.Map<List<GetSADTO>>(list);
        }

        public GetSADTO GetById(int id)
        {
            var sa = ShippingAddressDAO.GetSAById(id);
            return _mapper.Map<GetSADTO>(sa);
        }

        public bool CreateSA(CreateSADTO createSADTO)
        {
            var isNull = createSADTO.GetType()
                                    .GetProperties()
                                    .Where(pi => pi.PropertyType == typeof(string))
                                    .Select(pi => (string)pi.GetValue(createSADTO))
                                    .Any(value => string.IsNullOrEmpty(value));
            if (!isNull)
            {
                var mappedSA = _mapper.Map<ShippingAddress>(createSADTO);
                ShippingAddressDAO.CreateSA(mappedSA);
                return true;
            }
            return false;
        }

        public bool UpdateSA(UpdateSADTO updateSADTO)
        {
            var sa = ShippingAddressDAO.GetSAById(updateSADTO.ShippingAddressId);
            if (sa == null) return false;
            var mappedSA = _mapper.Map(updateSADTO, sa);
            ShippingAddressDAO.UpdateSA(mappedSA);
            return true;
        }

        public bool DeleteSA(int id)
        {
            var sa = ShippingAddressDAO.GetSAById(id);
            if (sa == null) return false;
            ShippingAddressDAO.Delete(id);
            return true;
        }
    }
}
