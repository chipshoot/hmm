using DomainEntity.Misc;
using Xunit;

namespace Hmm.Core.Tests
{
    public class NoteRepositoryTests
    {
        [Fact]
        public void CanCreateNote()
        {
            // Arrange
            var note = new HmmNote();

            // Act
            // Assert
            Assert.NotNull(note);
        }
    }
}