using AutoMapper;
using DomainEntity.User;
using Hmm.Api.Areas.HmmNote.Models;
using Hmm.Api.Models;
using Hmm.Utility.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using Hmm.Contract.Core;

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
            var ret = _mapper.Map<User, ApiUser>(user);
            return Ok(ret);
        }

        // POST api/users
        [HttpPost]
        public IActionResult Post([FromBody] ApiUserForCreate user)
        {
            try
            {
                var usr = _mapper.Map<ApiUserForCreate, User>(user);
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
                var curUsr = _userManager.FindUser(id);
                if (curUsr == null)
                {
                    return NotFound();
                }

                curUsr = _mapper.Map(user, curUsr);
                var apiNewUser = _userManager.Update(curUsr);
                if (apiNewUser == null)
                {
                    return BadRequest(_userManager.ProcessResult.MessageList);
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PATCH api/users/5
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromBody] JsonPatchDocument<ApiUser> patchDoc)
        {
            if (patchDoc == null || id <= 0)
            {
                return BadRequest(new ApiBadRequestResponse("Patch information is null or invalid id found"));
            }

            try
            {
                var curUsr = _userManager.FindUser(id);
                if (curUsr == null)
                {
                    return NotFound();
                }

                var user2Update = _mapper.Map<ApiUser>(curUsr);
                patchDoc.ApplyTo(user2Update);
                _mapper.Map(user2Update, curUsr);

                var newUser = _userManager.Update(curUsr);
                if (newUser == null)
                {
                    return BadRequest(_userManager.ProcessResult.MessageList);
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _userManager.Delete(id);
            if (_userManager.ProcessResult.Success)
            {
                return NoContent();
            }

            if (_userManager.ProcessResult.MessageList.Contains($"Cannot find user with id : {id}"))
            {
                return NotFound();
            }

            throw new Exception($"Deleting user {id} failed on saving");
        }
    }
}