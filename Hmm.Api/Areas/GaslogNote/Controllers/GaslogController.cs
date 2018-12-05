using AutoMapper;
using DomainEntity.Vehicle;
using Hmm.Api.Areas.GasLogNote.Models;
using Hmm.Api.Models;
using Hmm.Api.Models.Validation;
using Hmm.Contract.Core;
using Hmm.Contract.GasLogMan;
using Hmm.Utility.Currency;
using Hmm.Utility.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hmm.Api.Areas.GasLogNote.Controllers
{
    [Route("api/automobiles/gaslogs")]
    [ValidationModel]
    public class GasLogController : Controller
    {
        private readonly IGasLogManager _gasLogManager;
        private readonly IMapper _mapper;
        private readonly IUserManager _userManager;
        private readonly IAutomobileManager _autoManager;
        private readonly IDiscountManager _discountManager;

        public GasLogController(IGasLogManager gasLogManager, IMapper mapper, IUserManager userManager, IAutomobileManager autoManager, IDiscountManager discountManager)
        {
            Guard.Against<ArgumentNullException>(gasLogManager == null, nameof(gasLogManager));
            Guard.Against<ArgumentNullException>(mapper == null, nameof(mapper));
            Guard.Against<ArgumentNullException>(userManager == null, nameof(userManager));
            Guard.Against<ArgumentNullException>(autoManager == null, nameof(autoManager));
            Guard.Against<ArgumentNullException>(discountManager == null, nameof(discountManager));

            _gasLogManager = gasLogManager;
            _mapper = mapper;
            _userManager = userManager;
            _autoManager = autoManager;
            _discountManager = discountManager;
        }

        // GET api/automobiles/gaslogs/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var gasLog = _gasLogManager.FindGasLog(id);
                if (gasLog == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

                var apiGasLog = _mapper.Map<ApiGasLog>(gasLog);
                return Ok(apiGasLog);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // POST api/automobiles/gaslogs
        [HttpPost]
        public IActionResult Post(int authorId, [FromBody] ApiGasLogForCreation apiGasLog)
        {
            if (apiGasLog == null)
            {
                return BadRequest(new ApiBadRequestResponse("null gas log found"));
            }

            try
            {
                // get author for gas log
                var user = _userManager.GetEntities().FirstOrDefault(u => u.Id == authorId);
                if (user == null)
                {
                    return BadRequest(new ApiBadRequestResponse($"Cannot find user with id {authorId} from data source"));
                }
                var gasLog = _mapper.Map<GasLog>(apiGasLog);
                gasLog.Author = user;

                // get automobile
                var car = _autoManager.GetAutomobiles().FirstOrDefault(c => c.Id == apiGasLog.AutomobileId);
                if (car == null)
                {
                    return BadRequest(new ApiBadRequestResponse($"Cannot find automobile with id {authorId} from data source"));
                }
                gasLog.Car = car;

                // get discount for gas log
                var discounts = new List<GasDiscountInfo>();
                if (apiGasLog.DiscountInfos != null)
                {
                    foreach (var disc in apiGasLog.DiscountInfos)
                    {
                        var discount = _discountManager.GetDiscountById(disc.DiscountId);
                        if (discount == null)
                        {
                            return BadRequest(new ApiBadRequestResponse($"Cannot find discount information for discount with id {authorId} from data source"));
                        }

                        discounts.Add(new GasDiscountInfo
                        {
                            Program = discount,
                            Amount = new Money(disc.Amount)
                        });
                    }
                }
                gasLog.Discounts = discounts;

                var savedGasLog = _gasLogManager.CreateLog(gasLog);
                if (savedGasLog == null)
                {
                    return BadRequest(new ApiBadRequestResponse("Cannot add gas log"));
                }

                var apiSavedGasLog = _mapper.Map<ApiGasLog>(savedGasLog);
                return Ok(new ApiOkResponse(apiSavedGasLog));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/automobiles/gaslogs/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]ApiGasLogForUpdate apiGasLog)
        {
            if (apiGasLog == null)
            {
                return BadRequest(new ApiBadRequestResponse("null gas log found"));
            }

            try
            {
                var gasLog = _gasLogManager.FindGasLog(id);
                if (gasLog == null)
                {
                    return BadRequest(new ApiBadRequestResponse($"Cannot find gas log with id {id}"));
                }

                _mapper.Map(apiGasLog, gasLog);
                var newLog = _gasLogManager.UpdateGasLog(gasLog);
                if (newLog == null)
                {
                    return BadRequest(new ApiBadRequestResponse($"Cannot update gas log with id {id}"));
                }

                var newApiLog = _mapper.Map<ApiGasLog>(newLog);
                return Ok(newApiLog);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/automobiles/gaslogs/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}