using AutoMapper;
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
        }
    }
}