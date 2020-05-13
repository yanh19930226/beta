using ServiceB.DTO.Test;
using ServiceB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace ServiceB.Profiles
{
    public class TestProfile : Profile
    {
        public  TestProfile()
        {
            CreateMap<TestModel, TestDTO>()
                .ForMember(p => p.TName, src => src.MapFrom(p => p.Name));
        }
    }
}
