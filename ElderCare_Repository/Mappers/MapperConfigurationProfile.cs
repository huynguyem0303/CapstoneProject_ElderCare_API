﻿using AutoMapper;
using ElderCare_Domain.Commons;
using ElderCare_Domain.Enums;
using ElderCare_Domain.Models;
using ElderCare_Repository.DTO;
using ExpoCommunityNotificationServer.Models;
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
            CreateMap<Carer, CarerViewDto>().ReverseMap();
            CreateMap<AddElderDto.AddHobbyDto, Hobby>().ReverseMap();
            CreateMap<AddElderHobbyDto, Hobby>().ReverseMap();
            CreateMap<Elderly, ElderViewDto>().ReverseMap();
            CreateMap<Hobby, ElderViewDto.HobbyDto>().ReverseMap();
            CreateMap<UpdateHealthDetailDto, HealthDetail>().ReverseMap();
            CreateMap<UpdateHealthDetailDto, PsychomotorHealth>().ForMember(d => d.HealthDetailId, s => s.MapFrom(e => e.HealthDetailId));
            CreateMap<AddHealthDetailDto, HealthDetail>().ForMember(d => d.PsychomotorHealths, s => s.MapFrom(e => e.PsychomotorHealthDetails));
            CreateMap<AddHealthDetailDto.AddPsychomotorHealthDto, PsychomotorHealth>().ReverseMap();
            CreateMap<AddHealthDetailDto, PsychomotorHealth>();
            CreateMap<HealthDetailDto, HealthDetail>().ForMember(d => d.PsychomotorHealths, s => s.MapFrom(e => e.PsychomotorHealthDetails)).ReverseMap();
            CreateMap<HealthDetailDto.AddPsychomotorHealthDto, PsychomotorHealth>().ReverseMap();
            CreateMap<HealthDetailDto, PsychomotorHealth>().ReverseMap();
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
                //.ForMember(d => d.Address, s => s.MapFrom(e => e.Address))
                .ForMember(d => d.PhoneNumber, s => s.MapFrom(e => e.PhoneNumber));
            CreateMap<CarerSignInDto.CarerCertification, CertificationCarer>();
            CreateMap<CarerSignInDto, CertificationCarer>();
            CreateMap<Carer, Account>()
                .ForMember(d => d.Username, s => s.MapFrom(e => e.Name))
                .ForMember(d => d.PhoneNumber, s => s.MapFrom(e => e.Phone))
                .ForMember(d => d.CarerId, s => s.MapFrom(e => e.CarerId));
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
            //contract
            CreateMap<Contract, AddContractDto>().ReverseMap();

            //psychomotor
            CreateMap<AddPsychomotorDto, Psychomotor>().ReverseMap();
            CreateMap<UpdatePsychomotorDto, Psychomotor>().ReverseMap();

            //service
            CreateMap<Service, ServiceDto>().ReverseMap();
            CreateMap<AddServiceDto, Service>().ReverseMap();
            CreateMap<UpdateServiceDto, Service>().ReverseMap();

            //package
            CreateMap<AddPackageDto, Package>().ReverseMap();
            CreateMap<UpdatePackageDto, Package>().ReverseMap();
            CreateMap<PackageDto, Package>().ReverseMap();
            CreateMap<ServiceDto, Package>().ReverseMap();
            CreateMap<ServiceDto, PackageService>()
                .ReverseMap()
                .ForMember(d => d.ServiceId, s => s.MapFrom(e => e.ServiceId));

            //certificate
            CreateMap<AddCertificationTypeDto, Certification>().ReverseMap();
            CreateMap<UpdateCertificationTypeDto, Certification>().ReverseMap();

            //expo noti
            CreateMap<PushTicketRequestDto, PushTicketRequest>()
                .ForMember(d => d.PushTo, s => s.MapFrom(e => e.To))
                //.ForMember(d => d.PushData, s => s.MapFrom(e => e.Data))
                .ForMember(d => d.PushTitle, s => s.MapFrom(e => e.Title))
                .ForMember(d => d.PushBody, s => s.MapFrom(e => e.body))
                //.ForMember(d => d.PushTTL, s => s.MapFrom(e => e.Ttl))
                //.ForMember(d => d.PushExpiration, s => s.MapFrom(e => e.Expiration))
                .ForMember(d => d.PushPriority, s => s.MapFrom(e => e.Priority))
                .ForMember(d => d.PushSubTitle, s => s.MapFrom(e => e.SubTitle))
                .ForMember(d => d.PushSound, s => s.MapFrom(e => e.Sound))
                .ForMember(d => d.PushBadgeCount, s => s.MapFrom(e => e.Badge))
                .ForMember(d => d.PushChannelId, s => s.MapFrom(e => e.ChannelId))
                //.ForMember(d => d.PushCategoryId, s => s.MapFrom(e => e.CategoryId))
                .ForMember(d => d.PushMutableContent, s => s.MapFrom(e => e.MutableContent))
                .ReverseMap();
        }
    }
}
