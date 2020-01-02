using DomainEntity.User;
using DomainEntity.Vehicle;
using Hmm.Utility.Misc;
using System.Linq;

namespace Hmm.Contract.VehicleInfoManager
{
    public interface IDiscountManager
    {
        IQueryable<GasDiscount> GetDiscounts();

        GasDiscount GetDiscountById(int id);

        GasDiscount Create(GasDiscount discount, User author);

        GasDiscount Update(GasDiscount discount);

        ProcessingResult ProcessResult { get; }
    }
}