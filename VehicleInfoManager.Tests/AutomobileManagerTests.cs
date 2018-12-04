using DomainEntity.Vehicle;
using Hmm.Contract.GasLogMan;
using Hmm.Utility.TestHelp;
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
            InsertSeedRecords();
            _manager = new AutomobileManager(NoteManager, LookupRepo);
        }

        [Fact]
        public void CanCreateAutoMobile()
        {
            // Arrange
            var user = UserStorage.GetEntities().FirstOrDefault();
            var car = new Automobile
            {
                Author = user,
                Brand = "AutoBack",
                Maker = "Subaru",
                Content = "Blue",
                MeterReading = 100,
                Year = "2018",
                Pin = "1234",
                Description = "Testing car"
            };

            // Act
            var savedCar = _manager.Create(car);

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
            var car = new Automobile
            {
                Author = user,
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
            car.Brand = "AutoBack1";
            var savedCar = _manager.Update(car);

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
            var car = new Automobile
            {
                Author = user,
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
            _manager.Create(car);
        }
    }
}