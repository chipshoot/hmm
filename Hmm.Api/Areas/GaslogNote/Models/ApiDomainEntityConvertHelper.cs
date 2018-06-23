using AutoMapper;
using DomainEntity.User;
using DomainEntity.Vehicle;
using Hmm.Api.Areas.HmmNote.Models;
using Hmm.Utility.Currency;

namespace Hmm.Api.Areas.GaslogNote.Models
{
    public static class ApiDomainEntityConvertHelper
    {
        public static MapperConfiguration Api2DomainEntity()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApiUser, User>();
                cfg.CreateMap<ApiGasLog, GasLog>()
                    .ForMember(dest=>dest.Subject, opt=>opt.Ignore())
                    .ForMember(dest=>dest.Content, opt=>opt.Ignore());
                cfg.CreateMap<ApiDiscountInfo, GasDiscountInfo>()
                    .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => new Money(src.Amount)));
            });

            return config;
        }

        public static MapperConfiguration DomainEntity2Api()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, ApiUser>();
                cfg.CreateMap<GasLog, ApiGasLog>();
                cfg.CreateMap<GasDiscountInfo, ApiDiscountInfo>()
                    .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));
            });

            return config;
        }
    }
}