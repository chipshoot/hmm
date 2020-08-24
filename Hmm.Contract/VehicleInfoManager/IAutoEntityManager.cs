using Hmm.DomainEntity.User;
using Hmm.DomainEntity.Vehicle;
using Hmm.Utility.Misc;
using System;
using System.Collections.Generic;

namespace Hmm.Contract.VehicleInfoManager
{
    public interface IAutoEntityManager<T> where T : VehicleBase
    {
        T GetEntityById(int id);

        IEnumerable<T> GetEntities(User author);

        T Create(T entity, User author);

        T Update(T entity, User author);

        bool IsEntityOwner(int id, Guid authorId);

        ProcessingResult ProcessResult { get; }
    }
}