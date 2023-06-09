using AutoMapper;
using BusinessObject.Models;
using Repositories.DTOs.MealDTO;
using Repositories.DTOs.OrderDTO;
using Repositories.DTOs.ProductDTO;
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
        }
    }
}
