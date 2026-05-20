using LocalMarket.Core.DTos.Business;
using LocalMarket.Core.Entities;
using LocalMarket.Infrastructure.Persistence.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LocalMarket.Infrastructure.Mappers
{
    public class BusinessProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // BusinessModel → Business
            config.NewConfig<BusinessModel, Business>();

            // Business → BusinessModel
            config.NewConfig<Business, BusinessModel>();

            // Business → BusinessDto
            config.NewConfig<Business, BusinessDto>();

            // CreateBusinessDto → Business
            config.NewConfig<CreateBusinessDto, Business>()
                .Map(dest => dest.IsActive, _ => true)
                .Map(dest => dest.IsVerified, _ => false)
                .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.UserId)
                .Ignore(dest => dest.LogoUrl);

            // UpdateBusinessDto → Business
            config.NewConfig<UpdateBusinessDto, Business>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.UserId)
                .Ignore(dest => dest.LogoUrl)
                .Ignore(dest => dest.IsVerified)
                .Ignore(dest => dest.CreatedAt);
        }
    }
}