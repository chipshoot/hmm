using System.Collections.Generic;
using DomainEntity.Vehicle;
using Hmm.Utility.Misc;

namespace Hmm.Contract.GasLogMan
{
    public interface IDiscountManager
    {
        IEnumerable<GasDiscount> GetDiscounts();


        /// <summary>
        /// Gets the discount infos from gas log record.
        /// <remarks>
        /// The method will return the discount information of the log, include the amount of discount
        /// and discount program information
        /// </remarks>
        /// </summary>
        /// <param name="gasLog">The gas log which contains discount information.</param>
        /// <returns>
        /// The discount information list if it contains in <see cref="GasLog"/>, if cannot find discount information
        /// empty list returned
        /// </returns>
        IEnumerable<GasDiscountInfo> GetDiscountInfos(GasLog gasLog);

        GasDiscount GetDiscountById(int id);

        GasDiscount CreateDiscount(GasDiscount discount);

        GasDiscount UpdateDiscount(GasDiscount discount);

        ProcessingResult ProcessResult { get; }
    }
}