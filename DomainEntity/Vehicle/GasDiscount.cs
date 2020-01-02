using DomainEntity.Enumerations;
using Hmm.Utility.Currency;

namespace DomainEntity.Vehicle
{
    /// <summary>
    /// The class describe the information of a discount,
    /// when add a gas log to data source, user can select
    /// and apply the discount to log
    /// </summary>
    public class GasDiscount : VehicleBase
    {
        public string Program { get; set; }

        public Money Amount { get; set; }

        public GasDiscountType DiscountType { get; set; }

        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the comment, the way of calculating
        /// discount can be added to comment, e.g. 0.2c/liter
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        public string Comment { get; set; }
    }
}