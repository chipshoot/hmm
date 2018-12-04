using DomainEntity.Vehicle;
using Hmm.Api.Areas.GasLogNote.Models;
using Hmm.Api.Models;
using Hmm.Api.Models.Validation;
using Hmm.Contract.GasLogMan;
using Hmm.Utility.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Hmm.Api.Areas.GasLogNote.Controllers
{
    [Route("api/automobile/gaslogs")]
    [ValidationModel]
    public class GasLogController : Controller
    {
        private readonly IGasLogManager _gasLogManager;

        public GasLogController(IGasLogManager gasLogManager)
        {
            Guard.Against<ArgumentNullException>(gasLogManager == null, nameof(gasLogManager));

            _gasLogManager = gasLogManager;
        }

        // GET api/automobiles/gaslogs/5
        [HttpGet("{id}")]
        public IActionResult Get(int gasLogId)
        {
            var gasLog = _gasLogManager.FindGasLog(gasLogId);
            var config = ApiDomainEntityConvertHelper.DomainEntity2Api();
            var mapper = config.CreateMapper();
            var apiGasLog = mapper.Map<ApiGasLog>(gasLog);

            return Ok(apiGasLog);
        }

        // POST api/automobiles/gaslogs
        [HttpPost]
        public IActionResult Post(int authorId, [FromBody] ApiGasLogForCreation apiGasLog)
        {
            if (apiGasLog == null)
            {
                return BadRequest(new ApiBadRequestResponse("null gas log found"));
            }

            var config = ApiDomainEntityConvertHelper.Api2DomainEntity();
            var mapper = config.CreateMapper();
            var gasLog = mapper.Map<GasLog>(apiGasLog);
            var result = _gasLogManager.CreateLogForAuthor(authorId, gasLog);
            switch (result)
            {
                case null when _gasLogManager.ProcessResult.MessageList.Contains("Cannot found author with Id"):
                    return NotFound();

                case null:
                    throw new Exception(_gasLogManager.ProcessResult.MessageList.FirstOrDefault());

                default:
                    config = ApiDomainEntityConvertHelper.DomainEntity2Api();
                    mapper = config.CreateMapper();
                    var newGasLog = mapper.Map<ApiGasLog>(result);

                    return Ok(new ApiOkResponse(newGasLog));
            }
        }

        // PUT api/automobiles/gaslogs/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]ApiGasLogForUpdate apiGasLog)
        {
            //if (apiGasLog == null)
            //{
            //    return BadRequest(new ApiBadRequestResponse("null gas log found"));
            //}

            //var config = ApiDomainEntityConvertHelper.Api2DomainEntity();
            //var mapper = config.CreateMapper();
            //var gasLog = mapper.Map<GasLog>(apiGasLog);
            //var result = _gasLogManager.CreateLogForAuthor(authorId, gasLog);
            //switch (result)
            //{
            //    case null when _gasLogManager.ProcessResult.MessageList.Contains("Cannot found author with Id"):
            //        return NotFound();

            //    case null:
            //        throw new Exception(_gasLogManager.ProcessResult.MessageList.FirstOrDefault());

            //    default:
            //        config = ApiDomainEntityConvertHelper.DomainEntity2Api();
            //        mapper = config.CreateMapper();
            //        var newGasLog = mapper.Map<ApiGasLog>(result);

            //        return Ok(new ApiOkResponse(newGasLog));
            //}
            return Ok();
        }

        // DELETE api/automobiles/gaslogs/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}