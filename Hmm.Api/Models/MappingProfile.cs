using AutoMapper;
using DomainEntity.Misc;
using DomainEntity.User;
using DomainEntity.Vehicle;
using Hmm.Api.Areas.GasLogNote.Models;
using Hmm.Api.Areas.HmmNote.Models;
using Hmm.Utility.Currency;

namespace Hmm.Api.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApiUser, User>();
            CreateMap<User, ApiUser>();
            CreateMap<ApiUserForCreate, User>();
            CreateMap<ApiUserForUpdate, User>();
            CreateMap<User, ApiUserForUpdate>();

            CreateMap<ApiNoteRender, NoteRender>();
            CreateMap<NoteRender, ApiNoteRender>();
            CreateMap<ApiNoteRenderForCreate, NoteRender>();
            CreateMap<ApiNoteRenderForUpdate, NoteRender>();
            CreateMap<NoteRender, ApiNoteRenderForUpdate>();

            CreateMap<ApiNoteCatalog, NoteCatalog>();
            CreateMap<NoteCatalog, ApiNoteCatalog>();
            CreateMap<ApiNoteCatalogForCreate, NoteCatalog>();
            CreateMap<ApiNoteCatalogForUpdate, NoteCatalog>();
            CreateMap<NoteCatalog, ApiNoteCatalogForUpdate>();

            // setup gas discount mapping
            CreateMap<ApiDiscountForCreate, GasDiscount>()
                .ForMember(d => d.Amount, opt => opt.MapFrom(src => new Money(src.Amount, CurrencyCodeType.Cad)));
            CreateMap<ApiDiscountForUpdate, GasDiscount>()
                .ForMember(d => d.Amount, opt => opt.MapFrom(src => new Money(src.Amount, CurrencyCodeType.Cad)));
            CreateMap<GasDiscount, ApiDiscountInfo>()
                .ForMember(d => d.Amount, opt => opt.MapFrom(s => s.Amount.Amount));
            CreateMap<ApiAutomobile, Automobile>();
            CreateMap<ApiAutomobileForCreate, Automobile>();
            CreateMap<Automobile, ApiAutomobile>();
        }
    }
}