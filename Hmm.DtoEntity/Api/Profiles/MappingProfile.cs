using AutoMapper;
using Hmm.DomainEntity.Misc;
using Hmm.DomainEntity.User;
using Hmm.DomainEntity.Vehicle;
using Hmm.DtoEntity.Api.GasLogNotes;
using Hmm.DtoEntity.Api.HmmNote;
using Hmm.Utility.Currency;
using Hmm.Utility.MeasureUnit;

namespace Hmm.DtoEntity.Api.Profiles
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
            CreateMap<GasDiscount, ApiDiscount>()
                .ForMember(d => d.Amount, opt => opt.MapFrom(s => s.Amount.Amount));
            CreateMap<GasDiscountInfo, ApiDiscountInfo>()
                .ForMember(d => d.DiscountId, opt => opt.MapFrom(s => s.Program.Id))
                .ForMember(d => d.Amount, opt => opt.MapFrom(s => s.Amount.Amount));
            CreateMap<ApiAutomobile, Automobile>();
            CreateMap<ApiAutomobileForCreate, Automobile>();
            CreateMap<Automobile, ApiAutomobile>();
            CreateMap<ApiGasLogForCreation, GasLog>()
                .ForMember(d => d.Distance, opt => opt.MapFrom(s => Dimension.FromKilometre(s.Distance)))
                .ForMember(d => d.Gas, opt => opt.MapFrom(s => Volume.FromLiter(s.Gas)))
                .ForMember(d => d.Price, opt => opt.MapFrom(s => new Money(s.Price)))
                .ForMember(d => d.Discounts, opt => opt.Ignore())
                .ForMember(d => d.Car, opt => opt.Ignore());
            CreateMap<GasLog, ApiGasLog>()
                .ForMember(d => d.Distance, opt => opt.MapFrom(s => s.Distance.TotalKilometre))
                .ForMember(d => d.Gas, opt => opt.MapFrom(s => s.Gas.TotalLiter))
                .ForMember(d => d.Price, opt => opt.MapFrom(s => s.Price.Amount))
                .ForMember(d => d.DiscountInfos, opt => opt.MapFrom(s => s.Discounts));
        }
    }
}