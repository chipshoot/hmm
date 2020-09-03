using AutoMapper;
using Hmm.Api.Models;
using Hmm.Contract.Core;
using Hmm.Contract.VehicleInfoManager;
using Hmm.DomainEntity.User;
using Hmm.DomainEntity.Vehicle;
using Hmm.DtoEntity.Api.GasLogNotes;
using Hmm.Utility.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hmm.Api.Areas.GasLogNote.Controllers
{
    [ApiController]
    [Route("api/automobiles")]
    public class AutomobileController : Controller
    {
        private readonly IAutoEntityManager<Automobile> _automobileManager;
        private readonly IAuthorManager _userManager;
        private readonly IMapper _mapper;

        public AutomobileController(IAutoEntityManager<Automobile> automobileManager, IMapper mapper, IAuthorManager userManager)
        {
            Guard.Against<ArgumentNullException>(automobileManager == null, nameof(automobileManager));
            Guard.Against<ArgumentNullException>(mapper == null, nameof(mapper));
            Guard.Against<ArgumentNullException>(userManager == null, nameof(userManager));

            _automobileManager = automobileManager;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET api/automobiles/5
        [HttpGet("{id}", Name = "GetAutomobile")]
        [HttpHead]
        public IActionResult GetAutomobileById(int id)
        {
            var car = _automobileManager.GetEntityById(id);
            if (car == null)
            {
                return NotFound();
            }

            var apiCar = _mapper.Map<ApiAutomobile>(car);
            return Ok(apiCar);
        }

        // GET api/automobiles/
        [HttpGet]
        public ActionResult<IEnumerable<ApiAutomobile>> GetMobiles()
        {
            var apiCars = _mapper.Map<IEnumerable<ApiAutomobile>>(_automobileManager.GetEntities(null).ToList());
            return Ok(apiCars);
        }

        // POST api/automobiles
        [HttpPost]
        public ActionResult CreateAutomobile(ApiAutomobileForCreate apiCar)
        {
            //var user = _userManager.GetEntities().FirstOrDefault(u => u.Id == authorId);
            //if (user == null)
            //{
            //    return BadRequest();
            //}

            var newApiCars = new List<ApiAutomobile>();
            var car = _mapper.Map<Automobile>(apiCar);
            var newCar = _automobileManager.Create(car, null);

            if (newCar == null || !_automobileManager.ProcessResult.Success)
            {
                throw new Exception(_automobileManager.ProcessResult.GetWholeMessage());
            }

            return Ok();
        }

        // PUT api/automobiles/5
        [HttpPut("{id}")]
        public IActionResult UpdateAutomobile(int id, [FromBody] ApiAutomobile apiCar)
        {
            if (apiCar == null)
            {
                return NotFound();
            }

            var curCar = _automobileManager.GetEntityById(id);
            if (curCar == null)
            {
                return BadRequest(new ApiBadRequestResponse("Cannot find automobile"));
            }
            _mapper.Map(apiCar, curCar);
            var newCar = _automobileManager.Update(curCar, new Author());
            if (newCar == null)
            {
                return BadRequest(new ApiBadRequestResponse("Cannot update automobile"));
            }

            var newApiCar = _mapper.Map<ApiAutomobile>(newCar);
            return NoContent();
        }

        // DELETE api/automobiles/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}