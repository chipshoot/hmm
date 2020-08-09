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
            var user = new User
            {
                FirstName = "Gas",
                LastName = "Log",
                AccountName = "glog",
                BirthDay = new DateTime(2001, 10, 2),
                Password = "Password1!",
                Salt = "passwordSalt",
                Description = "testing user",
                IsActivated = true
            };

            // Act
            var savedRec = UserRepository.Add(user);

            // Assert
            Assert.NotNull(savedRec);
            Assert.True(savedRec.Id != Guid.Empty, "savedRec.Id is not empty Guid 0");
            Assert.Equal(user.Id, savedRec.Id);
        }

        [Fact]
        public void CanNotAddAlreadyExistedAccountNameToDataSource()
        {
            // Arrange
            var userExists = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Gas",
                LastName = "Log",
                AccountName = "glog",
                BirthDay = new DateTime(2001, 10, 2),
                Password = "Password1!",
                Salt = "passwordSalt",
                Description = "testing user",
                IsActivated = true
            };

            var user = new User
            {
                FirstName = "Gas2",
                LastName = "Log2",
                AccountName = "glog",
                BirthDay = new DateTime(2001, 10, 2),
                Password = "Password1!",
                Salt = "passwordSalt",
                Description = "testing user",
                IsActivated = true
            };

            // Act
            UserRepository.Add(userExists);
            var savedUser = UserRepository.Add(user);

            // Assert
            Assert.Null(savedUser);
            Assert.False(UserRepository.ProcessMessage.Success);
            Assert.Single(UserRepository.ProcessMessage.MessageList);
        }

        [Fact]
        public void CanDeleteUserFromDataSource()
        {
            // Arrange
            var user = new User
            {
                FirstName = "Gas",
                LastName = "Log",
                AccountName = "glog",
                BirthDay = new DateTime(2001, 10, 2),
                Password = "Password1!",
                Salt = "passwordSalt",
                Description = "testing user",
                IsActivated = true
            };

            var savedUser = UserRepository.Add(user);

            // Act
            var result = UserRepository.Delete(savedUser);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CannotDeleteNonExistsUserFromDataSource()
        {
            // Arrange
            var user = new User
            {
                FirstName = "Gas",
                LastName = "Log",
                AccountName = "glog",
                BirthDay = new DateTime(2001, 10, 2),
                Password = "Password1!",
                Salt = "passwordSalt",
                Description = "testing user",
                IsActivated = true
            };

            UserRepository.Add(user);

            var user2 = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Gas2",
                LastName = "Log2",
                AccountName = "glog2",
                BirthDay = new DateTime(2001, 10, 2),
                Password = "Password1!",
                Salt = "passwordSalt",
                Description = "testing user",
                IsActivated = true
            };

            // Act
            var result = UserRepository.Delete(user2);

            // Assert
            Assert.False(result);
            Assert.False(UserRepository.ProcessMessage.Success);
            Assert.Single(UserRepository.ProcessMessage.MessageList);
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

            var user = new User
            {
                FirstName = "Gas",
                LastName = "Log",
                AccountName = "glog",
                BirthDay = new DateTime(2001, 10, 2),
                Password = "Password1!",
                Salt = "passwordSalt",
                Description = "testing user",
                IsActivated = true
            };
            var savedUser = UserRepository.Add(user);

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
            var result = UserRepository.Delete(user);

            // Assert
            Assert.False(result, "Error: deleted user with note");
            Assert.False(UserRepository.ProcessMessage.Success);
            Assert.Single(UserRepository.ProcessMessage.MessageList);
        }

        [Fact]
        public void CanUpdateUser()
        {
            // Arrange - update first name
            var user = new User
            {
                FirstName = "Gas",
                LastName = "Log",
                AccountName = "glog",
                BirthDay = new DateTime(2001, 10, 2),
                Password = "Password1!",
                Salt = "passwordSalt",
                Description = "testing user",
                IsActivated = true
            };

            UserRepository.Add(user);
            user.FirstName = "GasLog2";

            // Act
            var result = UserRepository.Update(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("GasLog2", result.FirstName);

            // Arrange - update last name
            user.LastName = "new Last name";

            // Act
            result = UserRepository.Update(user);

            // Arrange
            Assert.NotNull(result);
            Assert.Equal("new Last name", result.LastName);

            // Arrange - update birth day
            var newDay = new DateTime(2000, 5, 1);
            user.BirthDay = newDay;

            // Act
            result = UserRepository.Update(user);

            // Arrange
            Assert.NotNull(result);
            Assert.Equal(newDay, result.BirthDay);

            // Arrange - activate status
            user.IsActivated = false;

            // Act
            result = UserRepository.Update(user);

            // Arrange
            Assert.NotNull(result);
            Assert.False(result.IsActivated);

            // Arrange - update description
            user.Description = "new testing user";

            // Act
            result = UserRepository.Update(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("new testing user", result.Description);
        }

        [Fact]
        public void CannotUpdateForNonExistsUser()
        {
            // Arrange
            var user = new User
            {
                FirstName = "Gas",
                LastName = "Log",
                AccountName = "glog",
                BirthDay = new DateTime(2001, 10, 2),
                Password = "Password1!",
                Salt = "passwordSalt",
                Description = "testing user",
                IsActivated = true
            };

            UserRepository.Add(user);

            var user2 = new User
            {
                FirstName = "Gas2",
                LastName = "Log2",
                AccountName = "glog2",
                BirthDay = new DateTime(2001, 10, 2),
                Password = "Password1!",
                Salt = "passwordSalt",
                Description = "testing user",
                IsActivated = true
            };

            // Act
            var result = UserRepository.Update(user2);

            // Assert
            Assert.Null(result);
            Assert.False(UserRepository.ProcessMessage.Success);
            Assert.Single(UserRepository.ProcessMessage.MessageList);
        }

        [Fact]
        public void CannotUpdateUserWithDuplicatedAccountName()
        {
            // Arrange
            var user = new User
            {
                FirstName = "Gas",
                LastName = "Log",
                AccountName = "glog",
                BirthDay = new DateTime(2001, 10, 2),
                Password = "Password1!",
                Salt = "passwordSalt",
                Description = "testing user",
                IsActivated = true
            };
            UserRepository.Add(user);

            var user2 = new User
            {
                FirstName = "Gas2",
                LastName = "Log2",
                AccountName = "glog2",
                BirthDay = new DateTime(2001, 10, 2),
                Password = "Password1!",
                Salt = "passwordSalt",
                Description = "testing user",
                IsActivated = true
            };
            UserRepository.Add(user2);

            user.AccountName = user2.AccountName;

            // Act
            var result = UserRepository.Update(user);

            // Assert
            Assert.Null(result);
            Assert.False(UserRepository.ProcessMessage.Success);
            Assert.Single(UserRepository.ProcessMessage.MessageList);
        }
    }
}