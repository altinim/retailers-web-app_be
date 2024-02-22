using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models {
    public class Brochure {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Compare(nameof(StartDate), ErrorMessage = "Expiry Date must be on or after Start Date")]
        [Required]
        public DateTime ExpiryDate { get; set; }
        [Required]
        public string Path { get; set; }
        [ForeignKey("CompanyId")]
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        [ForeignKey("AddressId")]
        public int AddressId { get; set; }
        public Address Address { get; set; }

    }
}
