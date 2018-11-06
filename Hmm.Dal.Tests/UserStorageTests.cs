using DomainEntity.Misc;
using DomainEntity.User;
using Hmm.Dal.Data;
using Hmm.Dal.Querys;
using Hmm.Dal.Storage;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Hmm.Dal.Tests
{
    public class UserStorageTests : IDisposable
    {
        private List<User> _users;
        private UserStorage _userStorage;
        private NoteStorage _noteStorage;
        private NoteRenderStorage _renderStorage;
        private NoteCatalogStorage _catalogStorage;
        private IHmmDataContext _dbContext;
        private IEntityLookup _lookupRepo;
        private readonly bool _isUsingMock;

        public UserStorageTests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var envSetting = config["TestEnvironment:UseMoc"];
            if (!bool.TryParse(envSetting, out _isUsingMock))
            {
                throw new InvalidOperationException($"Cannot get environment setting UseMoc {envSetting}");
            }

            if (_isUsingMock)
            {
                SetMockEnvironment();
            }
            else
            {
                var connectString = config["ConnectionStrings:DefaultConnection"];
                SetRealEnvironment(connectString);
            }
        }

        public void Dispose()
        {
            if (_isUsingMock)
            {
                _users.Clear();
            }
            else
            {
                if (_dbContext is DbContext context)
                {
                    context.Reset();
                }

                var notes = _lookupRepo.GetEntities<HmmNote>();
                foreach (var note in notes)
                {
                    _noteStorage.Delete(note);
                }

                var catalogs = _lookupRepo.GetEntities<NoteCatalog>();
                foreach (var catalog in catalogs)
                {
                    _catalogStorage.Delete(catalog);
                }

                var renders = _lookupRepo.GetEntities<NoteRender>();
                foreach (var render in renders)
                {
                    _renderStorage.Delete(render);
                }

                var users = _lookupRepo.GetEntities<User>();
                foreach (var user in users)
                {
                    _userStorage.Delete(user);
                }
            }
        }

        private void SetMockEnvironment()
        {
            _users = new List<User>();

            var lookupMoc = new Mock<IEntityLookup>();
            lookupMoc.Setup(lk => lk.GetEntity<User>(It.IsAny<int>())).Returns((int id) =>
            {
                var recFound = _users.FirstOrDefault(c => c.Id == id);
                return recFound;
            });

            // set up unit of work
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(u => u.Add(It.IsAny<User>())).Returns((User user) =>
                {
                    user.Id = _users.GetNextId();
                    _users.AddEntity(user);
                    return user;
                }
            );
            uowMock.Setup(u => u.Delete(It.IsAny<User>())).Callback((User user) =>
            {
                _users.Remove(user);
            });
            uowMock.Setup(u => u.Update(It.IsAny<User>())).Callback((User user) =>
            {
                var ordUser = _users.FirstOrDefault(c => c.Id == user.Id);
                if (ordUser != null)
                {
                    _users.Remove(ordUser);
                    _users.AddEntity(user);
                }
            });

            // setup date time provider
            var timeProviderMock = new Mock<IDateTimeProvider>();

            // setup user storage
            _userStorage = new UserStorage(uowMock.Object, lookupMoc.Object, timeProviderMock.Object);
        }

        private void SetRealEnvironment(string connectString)
        {
            var optBuilder = new DbContextOptionsBuilder()
                .UseSqlServer(connectString);
            _dbContext = new HmmDataContext(optBuilder.Options);
            var uow = new EfUnitOfWork(_dbContext);
            _lookupRepo = new EfEntityLookup(_dbContext);
            var dateProvider = new DateTimeAdapter();
            _userStorage = new UserStorage(uow, _lookupRepo, dateProvider);
            _noteStorage = new NoteStorage(uow, _lookupRepo, dateProvider);
            _renderStorage = new NoteRenderStorage(uow, _lookupRepo, dateProvider);
            _catalogStorage = new NoteCatalogStorage(uow, _lookupRepo, dateProvider);
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
            _userStorage.Add(userExists);
            var savedUser = _userStorage.Add(user);

            // Assert
            Assert.Null(savedUser);
            Assert.True(user.Id < 0, "user.Id < 0");
            Assert.False(_userStorage.ProcessMessage.Success);
            Assert.Single(_userStorage.ProcessMessage.MessageList);
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

            var savedUser = _userStorage.Add(user);

            // Act
            var result = _userStorage.Delete(savedUser);

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

            _userStorage.Add(user);

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
            Assert.False(_userStorage.ProcessMessage.Success);
            Assert.Single(_userStorage.ProcessMessage.MessageList);
        }

        [Fact]
        public void CannotDeleteUserWithNoteAssociated()
        {
            // Arrange
            var render = new NoteRender
            {
                Name = "DefaultRender",
                Namespace = "NameSpace",
                Description = "Description"
            };
            var savedRender = _renderStorage.Add(render);

            var catalog = new NoteCatalog
            {
                Name = "DefaultCatalog",
                Render = savedRender,
                Schema = "testScheme",
                Description = "Description"
            };
            var savedCatalog = _catalogStorage.Add(catalog);

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
            var savedUser = _userStorage.Add(user);

            var note = new HmmNote
            {
                Subject = string.Empty,
                Content = string.Empty,
                CreateDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                Author = savedUser,
                Catalog = savedCatalog
            };
            _noteStorage.Add(note);

            // Act
            var result = _userStorage.Delete(user);

            // Assert
            Assert.False(result, "Error: deleted user with note");
            Assert.False(_userStorage.ProcessMessage.Success);
            Assert.Single(_userStorage.ProcessMessage.MessageList);
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

            _userStorage.Add(user);
            user.FirstName = "GasLog2";

            // Act
            var result = _userStorage.Update(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("GasLog2", result.FirstName);

            // Arrange - update last name
            user.LastName = "new Last name";

            // Act
            result = _userStorage.Update(user);

            // Arrange
            Assert.NotNull(result);
            Assert.Equal("new Last name", result.LastName);

            // Arrange - update birth day
            var newDay = new DateTime(2000, 5, 1);
            user.BirthDay = newDay;

            // Act
            result = _userStorage.Update(user);

            // Arrange
            Assert.NotNull(result);
            Assert.Equal(newDay, result.BirthDay);

            // Arrange - activate status
            user.IsActivated = false;

            // Act
            result = _userStorage.Update(user);

            // Arrange
            Assert.NotNull(result);
            Assert.False(result.IsActivated);

            // Arrange - update description
            user.Description = "new testing user";

            // Act
            result = _userStorage.Update(user);

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

            _userStorage.Add(user);

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
            Assert.False(_userStorage.ProcessMessage.Success);
            Assert.Single(_userStorage.ProcessMessage.MessageList);
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
            _userStorage.Add(user);

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
            _userStorage.Add(user2);

            user.AccountName = user2.AccountName;

            // Act
            var result = _userStorage.Update(user);

            // Assert
            Assert.Null(result);
            Assert.False(_userStorage.ProcessMessage.Success);
            Assert.Single(_userStorage.ProcessMessage.MessageList);
        }
    }
}