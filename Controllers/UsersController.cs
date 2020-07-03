using webapi.Models;
using webapi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers
{
    public class UsersController: ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _userService.Authenticate(model);

            if (user == null)
                return BadRequest(new { message = "Usuario o Contraseña Incorrecta" });


            return Ok(user);
        }

        
    }
}