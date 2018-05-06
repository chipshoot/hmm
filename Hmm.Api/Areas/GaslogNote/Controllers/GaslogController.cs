using DomainEntity.Vehicle;
using Hmm.Api.Areas.GaslogNote.Models;
using Hmm.Api.Models;
using Hmm.Contract;
using Hmm.Utility.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Hmm.Api.Areas.GaslogNote.Controllers
{
    [Route("api/gaslogs")]
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
        public ApiGaslog Get(int id)
        {
            var log = _gaslogManager.GetNoteById(id);
            var config = ApiDomainEntityConvertHelper.DomainEntity2Api();
            var mapper = config.CreateMapper();
            var apilog = mapper.Map<ApiGaslog>(log);

            return apilog;
        }

        // POST api/gaslogs
        [HttpPost]
        public IActionResult Post([FromBody] ApiGaslog apiGaslog)
        {
            if (apiGaslog == null)
            {
                return BadRequest(new ApiBadRequestResponse("null gas log found"));
            }

            var config = ApiDomainEntityConvertHelper.Api2DomainEntity();
            var mapper = config.CreateMapper();
            var gaslog = mapper.Map<GasLog>(apiGaslog);
            var result = _gaslogManager.Create(gaslog);

            if (!_gaslogManager.ErrorMessage.MessageList.Any())
            {
                config = ApiDomainEntityConvertHelper.DomainEntity2Api();
                mapper = config.CreateMapper();
                var newlog = mapper.Map<ApiGaslog>(result);

                return Ok(new ApiOkResponse(newlog));
            }
            else
            {
                return Ok;
            }
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