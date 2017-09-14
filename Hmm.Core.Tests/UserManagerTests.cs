using DomainEntity.Misc;
using DomainEntity.User;
using Hmm.Contract;
using Hmm.Core.Manager;
using Hmm.Dal.Storages;
using Hmm.Utility.Dal;
using Hmm.Utility.Dal.Query;
using Hmm.Utility.Misc;
using Hmm.Utility.Validation;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Hmm.Core.Tests
{
    public class UserManagerTests : IDisposable
    {
        #region private fields

        private readonly List<User> _users;
        private readonly IUserManager _usrmanager;

        #endregion private fields

        public UserManagerTests()
        {
            _users = new List<User>();
            var uowmoc = new Mock<IUnitOfWork>();
            uowmoc.Setup(d => d.Add(It.IsAny<User>())).Returns((User usr) =>
              {
                  usr.Id = _users.GetNextId();
                  _users.Add(usr);
                  return usr;
              });
            var validmoc = new Mock<IValidator<User>>();
            validmoc.Setup(v => v.IsValid(It.IsAny<User>(), It.IsAny<bool>())).Returns(true);
            var lookupmoc = new Mock<IEntityLookup>();
            var notequerymoc = new Mock<IQueryHandler<IQuery<IEnumerable<HmmNote>>, IEnumerable<HmmNote>>>();
            var timeAdp = new Mock<IDateTimeProvider>();
            var data = new UserStorage(uowmoc.Object, validmoc.Object, lookupmoc.Object, notequerymoc.Object, timeAdp.Object);

            _usrmanager = new UserManager(data);
        }

        public void Dispose()
        {
            _users.Clear();
        }

        [Fact]
        public void Can_Add_Valid_User()
        {
            // Arrange
            var user = new User
            {
                AccountName = "jfang",
                FirstName = "Jack",
                LastName = "Fang",
                BirthDay = new DateTime(2016, 2, 15),
                IsActivated = true,
                Password = "1235",
            };

            // Act
            var newUsr = _usrmanager.Create(user);

            // Assert
            Assert.NotNull(newUsr);
            Assert.True(newUsr.Id > 0);
            Assert.NotNull(newUsr.Salt);
        }
    }
}