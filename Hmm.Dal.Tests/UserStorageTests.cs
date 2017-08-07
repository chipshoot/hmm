using DomainEntity.User;
using Hmm.Dal.Querys;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Hmm.Dal.Validation;
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
                    user.Id = _users.Count + 1;
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

        //[Fact]
        //public void CanDeleteNoteRenderFromDataSource()
        //{
        //    // Arrange
        //    var render = new NoteRender
        //    {
        //        Id = 1,
        //        Name = "GasLog",
        //        Namespace = "Note.GasLog",
        //        Description = "testing note"
        //    };

        //    _renders.AddEntity(render);
        //    Assert.Equal(1, _renders.Count);

        //    // Act
        //    var result = _renderStorage.Delete(render);

        //    // Assert
        //    Assert.True(result);
        //    Assert.Equal(0, _renders.Count);
        //}

        //[Fact]
        //public void CannotDeleteNonExistsNoteRenderFromDataSource()
        //{
        //    // Arrange
        //    var render = new NoteRender
        //    {
        //        Id = 1,
        //        Name = "GasLog",
        //        Namespace = "Note.GasLog",
        //        Description = "testing note"
        //    };

        //    _renders.AddEntity(render);

        //    var render2 = new NoteRender
        //    {
        //        Id = 2,
        //        Name = "GasLog2",
        //        Namespace = "Note.GasLog2",
        //        Description = "testing note"
        //    };

        //    // Act
        //    var result = _renderStorage.Delete(render2);

        //    // Assert
        //    Assert.False(result);
        //    Assert.Equal(1, _renders.Count);
        //}

        //[Fact]
        //public void CannotDeleteNoteRenderWithNoteAssociated()
        //{
        //    throw new NotImplementedException();
        //}

        //[Fact]
        //public void CanUpdateNoteRender()
        //{
        //    // Arrange - update name
        //    var render = new NoteRender
        //    {
        //        Id = 1,
        //        Name = "GasLog",
        //        Namespace = "Note.GasLog",
        //        Description = "testing note"
        //    };

        //    _renders.AddEntity(render);

        //    render.Name = "GasLog2";

        //    // Act
        //    var result = _renderStorage.Update(render);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal("GasLog2", _renders[0].Name);

        //    // Arrange - update description
        //    render.Description = "new testing note";

        //    // Act
        //    result = _renderStorage.Update(render);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal("new testing note", _renders[0].Description);
        //}

        //[Fact]
        //public void CannotUpdateNoteRenderForNonExistsCatalog()
        //{
        //    // Arrange
        //    var render = new NoteRender
        //    {
        //        Id = 1,
        //        Name = "GasLog",
        //        Namespace = "Note.GasLog",
        //        Description = "testing note"
        //    };

        //    _renders.AddEntity(render);

        //    var render2 = new NoteRender
        //    {
        //        Id = 2,
        //        Name = "GasLog2",
        //        Namespace = "Note.GasLog",
        //        Description = "testing note"
        //    };

        //    // Act
        //    var result = _renderStorage.Update(render2);

        //    // Assert
        //    Assert.Null(result);
        //    Assert.Equal(1, _renders.Count);
        //    Assert.Equal("GasLog", _renders[0].Name);
        //}

        //[Fact]
        //public void CannotUpdateNoteRenderWithDuplicatedName()
        //{
        //    // Arrange
        //    var render = new NoteRender
        //    {
        //        Id = 1,
        //        Name = "GasLog",
        //        Description = "testing note"
        //    };
        //    _renders.AddEntity(render);

        //    var render2 = new NoteRender
        //    {
        //        Id = 2,
        //        Name = "GasLog2",
        //        Description = "testing note2"
        //    };
        //    _renders.AddEntity(render2);

        //    render.Name = render2.Name;

        //    // Act
        //    var result = _renderStorage.Update(render);

        //    // Assert
        //    Assert.Null(result);
        //    Assert.Equal(2, _renders.Count);
        //    Assert.Equal("GasLog", _renders[0].Name);
        //    Assert.Equal("GasLog2", _renders[1].Name);
        //}
    }
}