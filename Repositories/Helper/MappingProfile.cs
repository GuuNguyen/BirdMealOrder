using AutoMapper;
using BusinessObject.Models;
using Repositories.DTOs.FeedbackDTO;
using Repositories.DTOs.MealDTO;
using Repositories.DTOs.OrderDTO;
using Repositories.DTOs.ProductDTO;
using Repositories.DTOs.ShippingAddressDTO;
using Repositories.DTOs.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<T, TDTO>().ReverseMap();
            CreateMap<Meal, CreateMealDTO>().ReverseMap();
            CreateMap<Meal, MealDTO>().ReverseMap();
            CreateMap<Order, UpdateOrderDTO>().ReverseMap();
            CreateMap<Product, CreateProductDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<User, CreateUserDTO>().ReverseMap();
            CreateMap<Meal, UpdateMealDTO>().ReverseMap();
            CreateMap<User, CreateUserDTOFull>().ReverseMap();
            CreateMap<User, UpdateUserDTO>().ReverseMap();
            CreateMap<Feedback, CreateFeedbackDTO>().ReverseMap();
            CreateMap<ShippingAddress, GetSADTO>().ReverseMap();     
            CreateMap<ShippingAddress, UpdateSADTO>().ReverseMap();     
            CreateMap<ShippingAddress, CreateSADTO>().ReverseMap();     
            CreateMap<Order, ChangeOrderStatusDTO>().ReverseMap(); 
        }
    }
}
