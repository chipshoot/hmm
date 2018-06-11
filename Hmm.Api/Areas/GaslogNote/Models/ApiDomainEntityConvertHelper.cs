using AutoMapper;
using DomainEntity.User;
using DomainEntity.Vehicle;
using Hmm.Api.Areas.HmmNote.Models;

namespace Hmm.Api.Areas.GaslogNote.Models
{
    public static class ApiDomainEntityConvertHelper
    {
        public static MapperConfiguration Api2DomainEntity()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApiUser, User>();
                cfg.CreateMap<ApiGasLog, GasLog>();
            });

            return config;
        }

        public static MapperConfiguration DomainEntity2Api()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, ApiUser>();
                cfg.CreateMap<GasLog, ApiGasLog>();
            });

            return config;
        }
    }
}