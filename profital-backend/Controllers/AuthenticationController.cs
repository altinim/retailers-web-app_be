using Core.DTO;
using Core.Exceptions;
using Core.Interfaces;
using DB.Models;
using Microsoft.AspNetCore.Mvc;

namespace profital_backend.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : Controller {
        private readonly IUser _userServices;
        public AuthenticationController(IUser userService) {
            _userServices = userService;

        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpDTO user) {
            try {
                var result = await _userServices.SignUp(user);

                return Created("", result);
            }
            catch (DuplicateEmailException ex) {
                return StatusCode(400, ex.Message);

            }

        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(LoginDTO user) {
            try {
                var result = await _userServices.SignIn(user);
                return Created("", result);

            }
            catch (InvalidUsernamePasswordException e) {
                return StatusCode(401, e.Message);
            }

        }
    }
}
