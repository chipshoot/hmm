using DomainEntity.Misc;
using DomainEntity.User;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Validation;

namespace Hmm.Dal.Validation
{
    public class HmmNoteValidator : ValidatorBase<HmmNote>
    {
        public HmmNoteValidator(IEntityLookup lookupRepo) : base(lookupRepo)
        {
        }

        public override bool IsValid(HmmNote entity, bool isNewEntity)
        {
            if (entity == null)
            {
                ValidationErrors.Add("Error: null note found.");
                return false;
            }

            // make sure note author is exists
            if (!IsAuthorValid(entity.Author))
            {
                return false;
            }

            // make sure catalog is exists
            if (!IsCatValid(entity.Catalog))
            {
                return false;
            }

            if (!isNewEntity)
            {
                if (entity.Id <= 0)
                {
                    ValidationErrors.Add($"The note get invalid id {entity.Id}");
                    return false;
                }

                var rec = LookupRepo.GetEntity<HmmNote>(entity.Id);
                if (rec == null)
                {
                    ValidationErrors.Add($"Cannot find note with id {entity.Id} from data source");
                    return false;
                }
            }

            return true;
        }

        private bool IsAuthorValid(User author)
        {
            // make sure note author is exists
            if (author == null || author.Id <= 0)
            {
                ValidationErrors.Add("Error: invalid author attached to note.");
                return false;
            }

            var savedAuthor = LookupRepo.GetEntity<User>(author.Id);
            if (savedAuthor == null)
            {
                ValidationErrors.Add("Error: cannot find author from data source.");
                return false;
            }

            return true;
        }

        private bool IsCatValid(NoteCatalog catalog)
        {
            // make sure note catalog is exists
            if (catalog == null || catalog.Id <= 0)
            {
                ValidationErrors.Add("Error: invalid catalog attached to note.");
                return false;
            }

            var savedCat = LookupRepo.GetEntity<NoteCatalog>(catalog.Id);
            if (savedCat == null)
            {
                ValidationErrors.Add("Error: cannot find catalog from data source.");
                return false;
            }

            return true;
        }
    }
}