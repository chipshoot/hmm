using DomainEntity.Enumerations;
using Hmm.Utility.Currency;
using Hmm.Utility.MeasureUnit;
using Hmm.Utility.TestHelp;
using System.Collections.Generic;
using System.Linq;
using DomainEntity.Vehicle;
using Hmm.Contract.VehicleInfoManager;
using VehicleInfoManager.GasLogMan;
using Xunit;

namespace VehicleInfoManager.Tests
{
    public class DiscountManagerTests : TestFixtureBase
    {
        private readonly IDiscountManager _manager;

        public DiscountManagerTests()
        {
            InsertSeedRecords();
            _manager = new DiscountManager(NoteManager, LookupRepo);
        }

        [Fact]
        public void CanCreateDiscount()
        {
            // Arrange
            var user = UserStorage.GetEntities().FirstOrDefault();
            var discount = new GasDiscount
            {
                Program = "Costco membership",
                Amount = new Money(0.6),
                DiscountType = GasDiscountType.PreLiter,
                Comment = "Test Discount",
                IsActive = true,
            };

            // Act
            var savedDisc = _manager.Create(discount, user);

            // Assert
            Assert.NotNull(savedDisc);
            Assert.True(savedDisc.Id >= 1, "savedDisc.Id>=1");
        }

        [Fact]
        public void CanUpdateDiscount()
        {
            // Arrange
            var discounts = SetupEnvironment();
            var discount = discounts.OrderByDescending(d => d.Id).FirstOrDefault();
            Assert.NotNull(discount);

            // Act
            discount.Program = "Petro-Canada";
            var savedDiscount = _manager.Update(discount);

            // Assert
            Assert.True(_manager.ProcessResult.Success);
            Assert.NotNull(savedDiscount);
            Assert.True(savedDiscount.Program == "Petro-Canada", "savedDiscount.Program=='Petro-Canada'");
        }

        [Fact]
        public void CanGetDiscounts()
        {
            //Arrange
            SetupEnvironment();

            // Act
            var savedDiscounts = _manager.GetDiscounts().ToList();

            // Assert
            Assert.True(_manager.ProcessResult.Success);
            Assert.NotNull(savedDiscounts);
            Assert.Equal(2, savedDiscounts.Count);
        }

        [Fact]
        public void CanGetDiscountById()
        {
            //Arrange
            var discounts = SetupEnvironment();

            // Act
            var savedDiscount = _manager.GetDiscountById(discounts.Select(d => d.Id).Max());

            // Assert
            Assert.True(_manager.ProcessResult.Success);
            Assert.NotNull(savedDiscount);
            Assert.True(savedDiscount.Id >= 1, "savedDiscount.Id >= 1");
            Assert.Equal(GasDiscountType.PreLiter, savedDiscount.DiscountType);
            Assert.Equal("Petro-Canada membership", savedDiscount.Program);
        }

        //        [Fact]
        //        public void CanGetDiscountInfos()
        //        {
        //            // Arrange
        //            var gasLog = GetGasLogsForDiscountInfo().FirstOrDefault();
        //            Assert.NotNull(gasLog);

        //            // Act
        //            var discountInfos = _manager.GetDiscountInfos(gasLog).ToList();

        //            // Assert
        //            Assert.NotNull(discountInfos);
        //            Assert.True(discountInfos.Any());
        //            var discountInfo = discountInfos.FirstOrDefault();
        //            Assert.NotNull(discountInfo);
        //            Assert.True(discountInfo.Amount.Amount == 5m, "discountInfo.Amount.Amount == 5m");
        //            Assert.True(discountInfo.Program.DiscountType == GasDiscountType.PreLiter, "discountInfo.Program.DiscountType == GasDiscountType.PreLiter");
        //            Assert.True(discountInfo.Program.Program == "Costco membership", "discountInfo.Program.Program == 'Costco membership'");
        //        }

        private List<GasDiscount> SetupEnvironment()
        {
            var discounts = new List<GasDiscount>();

            var user = UserStorage.GetEntities().FirstOrDefault();
            var discount = new GasDiscount
            {
                Program = "Costco membership",
                Amount = new Money(0.6),
                DiscountType = GasDiscountType.PreLiter,
                Comment = "Test Discount",
                IsActive = true,
            };
            var rec = _manager.Create(discount, user);
            discounts.Add(rec);

            discount = new GasDiscount
            {
                Program = "Petro-Canada membership",
                Amount = new Money(0.2),
                DiscountType = GasDiscountType.PreLiter,
                Comment = "Test Discount 2",
                IsActive = true,
            };

            rec = _manager.Create(discount, user);
            discounts.Add(rec);

            NoTrackingEntities();

            return discounts;
        }

        //        private IEnumerable<GasLog> GetGasLogsForDiscountInfo()
        //        {
        //            var gasLogs = new List<GasLog>();

        //            //// insert new discount
        //            //SetupEnvironment();

        //            //// insert sample car
        //            //var user = UserStorage.GetEntities().FirstOrDefault();
        //            //var carMan = new AutomobileManager(NoteManager, LookupRepo);

        //            //carMan.Create(new Automobile
        //            //{
        //            //    Brand = "AutoBack",
        //            //    Maker = "Subaru",
        //            //    MeterReading = 100,
        //            //    Year = "2018",
        //            //    Pin = "1234",
        //            //});

        //            //// insert sample gas log
        //            //var logMan = new GasLogManager(NoteManager, carMan, _manager, LookupRepo);
        //            //var gasLog = logMan.CreateLog(new GasLog
        //            //{
        //            //    Author = user,
        //            //    Car = carMan.GetAutomobiles().FirstOrDefault(),
        //            //    Distance = Dimension.FromKilometre(400),
        //            //    Gas = Volume.FromLiter(41),
        //            //    Station = "Costco",
        //            //    Price = new Money(40),
        //            //    Discounts = new List<GasDiscountInfo>
        //            //    {
        //            //        new GasDiscountInfo
        //            //        {
        //            //            Amount = new Money(5),
        //            //            Program = _manager.GetDiscounts().FirstOrDefault()
        //            //        }
        //            //    }
        //            //});

        //            //gasLogs.Add(gasLog);
        //            return gasLogs;
        //        }
    }
}