using DomainEntity.Misc;
using DomainEntity.User;
using DomainEntity.Vehicle;
using Hmm.Contract.GasLogMan;
using Hmm.Utility.TestHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using VehicleInfoManager.GasLogMan;
using Xunit;

namespace VehicleInfoManager.Tests
{
    public class AutomobileManagerTests : TestFixtureBase
    {
        private readonly IAutomobileManager _manager;

        public AutomobileManagerTests()
        {
            var authors = new List<User>
            {
                new User
                {
                    FirstName = "Jack",
                    LastName = "Fang",
                    AccountName = "jfang",
                    BirthDay = new DateTime(1977, 05, 21),
                    Password = "lucky1",
                    Salt = "passwordSalt",
                    IsActivated = true,
                    Description = "testing user"
                },
                new User
                {
                    FirstName = "Amy",
                    LastName = "Wang",
                    AccountName = "awang",
                    BirthDay = new DateTime(1977, 05, 21),
                    Password = "lucky1",
                    Salt = "passwordSalt",
                    IsActivated = true,
                    Description = "testing user"
                }
            };
            var renders = new List<NoteRender>
            {
                new NoteRender
                {
                    Name = "DefaultNoteRender",
                    Namespace = "Hmm.Renders",
                    IsDefault = true,
                    Description = "Testing default note render"
                },
                new NoteRender
                {
                    Name = "GasLog",
                    Namespace = "Hmm.Renders",
                    Description = "Testing default note render"
                }
            };
            var catalogs = new List<NoteCatalog>
            {
                new NoteCatalog
                {
                    Name = "DefaultNoteCatalog",
                    Schema = "DefaultSchema",
                    Render = renders[0],
                    IsDefault = true,
                    Description = "Testing catalog"
                },
                new NoteCatalog
                {
                    Name = "Gas Log",
                    Schema = "GasLogSchema",
                    Render = renders[1],
                    Description = "Testing catalog"
                },
                new NoteCatalog
                {
                    Name = "Automobile",
                    Schema = "AutomobileSchema",
                    Render = renders[0],
                    Description = "Testing automobile note"
                }
            };

            SetupRecords(authors, renders, catalogs);
            _manager = new AutomobileManager(NoteManager);
        }

        [Fact]
        public void CanCreateAutoMobile()
        {
            // Arrange
            var user = UserStorage.GetEntities().FirstOrDefault();
            var catalog = CatalogStorage.GetEntities().FirstOrDefault(c => c.Name == "Automobile");
            var car = new Automobile
            {
                Author = user,
                Catalog = catalog,
                Brand = "AutoBack",
                Maker = "Subaru",
                Content = "Blue",
                MeterReading = 100,
                Year = "2018",
                Pin = "1234",
                Description = "Testing car"
            };

            // Act
            var savedCar = _manager.CreateAutomobile(car);

            // Assert
            Assert.True(_manager.ProcessResult.Success);
            Assert.NotNull(savedCar);
            Assert.True(car.Id >= 1, "car.Id >= 1");
            Assert.True(car.Id == savedCar.Id, "car.Id == savedCar.Id");
            Assert.True(car.Subject == "Automobile");
        }

        [Fact]
        public void CanUpdateAutoMobile()
        {
            // Arrange
            var user = UserStorage.GetEntities().FirstOrDefault();
            var catalog = CatalogStorage.GetEntities().FirstOrDefault(c => c.Name == "Automobile");
            var car = new Automobile
            {
                Author = user,
                Catalog = catalog,
                Brand = "AutoBack",
                Maker = "Subaru",
                Content = "Blue",
                MeterReading = 100,
                Year = "2018",
                Pin = "1234",
                Description = "Testing car"
            };
            var savedCar = _manager.CreateAutomobile(car);
            Assert.True(_manager.ProcessResult.Success);
            Assert.NotNull(savedCar);

            // Act
            car.Brand = "AutoBack1";
            savedCar = _manager.UpdateAutomobile(car);

            // Assert
            Assert.True(_manager.ProcessResult.Success);
            Assert.NotNull(savedCar);
            Assert.True(savedCar.Brand == "AutoBack1", "savedCar.Brand=='AutoBack1'");
        }

        [Fact]
        public void CanGetAutoMobile()
        {
            // Arrange
            var user = UserStorage.GetEntities().FirstOrDefault();
            var catalog = CatalogStorage.GetEntities().FirstOrDefault(c => c.Name == "Automobile");
            var car = new Automobile
            {
                Author = user,
                Catalog = catalog,
                Brand = "AutoBack",
                Maker = "Subaru",
                Content = "Blue",
                MeterReading = 100,
                Year = "2018",
                Pin = "1234",
                Description = "Testing car"
            };
            SetupEnvironment(car);

            // Act
            var savedCars = _manager.GetAutomobiles();

            // Assert
            Assert.True(_manager.ProcessResult.Success);
            Assert.NotNull(savedCars);
            var automobiles = savedCars.ToList();
            Assert.True(automobiles.Count == 1, "savedCars.Count == 1");
            var savedCar = automobiles.FirstOrDefault();
            Assert.NotNull(savedCar);
            Assert.Equal("AutoBack", savedCar.Brand);
            Assert.True(savedCar.Id >= 1, "savedCar.Id>=1");
        }

        private void SetupEnvironment(Automobile car)
        {
            _manager.CreateAutomobile(car);
        }
    }
}