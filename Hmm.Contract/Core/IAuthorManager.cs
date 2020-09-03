using Hmm.DomainEntity.User;
using Hmm.Utility.Misc;
using System;
using System.Collections.Generic;

namespace Hmm.Contract.Core
{
    public interface IAuthorManager
    {
        IEnumerable<Author> GetEntities();

        bool AuthorExists(string id);

        Author Create(Author authorInfo);

        Author Update(Author authorInfo);

        /// <summary>
        /// Set the flag to de-activate author to make it invisible for system.
        /// author may associated with note so we not want to delete everything
        /// </summary>
        /// <param name="id">The id of author whose activate flag will be set</param>
        void DeActivate(Guid id);

        ProcessingResult ProcessResult { get; }
    }
}