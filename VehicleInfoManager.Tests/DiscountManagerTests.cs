using Hmm.Contract.VehicleInfoManager;
using Hmm.DomainEntity.Enumerations;
using Hmm.DomainEntity.Vehicle;
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
        private readonly IAutoEntityManager<GasDiscount> _manager;

        public DiscountManagerTests()
        {
            InsertSeedRecords();
            _manager = new DiscountManager(NoteManager, LookupRepo, DateProvider);
        }

        [Fact]
        public void CanCreateDiscount()
        {
            // Arrange
            var user = UserRepository.GetEntities().FirstOrDefault();
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
            var user = UserRepository.GetEntities().FirstOrDefault();
            Assert.NotNull(discount);

            // Act
            discount.Program = "Petro-Canada";
            var savedDiscount = _manager.Update(discount, user);

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
            var savedDiscounts = _manager.GetEntities(null).ToList();

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
            var savedDiscount = _manager.GetEntityById(discounts.Select(d => d.Id).Max());

            // Assert
            Assert.True(_manager.ProcessResult.Success);
            Assert.NotNull(savedDiscount);
            Assert.True(savedDiscount.Id >= 1, "savedDiscount.Id >= 1");
            Assert.Equal(GasDiscountType.PreLiter, savedDiscount.DiscountType);
            Assert.Equal("Petro-Canada membership", savedDiscount.Program);
        }

        private List<GasDiscount> SetupEnvironment()
        {
            var discounts = new List<GasDiscount>();

            var user = UserRepository.GetEntities().FirstOrDefault();
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
    }
}