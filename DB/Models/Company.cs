using System.ComponentModel.DataAnnotations;

namespace DB.Models {
    public class Company {
        [Key]
        public Guid CompanyId { get; set; }
        [Required]
        public string CompanyName { get; set; }
    
        public Company() {
            CompanyId = Guid.NewGuid();
        }
    }

}

