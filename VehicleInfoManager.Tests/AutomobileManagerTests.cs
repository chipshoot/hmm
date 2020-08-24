using Hmm.Contract.VehicleInfoManager;
using Hmm.DomainEntity.Vehicle;
using Hmm.Utility.TestHelp;
using System.Collections.Generic;
using System.Linq;
using VehicleInfoManager.GasLogMan;
using Xunit;

namespace VehicleInfoManager.Tests
{
    public class AutomobileManagerTests : TestFixtureBase
    {
        private readonly IAutoEntityManager<Automobile> _manager;

        public AutomobileManagerTests()
        {
            InsertSeedRecords();
            _manager = new AutomobileManager(NoteManager, LookupRepo, DateProvider);
        }

        [Fact]
        public void CanCreateAutoMobile()
        {
            // Arrange
            var user = UserRepository.GetEntities().FirstOrDefault();
            var car = new Automobile
            {
                Brand = "AutoBack",
                Maker = "Subaru",
                MeterReading = 100,
                Year = "2018",
                Pin = "1234",
            };

            // Act
            var savedCar = _manager.Create(car, user);

            // Assert
            Assert.True(_manager.ProcessResult.Success);
            Assert.NotNull(savedCar);
            Assert.True(savedCar.Id >= 1, "car.Id >= 1");
        }

        [Fact]
        public void CanUpdateAutoMobile()
        {
            // Arrange
            var user = UserRepository.GetEntities().FirstOrDefault();
            var car = SetupEnvironment().FirstOrDefault();
            Assert.NotNull(car);

            // Act
            car.Brand = "AutoBack1";
            var savedCar = _manager.Update(car, user);

            // Assert
            Assert.True(_manager.ProcessResult.Success);
            Assert.NotNull(savedCar);
            Assert.True(savedCar.Brand == "AutoBack1", "savedCar.Brand=='AutoBack1'");
        }

        [Fact]
        public void CanGetAutoMobile()
        {
            // Arrange
            SetupEnvironment();

            // Act
            var savedCars = _manager.GetEntities(null);

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

        private IEnumerable<Automobile> SetupEnvironment()
        {
            var user = UserRepository.GetEntities().FirstOrDefault();
            var car = new Automobile
            {
                Brand = "AutoBack",
                Maker = "Subaru",
                MeterReading = 100,
                Year = "2018",
                Pin = "1234",
            };
            _manager.Create(car, user);

            return _manager.GetEntities(null).ToList();
        }
    }
}