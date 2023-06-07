using AutoMapper;
using BusinessObject.Models;
using Repositories.DTOs;
using Repositories.ViewModel;
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
            CreateMap<Meal, CreateMeal>().ReverseMap();
            CreateMap<Meal, MealDTO>().ReverseMap();
        }
    }
}
