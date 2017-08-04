using DomainEntity.Misc;
using Hmm.Utility.Dal.DataStore;

namespace Hmm.Dal
{
    public class NoteStorage: IDataStore<HmmNote>
    {
        public HmmNote Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public HmmNote Add(HmmNote entity)
        {
            throw new System.NotImplementedException();
        }

        public HmmNote Update(HmmNote entity)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(HmmNote entity)
        {
            throw new System.NotImplementedException();
        }

        public void Refresh(ref HmmNote entity)
        {
            throw new System.NotImplementedException();
        }

        public void Flush()
        {
            throw new System.NotImplementedException();
        }
    }
}