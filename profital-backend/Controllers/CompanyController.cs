using Core.DTO;
using Core.Interfaces;
using Core.Services;
using DB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace profital_backend.Controllers {
    [ApiController]
    [Authorize(Roles = "Manager,Admin")]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase {
        private readonly ICompany _companyService;

        public CompanyController(ICompany companyService) {
            _companyService = companyService;
        }

        [HttpDelete("deleteExpiredBrochures/{companyId}")]
        public async Task<IActionResult> DeleteExpiredBrochures(Guid companyId) {
            try {
                await _companyService.DeleteExpiredBrochuresAsync(companyId);
                return Ok("Expired brochures deleted successfully.");
            }
            catch (Exception ex) {
                return BadRequest($"Error deleting expired brochures: {ex.Message}");
            }
        }

        [HttpGet("GetAddressesByCompany")]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddressesByCompany() {
            try {
                var addresses = await _companyService.GetAddressesByCompanyAsync();
                return Ok(addresses);
            }
            catch (Exception ex) {
                return BadRequest($"Error getting addresses: {ex.Message}\n{ex.StackTrace}");
            }
        }


        [HttpPost("AddAddress")]
        public async Task<IActionResult> AddAddress([FromBody] AddressDTO addressDTO) {
            try {
                await _companyService.AddAddress(addressDTO);
                return Ok("Address added successfully");
            }
            catch (Exception ex) {
                return BadRequest($"Failed to add address: {ex.Message}");
            }

        }

        [HttpDelete("{brochureId}")]
        public async Task<IActionResult> DeleteBrochure(int brochureId) {
            await _companyService.DeleteBrochureAsync(brochureId);
            return NoContent();
        }
    }

}


