using DomainEntity.Enumerations;
using DomainEntity.Vehicle;
using Hmm.Contract.GasLogMan;
using Hmm.Utility.Currency;
using Hmm.Utility.TestHelp;
using System.Collections.Generic;
using System.Linq;
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
                Author = user,
                Program = "Costco membership",
                Amount = new Money(0.6),
                DiscountType = GasDiscountType.PreLiter,
                Comment = "Test Discount",
                IsActive = true,
            };

            // Act
            var savedDisc = _manager.CreateDiscount(discount);

            // Assert
            Assert.NotNull(savedDisc);
            Assert.True(savedDisc.Id >= 1, "savedDisc.Id>=1");
            Assert.Equal(savedDisc.Id, discount.Id);
        }

        [Fact]
        public void CanUpdateDiscount()
        {
            // Arrange
            var ids = SetupEnvironment();
            var discount = _manager.GetDiscountById(ids.Min());

            // Act
            discount.Program = "Petro-Canada";
            var savedDiscount = _manager.UpdateDiscount(discount);

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
            var ids = SetupEnvironment();

            // Act
            var savedDiscount = _manager.GetDiscountById(ids.Max());

            // Assert
            Assert.True(_manager.ProcessResult.Success);
            Assert.NotNull(savedDiscount);
            Assert.True(savedDiscount.Id >= 1, "savedDiscount.Id >= 1");
            Assert.Equal("Petro-Canada membership", savedDiscount.Program);
        }

        private List<int> SetupEnvironment()
        {
            var ids = new List<int>();

            var user = UserStorage.GetEntities().FirstOrDefault();
            var discount = new GasDiscount
            {
                Author = user,
                Program = "Costco membership",
                Amount = new Money(0.6),
                DiscountType = GasDiscountType.PreLiter,
                Comment = "Test Discount",
                IsActive = true,
            };
            _manager.CreateDiscount(discount);
            ids.Add(discount.Id);

            discount = new GasDiscount
            {
                Author = user,
                Program = "Petro-Canada membership",
                Amount = new Money(0.2),
                DiscountType = GasDiscountType.PreLiter,
                Comment = "Test Discount 2",
                IsActive = true,
            };

            _manager.CreateDiscount(discount);
            ids.Add(discount.Id);

            return ids;
        }
    }
}