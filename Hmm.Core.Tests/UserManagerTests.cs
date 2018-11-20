using DomainEntity.User;
using Hmm.Contract.Core;
using Hmm.Core.Manager;
using Hmm.Utility.TestHelp;
using System;
using System.Linq;
using FluentValidation.TestHelper;
using Hmm.Core.Manager.Validation;
using Xunit;

namespace Hmm.Core.Tests
{
    public class UserManagerTests : TestFixtureBase
    {
        #region private fields

        private readonly IUserManager _usrManager;

        #endregion private fields

        public UserManagerTests()
        {
            InsertSeedRecords(isSetupDiscount: true, isSetupAutomobile: true);
            _usrManager = new UserManager(UserStorage);
        }

        [Fact]
        public void Can_Add_Valid_User()
        {
            // Arrange
            var user = new User
            {
                AccountName = "jfang2",
                FirstName = "Jack",
                LastName = "Fang",
                BirthDay = new DateTime(2016, 2, 15),
                IsActivated = true,
                Password = "1235",
            };

            // Act
            var newUsr = _usrManager.Create(user);

            // Assert
            Assert.True(_usrManager.ProcessResult.Success);
            Assert.NotNull(newUsr);
            Assert.True(newUsr.Id >= 1, "newUsr.Id >= 1");
            Assert.NotNull(newUsr.Salt);
        }

        //[Theory]
        //[InlineData("jfang", "Duplicated account name")]
        //public void Cannot_Add_Invalid_User(string accountName, string errorMessage)
        //{
        //    //    Arrange
        //    var user = new User
        //    {
        //        AccountName = accountName,
        //        FirstName = "Jack",
        //        LastName = "Fang",
        //        BirthDay = new DateTime(2016, 2, 15),
        //        IsActivated = true,
        //        Password = "1235",
        //    };

        //    //   Act
        //    var newUsr = _usrManager.Create(user);

        //    //  Assert
        //    Assert.False(_usrManager.ProcessResult.Success);
        //    Assert.True(_usrManager.ProcessResult.MessageList.FirstOrDefault()?.Contains(errorMessage));
        //    Assert.Null(newUsr);
        //}

        [Theory]
        [InlineData("jfang")]
        [InlineData("")]
        public void UserAccountNameValidationTests(string accountName)
        {
            // Arrange
            var validator = new UserValidator(_usrManager);

            // Act
            validator.ShouldHaveValidationErrorFor(u => u.AccountName, accountName);
        }
    }
}