using DomainEntity.Vehicle;
using Hmm.Contract.GasLogMan;
using Hmm.Core.Manager;
using Hmm.Utility.Currency;
using Hmm.Utility.MeasureUnit;
using Hmm.Utility.TestHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using VehicleInfoManager.GasLogMan;
using Xunit;

namespace VehicleInfoManager.Tests
{
    public class GasLogManagerTests : TestFixtureBase
    {
        private readonly IGasLogManager _manager;

        public GasLogManagerTests()
        {
            InsertSeedRecords();
            var noteManager = new HmmNoteManager(NoteStorage, LookupRepo);
            _manager = new GasLogManager(noteManager, LookupRepo);
        }

        [Fact]
        public void CanAddGasLog()
        {
            // Arrange
            var user = UserStorage.GetEntities().FirstOrDefault();
            var catalog = CatalogStorage.GetEntities().FirstOrDefault(cat => cat.Name == "Gas Log");
            var gasLog = new GasLog
            {
                Author = user,
                Catalog = catalog,
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
                        Program = "Patrol Canada RBC connection"
                    }
                }
            };

            // Act
            var newGas = _manager.CreateLog(gasLog);

            // Assert
            Assert.True(newGas.Id >= 1, "newGas.Id >= 1");
            Assert.True(_manager.ProcessResult.Success);
        }

        //        [Fact]
        //        public void CanUpdateGasLog()
        //        {
        //            throw new NotImplementedException();
        //        }

        //        [Fact]
        //        public void CanDeleteCasLog()
        //        {
        //            throw new NotImplementedException();
        //        }

        //        [Fact]
        //        public void CanFindGasLog()
        //        {
        //            throw new NotImplementedException();
        //        }
    }
}