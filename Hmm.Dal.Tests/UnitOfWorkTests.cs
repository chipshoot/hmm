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
        private readonly IHmmDataContext _dbcontext;

        public UnitOfWorkTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();
            var connstr = configuration.GetConnectionString("DefaultConnection");
            var optBuilder = new DbContextOptionsBuilder<HmmDataContext>().UseSqlServer(connstr);
            var options = optBuilder.Options;
            _dbcontext = new HmmDataContext(options);
        }

        public void Dispose()
        {
            var context = _dbcontext as DbContext;
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

            var uow = new EfUnitOfWork(_dbcontext);

            // Act
            var newusr = uow.Add(usr);

            // Assert
            Assert.NotNull(newusr);
            Assert.True(newusr.Id > 0);
        }
    }
}