using AutoMapper;
using ElderCare_Domain.Commons;
using ElderCare_Domain.Enums;
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
            CreateMap<AddElderDto, Elderly>();
            CreateMap<UpdateElderDto, Elderly>();
            CreateMap<LivingConditionDto, LivingCondition>().ReverseMap();
            CreateMap<HobbyDto, Hobby>().ReverseMap();
            CreateMap<AddElderDto.AddHobbyDto, Hobby>().ReverseMap();
            CreateMap<AddElderHobbyDto, Hobby>().ReverseMap();
            CreateMap<Elderly, ElderViewDto>().ReverseMap();
            CreateMap<Hobby, ElderViewDto.HobbyDto>().ReverseMap();
            CreateMap<UpdateHealthDetailDto, HealthDetail>().ReverseMap();
            CreateMap<UpdateHealthDetailDto, PsychomotorHealth>().ForMember(d => d.HealthDetailId, s => s.MapFrom(e => e.HealthDetailId));
            CreateMap<AddHealthDetailDto, HealthDetail>().ForMember(d => d.PsychomotorHealths, s => s.MapFrom(e => e.PsychomotorHealthDetails));
            CreateMap<AddHealthDetailDto.AddPsychomotorHealthDto, PsychomotorHealth>();
            CreateMap<AddHealthDetailDto, PsychomotorHealth>();
            CreateMap<HealthDetailDto, HealthDetail>().ForMember(d => d.PsychomotorHealths, s => s.MapFrom(e => e.PsychomotorHealthDetails)).ReverseMap();
            CreateMap<PsychomotorHealthDto, PsychomotorHealth>().ReverseMap();

            CreateMap<Transaction, TrasactionDto>().ReverseMap().ForMember(des => des.Type,
                opt => opt.MapFrom(src => EnumMapper<TransactionType>.MapType(src.Type)));
            CreateMap<Transaction, CarerTransactionDto>().ReverseMap();
            CreateMap<AccountNotiDto, NotificationModel>()
                .ForMember(d => d.IsAndroidDevice, s => s.MapFrom(e => e.Data.IsAndroidDevice))
                .ForMember(d => d.Title, s => s.MapFrom(e => e.Data.Title))
                .ForMember(d => d.Body, s => s.MapFrom(e => e.Data.Body));
            //carer sign in
            CreateMap<CarerSignInDto, Carer>()
                .ForMember(d => d.Name, s => s.MapFrom(e => e.Name))
                .ForMember(d => d.Email, s => s.MapFrom(e => e.Email))
                .ForMember(d => d.Phone, s => s.MapFrom(e => e.PhoneNumber))
                .ForMember(d => d.Age, s => s.MapFrom(e => e.Age))
                .ForMember(d => d.Bankinfo, s => s.MapFrom(e => e.BankInfo))
                .ForMember(d => d.Gender, s => s.MapFrom(e => e.Gender))
                .ForMember(d => d.Image, s => s.MapFrom(e => e.Image));
            CreateMap<CarerSignInDto.BankInfomation, Bankinformation>();
            CreateMap<CarerSignInDto, Account>()
                .ForMember(d => d.Email, s => s.MapFrom(e => e.Email))
                .ForMember(d => d.Username, s => s.MapFrom(e => e.Name))
                .ForMember(d => d.Address, s => s.MapFrom(e => e.Address))
                .ForMember(d => d.PhoneNumber, s => s.MapFrom(e => e.PhoneNumber));
            //customer sign in
            CreateMap<CustomerSignInDto, Customer>()
                .ForMember(d => d.CustomerName, s => s.MapFrom(e => e.Name))
                .ForMember(d => d.Email, s => s.MapFrom(e => e.Email))
                .ForMember(d => d.Phone, s => s.MapFrom(e => e.PhoneNumber))
                .ForMember(d => d.Bankinfo, s => s.MapFrom(e => e.BankInfo));
            CreateMap<CustomerSignInDto.CustomerBankInfomation, Bankinformation>();
            CreateMap<CustomerSignInDto, Account>()
                .ForMember(d => d.Email, s => s.MapFrom(e => e.Email))
                .ForMember(d => d.Username, s => s.MapFrom(e => e.Name))
                .ForMember(d => d.Address, s => s.MapFrom(e => e.Address))
                .ForMember(d => d.Password, s => s.MapFrom(e => e.Password))
                .ForMember(d => d.PhoneNumber, s => s.MapFrom(e => e.PhoneNumber));
        }
    }
}
