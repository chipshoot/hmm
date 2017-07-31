using DomainEntity.Misc;
using Hmm.Dal;
using Xunit;

namespace Hmm.DalTests
{
    public class NoteRepositoryTests
    {
        [Fact]
        public void CanAddNoteToRepository()
        {
            // Arrange
            var repo = new DefaultRepository<HmmNote>();
            var note = new HmmNote
            {
                Id = 1,
                Author = 1,

            };
            // Act

            // Assert

        }
    }
}