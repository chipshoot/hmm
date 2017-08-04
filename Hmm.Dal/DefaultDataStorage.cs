using Hmm.Utility.Dal.DataEntity;
using Hmm.Utility.Dal.DataStore;
using System;

namespace Hmm.Dal
{
    public class DefaultDataStorage<T> : IDataStore<T> where T : Entity
    {
        public T Get(int id)
        {
            throw new NotImplementedException();
        }

        public T Add(T entity)
        {
            throw new NotImplementedException();
        }

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void Refresh(ref T entity)
        {
            throw new NotImplementedException();
        }

        public void Flush()
        {
            throw new NotImplementedException();
        }

        public T FindEntityById(int id)
        {
            throw new NotImplementedException();
        }
    }
}