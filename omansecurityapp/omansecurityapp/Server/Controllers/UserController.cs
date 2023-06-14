using Microsoft.AspNetCore.Mvc;
using omansecurityapp.Client;
using omansecurityapp.Server.Services;
using omansecurityapp.Shared.Models;

namespace omansecurityapp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        readonly IUser _IUser;

        public UserController(IUser iUser)
        {
            _IUser = iUser;
        }


        [HttpPost("AddNewUser")]
        public async Task<IActionResult> AddNewUser([FromBody] AdminUsers user)
        {
            try
            {
                await _IUser.AddNewUser(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPut("UpdatePassword")]
        public IActionResult UpdatePassword(PasswordUpdateModelForAdmins user)
        {
            try
            {
                _IUser.UpdatePassword(user);
                return Ok("Password updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{UniqueID}")]
        public async Task<AdminUsers> GetUserData(int UniqueID)
        {
            AdminUsers user = _IUser.GetUserData(UniqueID);
            return user;
        }
    }
}
