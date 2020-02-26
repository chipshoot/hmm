using AutoMapper;
using DomainEntity.Vehicle;
using Hmm.Api.Areas.GasLogNote.Models;
using Hmm.Utility.Currency;

namespace Hmm.Api.Areas.GasLogNote.Profiles
{
    public class GasDiscountProfile : Profile
    {
        public GasDiscountProfile()
        {
            CreateMap<GasDiscount, ApiDiscount>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));

            CreateMap<ApiDiscountForCreate, GasDiscount>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => new Money(src.Amount, CurrencyCodeType.Cad)));
        }
    }
}