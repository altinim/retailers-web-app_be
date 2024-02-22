
using System.ComponentModel.DataAnnotations;
using DB.Models;

namespace Core.DTO {
    public class SignUpDTO {

        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6, ErrorMessage = "Please enter at least 6 characters")]
        public string Password { get; set; }
        [Required]
        public string CompanyName { get; set; }



        public static explicit operator User(SignUpDTO signUpDto) {
            var companyId = Guid.NewGuid(); 

            return new User
            {
                Name = signUpDto.Name,
                Email = signUpDto.Email,
                Password = signUpDto.Password,
                Company = new Company
                {
                    CompanyId = companyId,
                    CompanyName = signUpDto.CompanyName
                },
            };
        }
    }
}
