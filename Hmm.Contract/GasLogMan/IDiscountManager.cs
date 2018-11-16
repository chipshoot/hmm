using System.Collections.Generic;
using DomainEntity.Vehicle;
using Hmm.Utility.Misc;

namespace Hmm.Contract.GasLogMan
{
    public interface IDiscountManager
    {
        IEnumerable<GasDiscount> GetDiscounts();

        GasDiscount GetDiscountById(int id);

        GasDiscount CreateDiscount(GasDiscount discount);

        GasDiscount UpdateDiscount(GasDiscount discount);

        ProcessingResult ProcessResult { get; }
    }
}