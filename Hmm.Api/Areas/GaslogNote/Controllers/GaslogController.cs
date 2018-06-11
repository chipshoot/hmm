using DomainEntity.Vehicle;
using Hmm.Api.Areas.GaslogNote.Models;
using Hmm.Api.Models;
using Hmm.Api.Models.Validation;
using Hmm.Contract.GasLogMan;
using Hmm.Utility.Validation;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Hmm.Api.Areas.GaslogNote.Controllers
{
    [Route("api/gaslogs")]
    [ValidationModel]
    public class GaslogController : Controller
    {
        private readonly IGasLogManager _gaslogManager;

        public GaslogController(IGasLogManager gaslogManager)
        {
            Guard.Against<ArgumentNullException>(gaslogManager == null, nameof(gaslogManager));

            _gaslogManager = gaslogManager;
        }

        // GET api/gaslogs/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var log = _gaslogManager.GetGasLogById(id);
            var config = ApiDomainEntityConvertHelper.DomainEntity2Api();
            var mapper = config.CreateMapper();
            var apilog = mapper.Map<ApiGasLog>(log);

            return Ok(apilog);
        }

        // POST api/gaslogs
        [HttpPost]
        public IActionResult Post([FromBody] ApiGasLog apiGaslog)
        {
            if (apiGaslog == null)
            {
                return BadRequest(new ApiBadRequestResponse("null gas log found"));
            }

            var config = ApiDomainEntityConvertHelper.Api2DomainEntity();
            var mapper = config.CreateMapper();
            var gaslog = mapper.Map<GasLog>(apiGaslog);
            var result = _gaslogManager.CreateLog(gaslog);

            config = ApiDomainEntityConvertHelper.DomainEntity2Api();
            mapper = config.CreateMapper();
            var newlog = mapper.Map<ApiGasLog>(result);

            return Ok(new ApiOkResponse(newlog));
        }

        // PUT api/gaslogs/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]string gaslog)
        {
            throw new NotImplementedException();
        }

        // DELETE api/gaslogs/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}