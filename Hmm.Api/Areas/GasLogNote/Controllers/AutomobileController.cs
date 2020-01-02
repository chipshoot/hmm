//using AutoMapper;
//using Hmm.Api.Areas.GasLogNote.Models;
//using Hmm.Api.Models;
//using Hmm.Contract.Core;
//using Hmm.Utility.Validation;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Hmm.Contract.VehicleInfoManager;

//namespace Hmm.Api.Areas.GasLogNote.Controllers
//{
//    [Route("api/automobiles")]
//    public class AutomobileController : Controller
//    {
//        private readonly IAutomobileManager _automobileManager;
//        private readonly IMapper _mapper;
//        private readonly IUserManager _userManager;

//        public AutomobileController(IAutomobileManager automobileManager, IMapper mapper, IUserManager userManager)
//        {
//            Guard.Against<ArgumentNullException>(automobileManager == null, nameof(automobileManager));
//            Guard.Against<ArgumentNullException>(mapper == null, nameof(mapper));
//            Guard.Against<ArgumentNullException>(userManager == null, nameof(userManager));

//            _automobileManager = automobileManager;
//            _mapper = mapper;
//            _userManager = userManager;
//        }

//        // GET api/automobiles/
//        [HttpGet]
//        public IActionResult GetMobilesForAuthor(int authorId)
//        {
//            try
//            {
//                var cars = _automobileManager.GetAutomobiles().Where(c => c.Author.Id == authorId).ToList();

//                if (!cars.Any())
//                {
//                    return StatusCode(StatusCodes.Status404NotFound);
//                }

//                var apiCars = new List<ApiAutomobile>();
//                foreach (var car in cars)
//                {
//                    var apiCar = _mapper.Map<ApiAutomobile>(car);
//                    apiCars.Add(apiCar);
//                }

//                return Ok(apiCars);
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//        }

//        // GET api/automobiles/5
//        [HttpGet("{id}")]
//        public IActionResult Get(int id)
//        {
//            try
//            {
//                var car = _automobileManager.GetAutomobiles().FirstOrDefault(c => c.Id == id);

//                if (car == null)
//                {
//                    return StatusCode(StatusCodes.Status404NotFound);
//                }

//                var apiCar = _mapper.Map<ApiAutomobile>(car);
//                return Ok(apiCar);
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//        }

//        // POST api/automobiles
//        [HttpPost]
//        public IActionResult Post(int authorId, [FromBody] ApiAutomobileForCreate[] apiCars)
//        {
//            if (apiCars == null || !apiCars.Any())
//            {
//                return BadRequest(new ApiBadRequestResponse("null or empty automobile list found"));
//            }

//            try
//            {
//                var user = _userManager.GetEntities().FirstOrDefault(u => u.Id == authorId);
//                if (user == null)
//                {
//                    return BadRequest(new ApiBadRequestResponse($"Cannot find user with id : {authorId}"));
//                }

//                var newApiCars = new List<ApiAutomobile>();
//                foreach (var apiCar in apiCars)
//                {
//                    var car = _mapper.Map<Automobile>(apiCar);
//                    car.Author = user;
//                    var newCar = _automobileManager.Create(car);

//                    if (newCar == null)
//                    {
//                        return BadRequest(new ApiBadRequestResponse("Cannot create automobile"));
//                    }

//                    var apiNewCar = _mapper.Map<ApiAutomobile>(newCar);
//                    newApiCars.Add(apiNewCar);
//                }
//                return Ok(newApiCars);
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//        }

//        // PUT api/automobiles/5
//        [HttpPut("{id}")]
//        public IActionResult Put(int id, [FromBody]ApiAutomobile apiCar)
//        {
//            if (apiCar == null)
//            {
//                return BadRequest(new ApiBadRequestResponse("null automobile found"));
//            }

//            try
//            {
//                var curCar = _automobileManager.GetAutomobiles().FirstOrDefault(c => c.Id == id);
//                if (curCar == null)
//                {
//                    return BadRequest(new ApiBadRequestResponse("Cannot find automobile"));
//                }
//                _mapper.Map(apiCar, curCar);
//                var newCar = _automobileManager.Update(curCar);
//                if (newCar == null)
//                {
//                    return BadRequest(new ApiBadRequestResponse("Cannot update automobile"));
//                }

//                var newApiCar = _mapper.Map<ApiAutomobile>(newCar);
//                return Ok(new ApiOkResponse(newApiCar));
//            }
//            catch (Exception)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError);
//            }
//        }

//        // DELETE api/automobiles/5
//        [HttpDelete("{id}")]
//        public IActionResult Delete(int id)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}