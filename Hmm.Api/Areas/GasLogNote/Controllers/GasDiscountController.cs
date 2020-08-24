using AutoMapper;
using Hmm.Api.Models;
using Hmm.Contract.Core;
using Hmm.Contract.VehicleInfoManager;
using Hmm.DomainEntity.Vehicle;
using Hmm.DtoEntity.Api.GasLogNotes;
using Hmm.Utility.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hmm.Api.Areas.GasLogNote.Controllers
{
    [ApiController]
    [Route("api/automobiles/gaslogs/discounts")]
    public class GasDiscountController : Controller
    {
        private readonly IAutoEntityManager<GasDiscount> _discountManager;
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        public GasDiscountController(IAutoEntityManager<GasDiscount> discountManager, IMapper mapper, IUserManager userManager)
        {
            Guard.Against<ArgumentNullException>(discountManager == null, nameof(discountManager));
            Guard.Against<ArgumentNullException>(mapper == null, nameof(mapper));
            Guard.Against<ArgumentNullException>(userManager == null, nameof(userManager));

            _discountManager = discountManager;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET api/automobiles/gaslogs/discounts/1
        [HttpGet("{id}", Name = "GetDiscount")]
        [HttpHead]
        public IActionResult GetDiscountById(int id)
        {
            var discount = _discountManager.GetEntityById(id);
            if (discount == null)
            {
                return NotFound();
            }

            var apiDiscount = _mapper.Map<ApiDiscount>(discount);
            return Ok(apiDiscount);
        }

        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<ApiDiscount>> GetDiscounts()
        {
            var discounts = _mapper.Map<IEnumerable<ApiDiscount>>(_discountManager.GetEntities(null).ToList());
            return Ok(discounts);
        }

        // post api/automobiles/gaslogs/discounts?
        [HttpPost]
        public ActionResult<ApiDiscount> CreateGasDiscount(ApiDiscountForCreate apiDiscount)
        {
            var author = _userManager.GetEntities().FirstOrDefault(u => u.Id == Guid.NewGuid());
            if (author == null)
            {
                return BadRequest(new ApiBadRequestResponse("Cannot find author in data source"));
            }

            var discount = _mapper.Map<GasDiscount>(apiDiscount);
            var newDiscount = _discountManager.Create(discount, author);
            if (newDiscount == null)
            {
                return BadRequest("Cannot create discount");
            }

            var discountToReturn = _mapper.Map<ApiDiscount>(newDiscount);
            return CreatedAtRoute("GetDiscount",
                new { id = newDiscount.Id },
                discountToReturn);
        }

        // PUT api/automobiles/gaslogs/discount/5
        [HttpPut("{id}")]
        public IActionResult UpdateDiscount(int id, ApiDiscountForUpdate apiDiscount)
        {
            var curDiscount = _discountManager.GetEntityById(id);
            if (curDiscount == null)
            {
                return BadRequest(new ApiBadRequestResponse("Cannot find gas discount"));
            }
            _mapper.Map(apiDiscount, curDiscount);
            var newDiscount = _discountManager.Update(curDiscount, null);
            if (newDiscount == null)
            {
                return BadRequest(new ApiBadRequestResponse("Cannot update gas discount"));
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PartialUpdateDiscount(int id, JsonPatchDocument<ApiDiscountForUpdate> patchDocument)
        {
            var discount = _discountManager.GetEntityById(id);
            if (discount == null)
            {
                return NotFound();
            }

            var discountToPatch = _mapper.Map<ApiDiscountForUpdate>(discount);
            patchDocument.ApplyTo(discountToPatch, ModelState);

            if (!TryValidateModel(discountToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(discountToPatch, discountToPatch);
            _discountManager.Update(discount, null);

            return NoContent();
        }

        // DELETE api/automobiles/gaslogs/discount/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var discount = _discountManager.GetEntityById(id);
                if (discount == null)
                {
                    return BadRequest(new ApiBadRequestResponse($"Cannot find discount with id : {id}"));
                }

                discount.IsActive = false;
                var updatedDiscount = _discountManager.Update(discount, null);
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