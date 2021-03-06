﻿using Hmm.Contract.Core;
using Hmm.Core.Manager.Validation;
using Hmm.DomainEntity.Misc;
using Hmm.Utility.Dal.Repository;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using System;
using System.Collections.Generic;

namespace Hmm.Core.Manager
{
    public class NoteRenderManager : INoteRenderManager
    {
        private readonly IRepository<NoteRender> _dataSource;
        private readonly NoteRenderValidator _validator;

        public NoteRenderManager(IRepository<NoteRender> dataSource, NoteRenderValidator validator)
        {
            Guard.Against<ArgumentNullException>(dataSource == null, nameof(dataSource));
            Guard.Against<ArgumentNullException>(validator == null, nameof(validator));

            _dataSource = dataSource;
            _validator = validator;
        }

        public NoteRender Create(NoteRender render)
        {
            if (!_validator.IsValidEntity(render, ProcessResult))
            {
                return null;
            }

            try
            {
                var addedRender = _dataSource.Add(render);
                return addedRender;
            }
            catch (Exception ex)
            {
                ProcessResult.WrapException(ex);
                return null;
            }
        }

        public NoteRender Update(NoteRender render)
        {
            if (!_validator.IsValidEntity(render, ProcessResult))
            {
                return null;
            }

            var updatedRender = _dataSource.Update(render);
            if (updatedRender == null)
            {
                ProcessResult.PropagandaResult(_dataSource.ProcessMessage);
            }

            return updatedRender;
        }

        public IEnumerable<NoteRender> GetEntities()
        {
            try
            {
                var renders = _dataSource.GetEntities();
                return renders;
            }
            catch (Exception ex)
            {
                ProcessResult.WrapException(ex);
                return null;
            }
        }

        public ProcessingResult ProcessResult { get; } = new ProcessingResult();
    }
}