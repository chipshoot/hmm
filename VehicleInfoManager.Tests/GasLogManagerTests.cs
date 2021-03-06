﻿using System;
using Hmm.Contract.VehicleInfoManager;
using Hmm.DomainEntity.Vehicle;
using Hmm.Utility.Currency;
using Hmm.Utility.MeasureUnit;
using Hmm.Utility.TestHelp;
using System.Collections.Generic;
using System.Linq;
using VehicleInfoManager.GasLogMan;
using Xunit;

namespace VehicleInfoManager.Tests
{
    public class GasLogManagerTests : TestFixtureBase
    {
        private readonly IAutoEntityManager<GasLog> _manager;
        private readonly IAutoEntityManager<Automobile> _carManager;
        private readonly IAutoEntityManager<GasDiscount> _discountManager;

        public GasLogManagerTests()
        {
            InsertSeedRecords(isSetupDiscount: true, isSetupAutomobile: true);
            var noteManager = NoteManager;
            _carManager = new AutomobileManager(noteManager, LookupRepo, DateProvider);
            _discountManager = new DiscountManager(noteManager, LookupRepo, DateProvider);
            _manager = new GasLogManager(noteManager, _carManager, _discountManager, LookupRepo, DateProvider);
        }

        [Fact]
        public void CanAddGasLog()
        {
            // Arrange
            const string comment = "This is a test gas log";
            var author = AuthorRepository.GetEntities().FirstOrDefault();
            var car = _carManager.GetEntities(null).FirstOrDefault();
            var discount = _discountManager.GetEntities(null).FirstOrDefault();
            Assert.NotNull(author);
            var gasLog = new GasLog
            {
                Car = car,
                Station = "Costco",
                Gas = Volume.FromLiter(40),
                Price = new Money(40.0),
                Distance = Dimension.FromKilometre(300),
                CurrentMeterReading = Dimension.FromKilometre(12000),
                Discounts = new List<GasDiscountInfo>
                        {
                            new GasDiscountInfo
                            {
                                Amount = new Money(0.8),
                                Program = discount
                            }
                        },
                Comment = comment
            };

            // Act
            var newGas = _manager.Create(gasLog, author);

            // Assert
            Assert.True(_manager.ProcessResult.Success);
            Assert.True(newGas.Id >= 1, "newGas.Id >= 1");
            Assert.NotNull(newGas.Car);
            Assert.NotNull(newGas.Discounts);
            Assert.Equal(newGas.AuthorId, author.Id);
            Assert.Equal(newGas.Comment, comment);
            Assert.Equal(newGas.CurrentMeterReading, Dimension.FromKilometre(12000));
            Assert.Equal(newGas.Discounts.First().Amount, new Money(0.8));
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
            var author = AuthorRepository.GetEntities().FirstOrDefault();

            // Act
            var updatedGasLog = _manager.Update(gas, author);

            // Assert
            Assert.True(_manager.ProcessResult.Success);
            Assert.NotNull(updatedGasLog);
            Assert.NotEqual(orgDistance, updatedGasLog.Distance);
            Assert.Equal(newDistance, updatedGasLog.Distance);
        }

        [Fact]
        public void CanNotUpdateGasLogAuthorId()
        {
            // Arrange
            var gas = InsertSampleGasLog();
            Assert.NotNull(gas);
            gas.AuthorId = Guid.NewGuid();
            var author = AuthorRepository.GetEntities().FirstOrDefault();
            Assert.NotNull(author);

            // Act
            var updatedGasLog = _manager.Update(gas, author);

            // Assert
            Assert.True(_manager.ProcessResult.Success);
            Assert.NotNull(updatedGasLog);
            Assert.Equal(author.Id, updatedGasLog.AuthorId);
        }

        [Fact]
        public void CanFindGasLog()
        {
            // Arrange
            var id = InsertSampleGasLog().Id;

            // Act
            var gasLog = _manager.GetEntityById(id);

            // Assert
            Assert.NotNull(gasLog);
            Assert.Equal("Costco", gasLog.Station);
            Assert.NotNull(gasLog.Car);
            Assert.NotNull(gasLog.Discounts);
            Assert.True(gasLog.Discounts.Any());
            var disc = gasLog.Discounts.FirstOrDefault();
            Assert.NotNull(disc);
            Assert.Equal("Costco membership", disc.Program.Program);
        }

        private GasLog InsertSampleGasLog()
        {
            var author = AuthorRepository.GetEntities().FirstOrDefault();
            var car = _carManager.GetEntities(null).FirstOrDefault();
            var discount = _discountManager.GetEntities(null).FirstOrDefault();
            var gasLog = new GasLog
            {
                Car = car,
                Station = "Costco",
                Gas = Volume.FromLiter(40),
                Price = new Money(40.0),
                Distance = Dimension.FromKilometre(300),
                Discounts = new List<GasDiscountInfo>
                        {
                            new GasDiscountInfo
                            {
                                Amount = new Money(0.8),
                                Program = discount
                            }
                        }
            };

            var newLog = _manager.Create(gasLog, author);

            return newLog;
        }
    }
}