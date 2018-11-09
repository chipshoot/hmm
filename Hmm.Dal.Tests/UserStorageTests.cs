using DomainEntity.Misc;
using DomainEntity.User;
using System;
using Hmm.Utility.TestHelp;
using Xunit;

namespace Hmm.Dal.Tests
{
    public class UserStorageTests : TestFixtureBase
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
            var savedRec = UserStorage.Add(user);

            // Assert
            Assert.NotNull(savedRec);
            Assert.True(savedRec.Id > 0, "savedRec.Id > 0");
            Assert.Equal(user.Id, savedRec.Id);
        }

        [Fact]
        public void CanNotAddAlreadyExistedAccountNameToDataSource()
        {
            // Arrange
            var userExists = new User
            {
                Id = 1,
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
            UserStorage.Add(userExists);
            var savedUser = UserStorage.Add(user);

            // Assert
            Assert.Null(savedUser);
            Assert.True(user.Id < 0, "user.Id < 0");
            Assert.False(UserStorage.ProcessMessage.Success);
            Assert.Single(UserStorage.ProcessMessage.MessageList);
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

            var savedUser = UserStorage.Add(user);

            // Act
            var result = UserStorage.Delete(savedUser);

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

            UserStorage.Add(user);

            var user2 = new User
            {
                Id = 2,
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
            var result = UserStorage.Delete(user2);

            // Assert
            Assert.False(result);
            Assert.False(UserStorage.ProcessMessage.Success);
            Assert.Single(UserStorage.ProcessMessage.MessageList);
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
            var savedRender = RenderStorage.Add(render);

            var catalog = new NoteCatalog
            {
                Name = "DefaultCatalog",
                Render = savedRender,
                Schema = "testScheme",
                IsDefault = false,
                Description = "Description"
            };
            var savedCatalog = CatalogStorage.Add(catalog);

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
            var savedUser = UserStorage.Add(user);

            var note = new HmmNote
            {
                Subject = string.Empty,
                Content = string.Empty,
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Author = savedUser,
                Catalog = savedCatalog
            };
            NoteStorage.Add(note);

            // Act
            var result = UserStorage.Delete(user);

            // Assert
            Assert.False(result, "Error: deleted user with note");
            Assert.False(UserStorage.ProcessMessage.Success);
            Assert.Single(UserStorage.ProcessMessage.MessageList);
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

            UserStorage.Add(user);
            user.FirstName = "GasLog2";

            // Act
            var result = UserStorage.Update(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("GasLog2", result.FirstName);

            // Arrange - update last name
            user.LastName = "new Last name";

            // Act
            result = UserStorage.Update(user);

            // Arrange
            Assert.NotNull(result);
            Assert.Equal("new Last name", result.LastName);

            // Arrange - update birth day
            var newDay = new DateTime(2000, 5, 1);
            user.BirthDay = newDay;

            // Act
            result = UserStorage.Update(user);

            // Arrange
            Assert.NotNull(result);
            Assert.Equal(newDay, result.BirthDay);

            // Arrange - activate status
            user.IsActivated = false;

            // Act
            result = UserStorage.Update(user);

            // Arrange
            Assert.NotNull(result);
            Assert.False(result.IsActivated);

            // Arrange - update description
            user.Description = "new testing user";

            // Act
            result = UserStorage.Update(user);

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

            UserStorage.Add(user);

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
            var result = UserStorage.Update(user2);

            // Assert
            Assert.Null(result);
            Assert.False(UserStorage.ProcessMessage.Success);
            Assert.Single(UserStorage.ProcessMessage.MessageList);
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
            UserStorage.Add(user);

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
            UserStorage.Add(user2);

            user.AccountName = user2.AccountName;

            // Act
            var result = UserStorage.Update(user);

            // Assert
            Assert.Null(result);
            Assert.False(UserStorage.ProcessMessage.Success);
            Assert.Single(UserStorage.ProcessMessage.MessageList);
        }
    }
}