using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB.Models;

namespace Core.DTO {
    public class AddressDTO {
        public string CompanyAddress { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }


        public static explicit operator Address(AddressDTO addressDTO) => new Address
        {
            CompanyAddress = addressDTO.CompanyAddress,
            City = addressDTO.City,
            Region = addressDTO.Region,
            PostalCode = addressDTO.PostalCode
        };

    }
}