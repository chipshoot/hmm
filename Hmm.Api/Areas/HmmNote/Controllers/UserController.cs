using AutoMapper;
using DomainEntity.User;
using Hmm.Api.Areas.HmmNote.Models;
using Hmm.Contract;
using Hmm.Utility.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Hmm.Api.Models;

namespace Hmm.Api.Areas.HmmNote.Controllers
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
        public IActionResult Get(int id)
        {
            var user = _userManager.FindUser(id);
            return Ok(user);
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
        public IActionResult Put(int id, [FromBody]ApiUser user)
        {
            if (user == null || id <= 0)
            {
                return BadRequest(new ApiBadRequestResponse("user information is null or invalid id found"));
            }

            try
            {

                var usr = _mapper.Map<ApiUser, User>(user);
                if (usr == null)
                {
                    return BadRequest();
                }

                var apiNewUser = _userManager.Update(usr);
                var newUser = _mapper.Map<User, ApiUser>(apiNewUser);
                return Ok(newUser);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}