using AutoMapper;
using DomainEntity.Vehicle;
using Hmm.Api.Areas.GasLogNote.Models;
using Hmm.Api.Models;
using Hmm.Contract.Core;
using Hmm.Contract.GasLogMan;
using Hmm.Utility.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hmm.Api.Areas.GasLogNote.Controllers
{
    [Route("api/automobiles/gaslogs/discounts")]
    public class GasDiscountController : Controller
    {
        private readonly IDiscountManager _discountManager;
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        public GasDiscountController(IDiscountManager discountManager, IMapper mapper, IUserManager userManager)
        {
            Guard.Against<ArgumentNullException>(discountManager == null, nameof(discountManager));
            Guard.Against<ArgumentNullException>(mapper == null, nameof(mapper));
            Guard.Against<ArgumentNullException>(userManager == null, nameof(userManager));

            _discountManager = discountManager;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET api/automobiles/gaslogs/discounts/
        [HttpGet]
        public IActionResult GetDiscountsForAuthor(int authorId, bool getAll = false)
        {
            try
            {
                var discounts = getAll
                    ? _discountManager.GetDiscounts().Where(d => d.Author.Id == authorId).ToList()
                    : _discountManager.GetDiscounts().Where(d => d.Author.Id == authorId && d.IsActive).ToList();
                var apiDiscountInfos = discounts.Select(d => _mapper.Map<ApiDiscount>(d)).ToList();
                return Ok(apiDiscountInfos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/automobiles/gaslogs/discounts/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var discount = _discountManager.GetDiscounts().FirstOrDefault(d => d.Id == id);
                if (discount == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                var apiDiscount = _mapper.Map<ApiDiscount>(discount);
                return Ok(apiDiscount);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // post api/automobiles/gaslogs/discounts?authorId=1
        [HttpPost]
        public IActionResult Post(int authorId, [FromBody] ApiDiscountForCreate[] apiDiscounts)
        {
            try
            {
                if (apiDiscounts == null || !apiDiscounts.Any())
                {
                    return BadRequest(new ApiBadRequestResponse("null or empty discounts found"));
                }

                var author = _userManager.GetEntities().FirstOrDefault(u => u.Id == authorId);
                if (author == null)
                {
                    return BadRequest(new ApiBadRequestResponse("Cannot find author in data source"));
                }

                var newDiscounts = new List<ApiDiscount>();
                foreach (var apiDiscount in apiDiscounts)
                {
                    var discount = _mapper.Map<GasDiscount>(apiDiscount);
                    discount.Author = author;
                    var newDiscount = _discountManager.Create(discount);
                    if (newDiscount == null)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }

                    var apiNewDiscount = _mapper.Map<ApiDiscount>(newDiscount);
                    newDiscounts.Add(apiNewDiscount);
                }

                return Ok(newDiscounts);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/automobiles/gaslogs/discount/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]ApiDiscountForUpdate apiDiscount)
        {
            if (apiDiscount == null)
            {
                return BadRequest(new ApiBadRequestResponse("null gas discount found"));
            }

            try
            {
                var curDiscount = _discountManager.GetDiscounts().FirstOrDefault(c => c.Id == id);
                if (curDiscount == null)
                {
                    return BadRequest(new ApiBadRequestResponse("Cannot find gas discount"));
                }
                _mapper.Map(apiDiscount, curDiscount);
                var newDiscount = _discountManager.Update(curDiscount);
                if (newDiscount == null)
                {
                    return BadRequest(new ApiBadRequestResponse("Cannot update gas discount"));
                }

                var newApiDiscount = _mapper.Map<ApiDiscount>(newDiscount);
                return Ok(new ApiOkResponse(newApiDiscount));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/automobiles/gaslogs/discount/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var discount = _discountManager.GetDiscounts().FirstOrDefault(d => d.Id == id);
                if (discount == null)
                {
                    return BadRequest(new ApiBadRequestResponse($"Cannot find discount with id : {id}"));
                }

                discount.IsActive = false;
                var updatedDiscount = _discountManager.Update(discount);
                if (updatedDiscount == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                return Ok(true);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}