using System.ComponentModel.DataAnnotations;
using DB.Models;
using Microsoft.AspNetCore.Http;

namespace Core.DTO {
    public class BrochureDTO {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Expiry Date is required")]
        [DateAfter("StartDate", ErrorMessage = "Expiry Date must be after Start Date")]
        public DateTime ExpiryDate { get; set; }

        public IFormFile File { get; set; }

        [Required(ErrorMessage = "AddressId is required")]
        public int AddressId { get; set; }

        public static explicit operator Brochure(BrochureDTO brochureDto) => new Brochure
        {
            Title = brochureDto.Title,
            StartDate = brochureDto.StartDate,
            ExpiryDate = brochureDto.ExpiryDate,
            AddressId = brochureDto.AddressId
        };
    }
}
