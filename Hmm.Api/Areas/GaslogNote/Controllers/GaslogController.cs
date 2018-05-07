using DomainEntity.Vehicle;
using Hmm.Api.Areas.GaslogNote.Models;
using Hmm.Api.Models;
using Hmm.Api.Models.Validation;
using Hmm.Contract;
using Hmm.Utility.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using Hmm.Api.Areas.HmmNote.Models;

namespace Hmm.Api.Areas.GaslogNote.Controllers
{
    [Route("api/gaslogs")]
    [ValidationModel]
    public class GaslogController : Controller
    {
        private readonly IHmmNoteManager<GasLog> _gaslogManager;

        public GaslogController(IHmmNoteManager<GasLog> gaslogManager)
        {
            Guard.Against<ArgumentNullException>(gaslogManager == null, nameof(gaslogManager));

            _gaslogManager = gaslogManager;
        }

        // GET api/gaslogs/5
        [HttpGet("{id}")]
        public ApiNote Get(int id)
        {
            var log = _gaslogManager.GetNoteById(id);
            var config = ApiDomainEntityConvertHelper.DomainEntity2Api();
            var mapper = config.CreateMapper();
            var apilog = mapper.Map<ApiNote>(log);

            return apilog;
        }

        // POST api/gaslogs
        [HttpPost]
        public IActionResult Post([FromBody] ApiNote apiGaslog)
        {
            if (apiGaslog == null)
            {
                return BadRequest(new ApiBadRequestResponse("null gas log found"));
            }

            var config = ApiDomainEntityConvertHelper.Api2DomainEntity();
            var mapper = config.CreateMapper();
            var gaslog = mapper.Map<GasLog>(apiGaslog);
            var result = _gaslogManager.Create(gaslog);

            config = ApiDomainEntityConvertHelper.DomainEntity2Api();
            mapper = config.CreateMapper();
            var newlog = mapper.Map<ApiNote>(result);

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
        public void Delete(int id)
        {
        }
    }
}