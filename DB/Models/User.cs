using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models {
    public class User {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        [ForeignKey("CompanyId")]
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        public bool IsApproved { get; set; }
        public User() {
            Role = "Manager";
        }
    }
}

