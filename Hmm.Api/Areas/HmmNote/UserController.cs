using AutoMapper;
using DomainEntity.User;
using Hmm.Api.Areas.HmmNote.Models;
using Hmm.Contract;
using Hmm.Utility.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Hmm.Api.Areas.HmmNote
{
    [Route("api/users")]
    public class UserController : Controller
    {
        #region private fields

        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        #endregion private fields

        #region constructor

        public UserController(IUserManager userManager, IMapper mapper)
        {
            Guard.Against<ArgumentNullException>(userManager == null, nameof(userManager));
            Guard.Against<ArgumentNullException>(mapper == null, nameof(mapper));

            _userManager = userManager;
            _mapper = mapper;
        }

        #endregion constructor

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/users
        [HttpPost]
        public IActionResult Post([FromBody] ApiUser user)
        {
            try
            {
                var usr = _mapper.Map<ApiUser, User>(user);
                var newusr = _userManager.Create(usr);

                if (newusr == null)
                {
                    return BadRequest();
                }

                var apinewusr = _mapper.Map<User, ApiUser>(newusr);

                return Ok(apinewusr);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]ApiUser user)
        {
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}