using AutoMapper;
using Domain.DTO;
using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Helper;

namespace Domain.Helper
{
    public class MappingProfile:Profile
    {
        
        public MappingProfile()
        {
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.PasswordHash,
                    opt => opt.MapFrom(src => PasswordHelper.HashPassword(src.Password)))
                .ForMember(dest => dest.Role,
                    opt => opt.MapFrom(src => UserRole.User));
        }
    }
}
