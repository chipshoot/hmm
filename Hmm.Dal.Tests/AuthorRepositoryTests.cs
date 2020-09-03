using Hmm.DomainEntity.Misc;
using Hmm.DomainEntity.User;
using Hmm.Utility.TestHelp;
using System;
using Xunit;

namespace Hmm.Dal.Tests
{
    public class UserRepositoryTests : TestFixtureBase
    {
        [Fact]
        public void CanAddUserToDataSource()
        {
            // Arrange
            var user = new Author
            {
                AccountName = "glog",
                Description = "testing user",
                IsActivated = true
            };

            // Act
            var savedRec = AuthorRepository.Add(user);

            // Assert
            Assert.NotNull(savedRec);
            Assert.True(savedRec.Id != Guid.Empty, "savedRec.Id is not empty Guid 0");
            Assert.Equal(user.Id, savedRec.Id);
        }

        [Fact]
        public void CanNotAddAlreadyExistedAccountNameToDataSource()
        {
            // Arrange
            var userExists = new Author
            {
                Id = Guid.NewGuid(),
                AccountName = "glog",
                Description = "testing user",
                IsActivated = true
            };

            var user = new Author
            {
                AccountName = "glog",
                Description = "testing user",
                IsActivated = true
            };

            // Act
            AuthorRepository.Add(userExists);
            var savedUser = AuthorRepository.Add(user);

            // Assert
            Assert.Null(savedUser);
            Assert.False(AuthorRepository.ProcessMessage.Success);
            Assert.Single(AuthorRepository.ProcessMessage.MessageList);
        }

        [Fact]
        public void CanDeleteUserFromDataSource()
        {
            // Arrange
            var user = new Author
            {
                AccountName = "glog",
                Description = "testing user",
                IsActivated = true
            };

            var savedUser = AuthorRepository.Add(user);

            // Act
            var result = AuthorRepository.Delete(savedUser);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CannotDeleteNonExistsUserFromDataSource()
        {
            // Arrange
            var user = new Author
            {
                AccountName = "glog",
                Description = "testing user",
                IsActivated = true
            };

            AuthorRepository.Add(user);

            var user2 = new Author
            {
                Id = Guid.NewGuid(),
                AccountName = "glog2",
                Description = "testing user",
                IsActivated = true
            };

            // Act
            var result = AuthorRepository.Delete(user2);

            // Assert
            Assert.False(result);
            Assert.False(AuthorRepository.ProcessMessage.Success);
            Assert.Single(AuthorRepository.ProcessMessage.MessageList);
        }

        [Fact]
        public void CannotDeleteUserWithNoteAssociated()
        {
            // Arrange
            var render = new NoteRender
            {
                Name = "DefaultRender",
                Namespace = "NameSpace",
                IsDefault = true,
                Description = "Description"
            };
            var savedRender = RenderRepository.Add(render);

            var catalog = new NoteCatalog
            {
                Name = "DefaultCatalog",
                Render = savedRender,
                Schema = "testScheme",
                IsDefault = false,
                Description = "Description"
            };
            var savedCatalog = CatalogRepository.Add(catalog);

            var user = new Author
            {
                AccountName = "glog",
                Description = "testing user",
                IsActivated = true
            };
            var savedUser = AuthorRepository.Add(user);

            var note = new HmmNote
            {
                Subject = string.Empty,
                Content = string.Empty,
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Author = savedUser,
                Catalog = savedCatalog
            };
            NoteRepository.Add(note);

            // Act
            var result = AuthorRepository.Delete(user);

            // Assert
            Assert.False(result, "Error: deleted user with note");
            Assert.False(AuthorRepository.ProcessMessage.Success);
            Assert.Single(AuthorRepository.ProcessMessage.MessageList);
        }

        [Fact]
        public void CanUpdateUser()
        {
            // Arrange - update first name
            var user = new Author
            {
                AccountName = "glog",
                Description = "testing user",
                IsActivated = true
            };

            AuthorRepository.Add(user);

            // Arrange - activate status
            user.IsActivated = false;

            // Act
            var result = AuthorRepository.Update(user);

            // Arrange
            Assert.NotNull(result);
            Assert.False(result.IsActivated);

            // Arrange - update description
            user.Description = "new testing user";

            // Act
            result = AuthorRepository.Update(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("new testing user", result.Description);
        }

        [Fact]
        public void CannotUpdateForNonExistsUser()
        {
            // Arrange
            var user = new Author
            {
                AccountName = "glog",
                Description = "testing user",
                IsActivated = true
            };

            AuthorRepository.Add(user);

            var user2 = new Author
            {
                AccountName = "glog2",
                Description = "testing user",
                IsActivated = true
            };

            // Act
            var result = AuthorRepository.Update(user2);

            // Assert
            Assert.Null(result);
            Assert.False(AuthorRepository.ProcessMessage.Success);
            Assert.Single(AuthorRepository.ProcessMessage.MessageList);
        }

        [Fact]
        public void CannotUpdateUserWithDuplicatedAccountName()
        {
            // Arrange
            var user = new Author
            {
                AccountName = "glog",
                Description = "testing user",
                IsActivated = true
            };
            AuthorRepository.Add(user);

            var user2 = new Author
            {
                AccountName = "glog2",
                Description = "testing user",
                IsActivated = true
            };
            AuthorRepository.Add(user2);

            user.AccountName = user2.AccountName;

            // Act
            var result = AuthorRepository.Update(user);

            // Assert
            Assert.Null(result);
            Assert.False(AuthorRepository.ProcessMessage.Success);
            Assert.Single(AuthorRepository.ProcessMessage.MessageList);
        }
    }
}