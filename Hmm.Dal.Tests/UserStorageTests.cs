using DomainEntity.User;
using Hmm.Dal.Querys;
using Hmm.Dal.Validation;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Hmm.Dal.Tests
{
    public class UserStorageTests : IDisposable
    {
        private readonly List<User> _users;
        private readonly UserStorage _userStorage;

        public UserStorageTests()
        {
            _users = new List<User>();

            var lookupMoc = new Mock<IEntityLookup>();
            lookupMoc.Setup(lk => lk.GetEntity<User>(It.IsAny<int>())).Returns((int id) =>
            {
                var recFound = _users.FirstOrDefault(c => c.Id == id);
                return recFound;
            });

            var uowmock = new Mock<IUnitOfWork>();
            uowmock.Setup(u => u.Add(It.IsAny<User>())).Returns((User user) =>
                {
                    user.Id = _users.GetNextId();
                    _users.AddEntity(user);
                    return user;
                }
            );
            uowmock.Setup(u => u.Delete(It.IsAny<User>())).Callback((User user) =>
            {
                _users.Remove(user);
            });
            uowmock.Setup(u => u.Update(It.IsAny<User>())).Callback((User user) =>
            {
                var orduser = _users.FirstOrDefault(c => c.Id == user.Id);
                if (orduser != null)
                {
                    _users.Remove(orduser);
                    _users.AddEntity(user);
                }
            });

            var queryMock = new Mock<IQueryHandler<UserQueryByAccount, User>>();
            queryMock.Setup(q => q.Execute(It.IsAny<UserQueryByAccount>())).Returns((UserQueryByAccount q) =>
            {
                var userfound = _users.FirstOrDefault(c => c.AccountName == q.AccountName);
                return userfound;
            });

            var valiator = new UserValidator(lookupMoc.Object, queryMock.Object);
            _userStorage = new UserStorage(uowmock.Object, valiator, lookupMoc.Object);
        }

        public void Dispose()
        {
            _users.Clear();
        }

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
            var savedRec = _userStorage.Add(user);

            // Assert
            Assert.NotNull(savedRec);
            Assert.Equal(1, savedRec.Id);
            Assert.Equal(1, user.Id);
            Assert.Equal(1, _users.Count);
        }

        [Fact]
        public void CanNotAddAlreadyExistedAccountNameToDataSource()
        {
            // Arrange
            _users.AddEntity(new User
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
            });

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
            var savedRec = _userStorage.Add(user);

            // Assert
            Assert.Null(savedRec);
            Assert.Equal(0, user.Id);
            Assert.Equal(1, _users.Count);
        }

        [Fact]
        public void CanDeleteUserFromDataSource()
        {
            // Arrange
            var user = new User
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

            _users.AddEntity(user);
            Assert.Equal(1, _users.Count);

            // Act
            var result = _userStorage.Delete(user);

            // Assert
            Assert.True(result);
            Assert.Equal(0, _users.Count);
        }

        [Fact]
        public void CannotDeleteNonExistsUserFromDataSource()
        {
            // Arrange
            var user = new User
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

            _users.AddEntity(user);

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
            var result = _userStorage.Delete(user2);

            // Assert
            Assert.False(result);
            Assert.Equal(1, _users.Count);
        }

        [Fact]
        public void CannotDeleteUserWithNoteAssociated()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CanUpdateUser()
        {
            // Arrange - update first name
            var user = new User
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

            _users.AddEntity(user);

            user.FirstName = "GasLog2";

            // Act
            var result = _userStorage.Update(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("GasLog2", _users[0].FirstName);

            // Arrange - update last name
            user.LastName = "new Last name";

            // Act
            result = _userStorage.Update(user);

            // Arrange
            Assert.NotNull(result);
            Assert.Equal("new Last name", _users[0].LastName);

            // Arrange - update birth day
            var newDay = new DateTime(2000, 5, 1);
            user.BirthDay = newDay;

            // Act
            result = _userStorage.Update(user);

            // Arrange
            Assert.NotNull(result);
            Assert.Equal(newDay, _users[0].BirthDay);

            // Arrange - activate status
            user.IsActivated = false;

            // Act
            result = _userStorage.Update(user);

            // Arrange
            Assert.NotNull(result);
            Assert.False(_users[0].IsActivated);

            // Arrange - update description
            user.Description = "new testing user";

            // Act
            result = _userStorage.Update(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("new testing user", _users[0].Description);
        }

        [Fact]
        public void CannotUpdateForNonExistsUser()
        {
            // Arrange
            var user = new User
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

            _users.AddEntity(user);

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
            var result = _userStorage.Update(user2);

            // Assert
            Assert.Null(result);
            Assert.True(_userStorage.Validator.ValidationErrors.Count > 0);
            Assert.Equal(1, _users.Count);
            Assert.Equal("Gas", _users[0].FirstName);
        }

        [Fact]
        public void CannotUpdateUserWithDuplicatedAccountName()
        {
            // Arrange
            var user = new User
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
            _users.AddEntity(user);

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
            _users.AddEntity(user2);

            user.AccountName = user2.AccountName;

            // Act
            var result = _userStorage.Update(user);

            // Assert
            Assert.Null(result);
            Assert.True(_userStorage.Validator.ValidationErrors.Count > 0);
            Assert.Equal(2, _users.Count);
            Assert.Equal("glog", _users[0].AccountName);
            Assert.Equal("glog2", _users[1].AccountName);
        }
    }
}