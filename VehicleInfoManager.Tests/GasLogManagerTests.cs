using DomainEntity.Vehicle;
using Hmm.Contract.GasLogMan;
using Hmm.Core.Manager;
using Hmm.Utility.Currency;
using Hmm.Utility.MeasureUnit;
using Hmm.Utility.TestHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using Hmm.Core.Manager.Validation;
using VehicleInfoManager.GasLogMan;
using Xunit;

namespace VehicleInfoManager.Tests
{
    public class GasLogManagerTests : TestFixtureBase
    {
        private readonly IGasLogManager _manager;
        private readonly IAutomobileManager _carManager;
        private readonly IDiscountManager _discountManager;

        public GasLogManagerTests()
        {
            InsertSeedRecords(isSetupDiscount: true, isSetupAutomobile: true);
            var noteManager = new HmmNoteManager(NoteStorage, LookupRepo, new NoteValidator(NoteStorage));
            _carManager = new AutomobileManager(noteManager, LookupRepo);
            _discountManager = new DiscountManager(noteManager, LookupRepo);
            _manager = new GasLogManager(noteManager, _carManager, _discountManager, LookupRepo);
        }

        [Fact]
        public void CanAddGasLog()
        {
            // Arrange
            var user = UserStorage.GetEntities().FirstOrDefault();
            var car = _carManager.GetAutomobiles().FirstOrDefault();
            var discount = _discountManager.GetDiscounts().FirstOrDefault();
            var gasLog = new GasLog
            {
                Author = user,
                Car = car,
                GasStation = "Costco",
                Gas = Volume.FromLiter(40),
                Price = new Money(40.0),
                Distance = Dimension.FromKilometre(300),
                CreateDate = DateTime.UtcNow,
                Discounts = new List<GasDiscountInfo>
                {
                    new GasDiscountInfo
                    {
                        Amount = new Money(0.8),
                        Program = discount
                    }
                }
            };

            // Act
            var newGas = _manager.CreateLog(gasLog);

            // Assert
            Assert.True(_manager.ProcessResult.Success);
            Assert.True(newGas.Id >= 1, "newGas.Id >= 1");
            Assert.NotNull(newGas.Car);
            Assert.NotNull(newGas.Discounts);
            Assert.True(newGas.Discounts.Any());
            Assert.True(newGas.Discounts.FirstOrDefault()?.Amount.Amount == 0.8m);
        }

        [Theory]
        [InlineData(250)]
        public void CanUpdateGasLog(int distance)
        {
            // Arrange
            var gas = InsertSampleGasLog();
            Assert.NotNull(gas);
            var orgDistance = gas.Distance;
            var newDistance = Dimension.FromKilometre(distance);
            gas.Distance = newDistance;

            // Act
            var updatedGasLog = _manager.UpdateGasLog(gas);

            // Assert
            Assert.True(_manager.ProcessResult.Success);
            Assert.NotNull(updatedGasLog);
            Assert.NotEqual(orgDistance, updatedGasLog.Distance);
            Assert.Equal(newDistance, updatedGasLog.Distance);
        }

        [Fact]
        public void CanFindGasLog()
        {
            // Arrange
            var id = InsertSampleGasLog().Id;

            // Act
            var gasLog = _manager.FindGasLog(id);

            // Assert
            Assert.NotNull(gasLog);
            Assert.Equal("Costco", gasLog.GasStation);
            Assert.NotNull(gasLog.Car);
            Assert.NotNull(gasLog.Discounts);
            Assert.True(gasLog.Discounts.Any());
            var disc = gasLog.Discounts.FirstOrDefault();
            Assert.NotNull(disc);
            Assert.Equal("Costco membership", disc.Program.Program);
        }

        private GasLog InsertSampleGasLog()
        {
            var user = UserStorage.GetEntities().FirstOrDefault();
            var car = _carManager.GetAutomobiles().FirstOrDefault();
            var discount = _discountManager.GetDiscounts().FirstOrDefault();
            var gasLog = new GasLog
            {
                Author = user,
                Car = car,
                GasStation = "Costco",
                Gas = Volume.FromLiter(40),
                Price = new Money(40.0),
                Distance = Dimension.FromKilometre(300),
                CreateDate = DateTime.UtcNow,
                Discounts = new List<GasDiscountInfo>
                {
                    new GasDiscountInfo
                    {
                        Amount = new Money(0.8),
                        Program = discount
                    }
                }
            };

            _manager.CreateLog(gasLog);

            return gasLog;
        }
    }
}