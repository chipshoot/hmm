using AutoMapper;
using DomainEntity.User;
using DomainEntity.Vehicle;
using Hmm.Api.Areas.HmmNote.Models;
using Hmm.Utility.Currency;
using Hmm.Utility.MeasureUnit;

namespace Hmm.Api.Areas.GasLogNote.Models
{
    public static class ApiDomainEntityConvertHelper
    {
        public static MapperConfiguration Api2DomainEntity()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApiUser, User>();
                cfg.CreateMap<ApiGasLog, GasLog>()
                    .ForMember(dest => dest.Subject, opt => opt.Ignore())
                    .ForMember(dest => dest.Content, opt => opt.Ignore())
                    .ForMember(dest => dest.Distance, opt => opt.MapFrom(src => Dimension.FromKilometre(src.Distance)))
                    .ForMember(dest => dest.Gas, opt => opt.MapFrom(src => Volume.FromLiter(src.Distance)))
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => new Money(src.Price)))
                    .ForMember(dest => dest.LastModifiedDate, opt => opt.Ignore());
                cfg.CreateMap<ApiGasLogForCreation, GasLog>()
                    .ForMember(dest => dest.Author, opt => opt.Ignore())
                    .ForMember(dest => dest.Subject, opt => opt.Ignore())
                    .ForMember(dest => dest.Content, opt => opt.Ignore())
                    .ForMember(dest => dest.Distance, opt => opt.MapFrom(src => Dimension.FromKilometre(src.Distance)))
                    .ForMember(dest => dest.Gas, opt => opt.MapFrom(src => Volume.FromLiter(src.Distance)))
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => new Money(src.Price)))
                    .ForMember(dest => dest.LastModifiedDate, opt => opt.MapFrom(src => src.CreateDate));

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
                cfg.CreateMap<GasLog, ApiGasLog>()
                    .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Author.Id))
                    .ForMember(dest => dest.Distance, opt => opt.MapFrom(src => src.Distance.TotalKilometre))
                    .ForMember(dest => dest.Gas, opt => opt.MapFrom(src => src.Gas.TotalLiter))
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount));
                cfg.CreateMap<GasDiscountInfo, ApiDiscountInfo>()
                    .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));
            });

            return config;
        }
    }
}