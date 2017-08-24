using System.Xml;
using DomainEntity.Misc;
using DomainEntity.Vehicle;
using Hmm.Contract.GasLogMan;
using Hmm.Utility.Dal.DataStore;

namespace Hmm.Core.Manager
{
    public class GasLogManager : HmmNoteManager, IGasLogManager
    {
        public GasLogManager(IDataStore<HmmNote> storage) : base(storage)
        {
        }

        public GasLog Create(GasLog note)
        {
            throw new System.NotImplementedException();
        }

        public GasLog Update(GasLog note)
        {
            throw new System.NotImplementedException();
        }

        public virtual XmlDocument GetNoteContent(GasLog note)
        {
            throw new System.NotImplementedException();
        }

    }
}