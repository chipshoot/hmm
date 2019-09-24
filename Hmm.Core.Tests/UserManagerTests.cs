using DomainEntity.User;
using FluentValidation.TestHelper;
using Hmm.Contract.Core;
using Hmm.Core.Manager;
using Hmm.Core.Manager.Validation;
using Hmm.Utility.TestHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using Hmm.Utility.Misc;
using Xunit;

namespace Hmm.Core.Tests
{
    public class UserManagerTests : TestFixtureBase
    {
        #region private fields

        private readonly IUserManager _userManager;

        #endregion private fields

        public UserManagerTests()
        {
            InsertSeedRecords(isSetupDiscount: true, isSetupAutomobile: true);
            _userManager = new UserManager(UserStorage, new UserValidator(UserStorage));
        }

        [Fact]
        public void Can_Get_User()
        {
            // Act
            var users = _userManager.GetEntities().ToList();

            // Assert
            Assert.True(_userManager.ProcessResult.Success);
            Assert.True(users.Count > 1, "users.Count > 1");
            Assert.Equal("Jack", users.FirstOrDefault()?.FirstName);
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
            var newUsr = _userManager.Create(user);

            // Assert
            Assert.True(_userManager.ProcessResult.Success);
            Assert.NotNull(newUsr);
            Assert.True(newUsr.Id >= 1, "newUsr.Id >= 1");
            Assert.NotNull(newUsr.Salt);
        }

        [Theory]
        [InlineData("jfang", "Duplicated account name")]
        public void Cannot_Add_Invalid_User(string accountName, string errorMessage)
        {
            //    Arrange
            var user = new User
            {
                AccountName = accountName,
                FirstName = "Jack",
                LastName = "Fang",
                BirthDay = new DateTime(2016, 2, 15),
                IsActivated = true,
                Password = "1235",
            };

            //   Act
            var newUsr = _userManager.Create(user);

            //  Assert
            Assert.False(_userManager.ProcessResult.Success);
            Assert.True(_userManager.ProcessResult.MessageList.FirstOrDefault()?.Contains(errorMessage));
            Assert.Null(newUsr);
        }

        [Fact]
        public void Can_Update_Valid_User()
        {
            //    Arrange
            var user = new User
            {
                AccountName = "jfang2",
                FirstName = "Jack",
                LastName = "Fang",
                BirthDay = new DateTime(2016, 2, 15),
                IsActivated = true,
                Password = "1235",
            };
            _userManager.Create(user);
            Assert.True(user.Id >= 1, "user.Id>=1");

            //   Act
            user.FirstName = "Chaoyang";
            user.BirthDay = new DateTime(1967, 03, 13);
            var newUsr = _userManager.Update(user);

            //  Assert
            Assert.True(_userManager.ProcessResult.Success);
            Assert.False(_userManager.ProcessResult.MessageList.Any());
            Assert.NotNull(newUsr);
            Assert.Equal("Chaoyang", newUsr.FirstName);
            Assert.Equal(1967, newUsr.BirthDay.Year);
        }

        [Theory]
        [InlineData("jfang", "Duplicated account name")]
        public void Cannot_Update_Valid_User(string accountName, string errorMsg)
        {
            //    Arrange
            var user = new User
            {
                AccountName = "jfang2",
                FirstName = "Jack",
                LastName = "Fang",
                BirthDay = new DateTime(2016, 2, 15),
                IsActivated = true,
                Password = "1235",
            };
            _userManager.Create(user);
            Assert.True(user.Id >= 1, "user.Id>=1");

            //   Act
            user.AccountName = accountName;
            user.BirthDay = new DateTime(1967, 03, 13);
            var newUsr = _userManager.Update(user);

            //  Assert
            Assert.False(_userManager.ProcessResult.Success);
            Assert.True(_userManager.ProcessResult.MessageList.FirstOrDefault()?.Contains(errorMsg));
            Assert.Null(newUsr);
        }

        [Fact]
        public void Can_Deactivate_User()
        {
            // Arrange
            var user = _userManager.GetEntities().FirstOrDefault();
            Assert.NotNull(user);
            Assert.True(user.IsActivated);

            // Act
            _userManager.DeActivate(user.Id);

            // Assert
            Assert.True(_userManager.ProcessResult.Success);
            Assert.False(_userManager.ProcessResult.MessageList.Any());
            Assert.False(user.IsActivated);
        }

        [Theory]
        [InlineData("jfang")]
        [InlineData("")]
        public void UserAccountNameValidationTests(string accountName)
        {
            // Arrange
            var validator = new UserValidator(UserStorage);

            // Act
            validator.ShouldHaveValidationErrorFor(u => u.AccountName, accountName);
        }

        [Theory]
        [MemberData(nameof(GetInvalidBirthDay))]
        public void BirthDayValidationTests(DateTime birthDay)
        {
            // Arrange
            var validator = new UserValidator(UserStorage);
            var user = new User { AccountName = "UniquId", BirthDay = birthDay };

            // Act
            validator.ShouldHaveValidationErrorFor(u => u.BirthDay, user);
        }

        public static IEnumerable<object[]> GetInvalidBirthDay()
        {
            var allDate = new List<object[]>
            {
                new object[] {DateTime.Now},
                new object[] {DateTime.MaxValue},
                new object[] {DateTime.MinValue},
                new object[] {DateTime.Now.AddYears(1)}
            };

            return allDate;
        }
    }
}