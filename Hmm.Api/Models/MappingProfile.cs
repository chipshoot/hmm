using AutoMapper;
using DomainEntity.Misc;
using DomainEntity.User;
using Hmm.Api.Areas.HmmNote.Models;

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
        }
    }
}