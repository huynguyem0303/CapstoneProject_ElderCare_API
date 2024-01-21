using AutoMapper;
using ElderCare_Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElderCare_Repository.Mappers
{
    public class MapperConfigurationProfile : Profile
    {
        protected MapperConfigurationProfile()
        {
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
        }
    }
}
