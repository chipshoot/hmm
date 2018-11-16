using Hmm.Contract.GasLogMan;
using Hmm.Utility.Validation;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Hmm.Api.Areas.GasLogNote.Controllers
{
    [Route("api/gaslogs/discounts")]
    public class GasDiscountController : Controller
    {
        private readonly IGasLogManager _gasLogManager;

        public GasDiscountController(IGasLogManager gasLogManager)
        {
            Guard.Against<ArgumentNullException>(gasLogManager == null, nameof(gasLogManager));

            _gasLogManager = gasLogManager;
        }

        // GET api/gaslogs/discounts/
        //public IActionResult Get()
        //{
        //    //var discounts = _gasLogManager.FindGasDiscounts().ToList();
        //    var config = ApiDomainEntityConvertHelper.DomainEntity2Api();
        //    var mapper = config.CreateMapper();
        //    //var apiDiscountInfos = mapper.Map<ApiDiscountInfo>(discounts);

        //    return Ok(apiDiscountInfos);
        //}

        //// GET api/gaslogs/discounts/1
        //[HttpGet("{id}")]
        //public IActionResult Get(int discountId)
        //{
        //    var discount = _gasLogManager.FindGasDiscounts().FirstOrDefault(d=>d.Id == discountId);
        //    var config = ApiDomainEntityConvertHelper.DomainEntity2Api();
        //    var mapper = config.CreateMapper();
        //    var apiDiscountInfo = mapper.Map<ApiDiscountInfo>(discount);

        //    return Ok(apiDiscountInfo);
        //}
    }
}