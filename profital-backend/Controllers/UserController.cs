using Core.DTO;
using Core.Interfaces;
using DB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace profital_backend.Controllers {
    [Route("api/users")]
    [Authorize(Policy = "Admin")]
    public class UserController : ControllerBase {
        private readonly IUser _userService;
        public UserController(IUser userService) {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetCompanysAsync([FromQuery] bool? isApproved, [FromQuery] int? pageNumber, [FromQuery] int? pageSize) {
            try {
                var companys = await _userService.ReadUserAsync(isApproved, pageNumber, pageSize);
                return Ok(companys);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("approve")]
        public async Task<IActionResult> ApproveUser([FromQuery] Guid userId) {
            try {
                await _userService.ApproveUser(userId);
                return Ok("User approved successfully.");
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is InvalidOperationException) {
                return NotFound($"User with ID {userId} not found.");
            }
            catch (Exception ex) {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUserAsync(string email) {
            try {
                bool deleted = await _userService.DeleteUserAsync(email);

                if (deleted) {
                    return Ok("Company deleted successfully");
                }
                else {
                    return NotFound($"Company with ID {email} not found");
                }
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/role")]
        public IActionResult UpdateUserRole(Guid id, [FromBody] RoleDTO roleUpdate) {
            var user = _userService.GetUserById(id);
            if (user == null) {
                return NotFound($"User with ID {id} not found");
            }

            _userService.UpdateUserRole(id, roleUpdate.Role);
            return Ok(new { Message = "Role updated successfully" });
        }
    }
}
