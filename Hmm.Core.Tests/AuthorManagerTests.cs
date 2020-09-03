using FluentValidation.TestHelper;
using Hmm.Contract.Core;
using Hmm.Core.Manager;
using Hmm.Core.Manager.Validation;
using Hmm.DomainEntity.User;
using Hmm.Utility.TestHelp;
using System;
using System.Linq;
using Xunit;

namespace Hmm.Core.Tests
{
    public class AuthorManagerTests : TestFixtureBase
    {
        #region private fields

        private readonly IAuthorManager _authorManager;

        #endregion private fields

        public AuthorManagerTests()
        {
            InsertSeedRecords(isSetupDiscount: true, isSetupAutomobile: true);
            _authorManager = new AuthorManager(AuthorRepository, new AuthorValidator(AuthorRepository));
        }

        [Fact]
        public void Can_Get_User()
        {
            // Act
            var users = _authorManager.GetEntities().ToList();

            // Assert
            Assert.True(_authorManager.ProcessResult.Success);
            Assert.True(users.Count > 1, "users.Count > 1");
        }

        [Fact]
        public void Can_Add_Valid_User()
        {
            // Arrange
            var user = new Author
            {
                AccountName = "jfang2",
                IsActivated = true,
            };

            // Act
            var newUsr = _authorManager.Create(user);

            // Assert
            Assert.True(_authorManager.ProcessResult.Success);
            Assert.NotNull(newUsr);
            Assert.True(newUsr.Id != Guid.Empty, "newUsr.Id is not empty");
        }

        [Theory]
        [InlineData("jfang", "Duplicated account name")]
        public void Cannot_Add_Invalid_User(string accountName, string errorMessage)
        {
            // Arrange
            var user = new Author
            {
                AccountName = accountName,
                IsActivated = true,
            };

            // Act
            var newUsr = _authorManager.Create(user);

            // Assert
            Assert.False(_authorManager.ProcessResult.Success);
            Assert.True(_authorManager.ProcessResult.MessageList.FirstOrDefault()?.Message.Contains(errorMessage));
            Assert.Null(newUsr);
        }

        [Fact]
        public void Can_Update_Valid_User()
        {
            //    Arrange
            var user = new Author
            {
                AccountName = "jfang2",
                IsActivated = true,
            };
            _authorManager.Create(user);
            Assert.True(user.Id != Guid.Empty, "user.Id is not Guid empty");

            //   Act
            var newUsr = _authorManager.Update(user);

            //  Assert
            Assert.True(_authorManager.ProcessResult.Success);
            Assert.False(_authorManager.ProcessResult.MessageList.Any());
            Assert.NotNull(newUsr);
        }

        [Theory]
        [InlineData("jfang", "Duplicated account name")]
        public void Cannot_Update_Valid_User(string accountName, string errorMsg)
        {
            //    Arrange
            var user = new Author
            {
                AccountName = "jfang2",
                IsActivated = true,
            };
            _authorManager.Create(user);
            Assert.True(user.Id != Guid.Empty, "user.Id is not empty Guid");

            //   Act
            user.AccountName = accountName;
            var newUsr = _authorManager.Update(user);

            //  Assert
            Assert.False(_authorManager.ProcessResult.Success);
            Assert.True(_authorManager.ProcessResult.MessageList.FirstOrDefault()?.Message.Contains(errorMsg));
            Assert.Null(newUsr);
        }

        [Fact]
        public void Can_Deactivate_User()
        {
            // Arrange
            var user = _authorManager.GetEntities().FirstOrDefault();
            Assert.NotNull(user);
            Assert.True(user.IsActivated);

            // Act
            _authorManager.DeActivate(user.Id);

            // Assert
            Assert.True(_authorManager.ProcessResult.Success);
            Assert.False(_authorManager.ProcessResult.MessageList.Any());
            Assert.False(user.IsActivated);
        }

        [Theory]
        [InlineData("jfang")]
        [InlineData("")]
        public void UserAccountNameValidationTests(string accountName)
        {
            // Arrange
            var validator = new AuthorValidator(AuthorRepository);

            // Act
            validator.ShouldHaveValidationErrorFor(u => u.AccountName, accountName);
        }
    }
}