using Core.DTO;
using Core.Interfaces;
using Core.Services;
using DB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace profital_backend.Controllers {
    [ApiController]
    [Route("api/brochures")]
    public class BrochureController : ControllerBase {
        private readonly IBrochure _brochureService;

        public BrochureController(IBrochure brochureService) {
            _brochureService = brochureService ?? throw new ArgumentNullException(nameof(brochureService));
        }
        [HttpPost("AddBrochure")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> AddBrochureAsync([FromForm] BrochureDTO brochure) {
            try {
                await _brochureService.AddBrochureAsync(brochure);
                return Ok(brochure);
            }
            catch (Exception ex) {
                return BadRequest($"Error adding brochure: {ex.Message}\n{ex.StackTrace}");
            }
        }




        [HttpGet("user-company-brochures")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult<IEnumerable<Brochure>>> GetUserCompanyBrochures() {
            try {
                var brochures = await _brochureService.GetUserCompanyBrochuresAsync();
                return Ok(brochures);
            }
            catch (ApplicationException ex) {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brochure>>> GetBrochuresAsync() {
            try {
                var brochures = await _brochureService.ReadBrochuresAsync();
                return Ok(brochures);
            }
            catch (Exception ex) {
                return BadRequest($"Error retrieving brochures: {ex.Message}");
            }
        }

        [HttpGet("images")]
        public async Task<IActionResult> GetImages([FromQuery] int id) {
            try {
                Brochure brochure = await _brochureService.GetBrochureByIdAsync(id);

                if (brochure == null) {
                    return NotFound($"Brochure with ID {id} not found.");
                }

                List<string> extractedImages = _brochureService.ReadPdfAndExtractImages(brochure.Path);
                return Ok(extractedImages);
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error occurred: {ex.Message}");
            }
        }

        [HttpGet("thumbnail")]
        public async Task<IActionResult> GetThumbnail([FromQuery] int id) {
            try {
                Brochure brochure = await _brochureService.GetBrochureByIdAsync(id);

                if (brochure == null) {
                    return NotFound($"Brochure with ID {id} not found.");
                }

                string extractedImage = _brochureService.ReadThumbnail(brochure.Path);

                return Ok(new { thumbnail = extractedImage });
            }
            catch (Exception ex) {
                return StatusCode(500, $"Error occurred: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Brochure>> GetBrochureByIdAsync(int id) {
            try {
                var brochure = await _brochureService.GetBrochureByIdAsync(id);
                if (brochure == null) {
                    return NotFound(); // Return 404 Not Found if the brochure with the specified ID is not found
                }
                return Ok(brochure);
            }
            catch (Exception ex) {
                return BadRequest($"Error retrieving brochure with ID {id}: {ex.Message}");
            }
        }
        [HttpGet("{id}/address")]
        public async Task<ActionResult<Address>> GetBrochureAddress(int id) {
            var address = await _brochureService.GetBrochureAddressAsync(id);

            if (address == null) {
                return NotFound();
            }

            return address;
        }
        [HttpGet("companyBrochures/{companyId}")]
        public ActionResult<IEnumerable<Brochure>> GetByCompanyId(Guid companyId) {
            var brochures = _brochureService.GetBrochuresByCompanyId(companyId);
            if (brochures.Any()) {
                return Ok(brochures);
            }
            else {
                return NotFound($"No brochures found for company with ID {companyId}");
            }
        }
        [HttpGet("companyName")]
        public async Task<ActionResult<string>> GetCompanyNameByBrochureId(int brochureId) {
            try {
                var companyName = await _brochureService.GetCompanyNameByBrochureIdAsync(brochureId);
                return Ok(companyName);
            }
            catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
    }

