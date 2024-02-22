using System.ComponentModel.DataAnnotations;

namespace DB.Models {
    public class LoginDTO {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Please enter at least 6 characters")]
        public string Password { get; set; }

    }
}
