using DomainEntity.User;
using Hmm.Dal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xunit;

namespace Hmm.Dal.Tests
{
    public class UnitOfWorkTests : IDisposable
    {
        private readonly IHmmDataContext _dbContext;

        public UnitOfWorkTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            var connStr = configuration.GetConnectionString("DefaultConnection");
            var optBuilder = new DbContextOptionsBuilder<HmmDataContext>().UseSqlServer(connStr);
            var options = optBuilder.Options;
            _dbContext = new HmmDataContext(options);
        }

        public void Dispose()
        {
            var context = _dbContext as DbContext;
            context?.Dispose();
        }

        [Fact]
        public void AddCanSaveUser()
        {
            // Arrange
            var usr = new User
            {
                AccountName = "jfang",
                FirstName = "Jack",
                LastName = "Fang",
                BirthDay = new DateTime(2016, 2, 15),
                IsActivated = true,
                Password = "1235",
                Salt = "81C70F747E094C05B6EEB4457B3A40D1"
            };

            // Act
            User newUser;
            using (var uow = new EfUnitOfWork(_dbContext))
            {
                newUser = uow.Add(usr);
            }

            // Assert
            Assert.NotNull(newUser);
            Assert.True(newUser.Id > 0);
        }
    }
}