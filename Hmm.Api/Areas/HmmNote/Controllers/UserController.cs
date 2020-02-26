using AutoMapper;
using DomainEntity.User;
using Hmm.Api.Areas.HmmNote.Models;
using Hmm.Api.Models;
using Hmm.Contract.Core;
using Hmm.Utility.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Hmm.Utility.Misc;

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
        public IActionResult Get(Guid id)
        {
            var user = _userManager.GetEntities().FirstOrDefault(u => u.Id == id);
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
                usr.IsActivated = true;
                var newUser = _userManager.Create(usr);

                if (newUser == null)
                {
                    return BadRequest(new ApiBadRequestResponse("null user found"));
                }

                var apiNewUser = _mapper.Map<User, ApiUser>(newUser);

                return Ok(apiNewUser);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // todo: add reset password method for user account
        [HttpPost("{id}/password")]
        public IActionResult ResetPassword(Guid userId, [FromBody] string newPassword)
        {
            try
            {
                var successful = _userManager.ResetPassword(userId, newPassword);

                if (!successful)
                {
                    return BadRequest();
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody]ApiUserForUpdate user)
        {
            if (user == null || id != Guid.Empty)
            {
                return BadRequest(new ApiBadRequestResponse("user information is null or invalid id found"));
            }

            try
            {
                var curUsr = _userManager.GetEntities().FirstOrDefault(u => u.Id == id);
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
        public IActionResult Patch(Guid id, JsonPatchDocument<ApiUserForUpdate> patchDoc)
        {
            if (patchDoc == null || id != Guid.Empty)
            {
                return BadRequest(new ApiBadRequestResponse("Patch information is null or invalid id found"));
            }

            try
            {
                var curUsr = _userManager.GetEntities().FirstOrDefault(u => u.Id == id);
                if (curUsr == null)
                {
                    return NotFound();
                }

                var user2Update = _mapper.Map<ApiUserForUpdate>(curUsr);
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
        public ActionResult Delete(Guid id)
        {
            _userManager.DeActivate(id);
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