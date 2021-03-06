﻿using System;
using System.Linq;
using System.Linq.Expressions;
using Hmm.Dal.Data;
using Hmm.DomainEntity.Misc;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Dal.Repository;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;

namespace Hmm.Dal.DataRepository
{
    public class SubsystemEfRepository :RepositoryBase, IRepository<Subsystem>
    {
        public SubsystemEfRepository(IHmmDataContext dataContext, IEntityLookup lookupRepo, IDateTimeProvider dateTimeProvider) : base(dataContext, lookupRepo, dateTimeProvider)
        {
        }

        public IQueryable<Subsystem> GetEntities(Expression<Func<Subsystem, bool>> query = null)
        {
            return LookupRepo.GetEntities(query);
        }

        public Subsystem Add(Subsystem entity)
        {

            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));

            try
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                DataContext.Subsystems.Add(entity);
                DataContext.Save();
                return entity;
            }
            catch (Exception ex)
            {
                ProcessMessage.WrapException(ex);
                return null;
            }
        }

        public Subsystem Update(Subsystem entity)
        {
            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));

            // ReSharper disable once PossibleNullReferenceException
            if (entity.Id <= 0)
            {
                ProcessMessage.Success = false;
                ProcessMessage.AddErrorMessage($"Can not update Subsystem with id {entity.Id}", true);
                return null;
            }

            try
            {
                // make sure the record exists in data source
                DataContext.Subsystems.Update(entity);
                DataContext.Save();
                return LookupRepo.GetEntity<Subsystem>(entity.Id);
            }
            catch (Exception ex)
            {
                ProcessMessage.WrapException(ex);
                return null;
            }
        }

        public bool Delete(Subsystem entity)
        {
            Guard.Against<ArgumentNullException>(entity == null, nameof(entity));

            try
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                DataContext.Subsystems.Remove(entity);
                DataContext.Save();
                return true;
            }
            catch (Exception ex)
            {
                ProcessMessage.WrapException(ex);
                return false;
            }
        }

        public void Flush()
        {
            DataContext.Save();
        }

        public ProcessingResult ProcessMessage { get; } = new ProcessingResult();
    }
}