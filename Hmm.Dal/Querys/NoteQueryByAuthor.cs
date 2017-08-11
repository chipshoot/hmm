using DomainEntity.Misc;
using DomainEntity.User;
using Hmm.Utility.Dal.Query;
using System.Collections.Generic;

namespace Hmm.Dal.Querys
{
    public class NoteQueryByAuthor : IQuery<IEnumerable<HmmNote>>
    {
        public User Author { get; set; }
    }
}