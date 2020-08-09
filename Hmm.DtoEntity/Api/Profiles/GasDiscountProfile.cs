using AutoMapper;
using Hmm.DomainEntity.Vehicle;
using Hmm.DtoEntity.Api.GasLogNotes;
using Hmm.Utility.Currency;

namespace Hmm.DtoEntity.Api.Profiles
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