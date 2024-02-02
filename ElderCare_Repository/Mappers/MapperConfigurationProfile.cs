using AutoMapper;
using ElderCare_Domain.Commons;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Mappers
{
    public class MapperConfigurationProfile : Profile
    {
        public MapperConfigurationProfile()
        {
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
            CreateMap<SignInDto, Account>();
            CreateMap<AccountNotiDto, NotificationModel>()
                .ForMember(d => d.IsAndroidDevice, s => s.MapFrom(e => e.Data.IsAndroidDevice))
                .ForMember(d => d.Title, s => s.MapFrom(e => e.Data.Title))
                .ForMember(d => d.Body, s => s.MapFrom(e => e.Data.Body));
        }
    }
}
