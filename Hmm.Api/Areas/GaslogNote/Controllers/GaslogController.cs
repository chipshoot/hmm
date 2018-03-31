using Hmm.Api.Areas.GaslogNote.Models;
using Hmm.Api.Models;
using Hmm.Contract.GasLogMan;
using Hmm.Utility.Validation;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Hmm.Api.Areas.GaslogNote.Controllers
{
    [Route("api/gaslogs")]
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
        public ApiGaslog Get(int id)
        {
            return new ApiGaslog();
        }

        // POST api/gaslogs
        [HttpPost]
        public IActionResult Post([FromBody] ApiGaslog gaslog)
        {
            if (gaslog == null)
            {
                return BadRequest(new ApiBadRequestResponse("null gas log found"));
            }

            var config = ApiDomainEntityConvertHelper.Api2DomainEntity();
            var log = config.CreateMapper();

            return Ok(new ApiOkResponse(log));
        }

        // PUT api/gaslogs/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]string gaslog)
        {
            throw new NotImplementedException();
        }

        // DELETE api/gaslogs/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}