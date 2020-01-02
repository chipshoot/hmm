//using DomainEntity.Misc;
//using Hmm.Utility.Dal.Repository;
//using Hmm.Utility.Misc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using Hmm.Utility.Validation;
//using System.Xml.Linq;

//namespace Hmm.Dal.Storage
//{
//    public abstract class NoteRepositoryBase<T> : IRepository<T> where T : HmmNote
//    {
//        private readonly ValidatorBase<T> _validator;

//        protected NoteRepositoryBase(ValidatorBase<T> validator)
//        {
//            Guard.Against<ArgumentNullException>(validator==null, nameof(validator));
//            _validator = validator;
//        }

//        public ProcessingResult ProcessMessage => throw new NotImplementedException();

//        public T FindEntityById(int id)
//        {
//            var note = FindEntities().FirstOrDefault(n => n.Id == id);
//            return note;
//        }

//        public IQueryable<T> FindEntities()
//        {
//            throw new NotImplementedException();
//        }

//        public IEnumerable<T> FindEntities(Expression<Func<T, bool>> query)
//        {
//            throw new NotImplementedException();
//        }

//        public bool Add(T entity)
//        {
//            if (!_validator.IsValidEntity(entity, ProcessMessage))
//            {
//                return false;
//            }

//            var xmlContent = GetEntity(entity, true);
//            note.Content = xmlContent.ToString(SaveOptions.DisableFormatting);
//            var ret = _noteStorage.Add(note);
//            if (ret == null)
//            {
//                ProcessMessage.PropagandaResult(_noteStorage.ProcessMessage);
//            }

//            return ProcessMessage.Success;
//        }

//        public bool Update(T entity)
//        {
//            throw new NotImplementedException();
//        }

//        public void Delete(T entity)
//        {
//            throw new NotImplementedException();
//        }

//        protected abstract void SetEntity(T entity);

//        protected abstract T GetEntity(HmmNote note);
//    }
//}