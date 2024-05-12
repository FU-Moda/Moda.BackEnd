using AutoMapper;
using Moda.Backend.Domain.Models;
using Moda.BackEnd.Common.DTO.Request;
using Moda.BackEnd.Common.DTO.Response;
using Moda.BackEnd.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moda.BackEnd.Infrastructure.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMap()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Account, AccountResponse>()
              .ForMember(desc => desc.Id, act => act.MapFrom(src => src.Id))
              .ForMember(desc => desc.Email, act => act.MapFrom(src => src.Email))
              .ForMember(desc => desc.Gender, act => act.MapFrom(src => src.Gender))
              .ForMember(desc => desc.IsVerified, act => act.MapFrom(src => src.IsVerified))
              .ForMember(desc => desc.FirstName, act => act.MapFrom(src => src.FirstName))
              .ForMember(desc => desc.LastName, act => act.MapFrom(src => src.LastName))
              .ForMember(desc => desc.PhoneNumber, act => act.MapFrom(src => src.PhoneNumber))
              .ForMember(desc => desc.UserName, act => act.MapFrom(src => src.UserName))
              ;

                config.CreateMap<Product, ProductDto>()
                .ForMember(desc => desc.Name, act => act.MapFrom(src => src.Name))
                .ForMember(desc => desc.Description, act => act.MapFrom(src => src.Description))
                .ForMember(desc => desc.ClothType, act => act.MapFrom(src => src.ClothType))
                .ForMember(desc => desc.Gender, act => act.MapFrom(src => src.Gender))
                .ForMember(desc => desc.ShopId, act => act.MapFrom(src => src.ShopId))
                .ReverseMap();

                config.CreateMap<StaticFile, StaticFileDto>()
               .ForMember(desc => desc.Id, act => act.MapFrom(src => src.Id))
               .ForMember(desc => desc.ProductId, act => act.MapFrom(src => src.ProductId))
               .ForMember(desc => desc.RatingId, act => act.MapFrom(src => src.RatingId))
                ;


            });
            return mappingConfig;   
        }
    }
}
