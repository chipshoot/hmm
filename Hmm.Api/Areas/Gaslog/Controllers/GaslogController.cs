using Hmm.Api.Areas.Gaslog.Models;
using Hmm.Contract.GasLogMan;
using Hmm.Utility.Validation;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Hmm.Api.Areas.Gaslog.Controllers
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
        public string Get(int id)
        {
            return "value";
        }

        // POST api/gaslogs
        [HttpPost]
        public void Post([FromBody] ApiGaslog gaslog)
        {
        }

        // PUT api/gaslogs/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string gaslog)
        {
        }

        // DELETE api/gaslogs/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}