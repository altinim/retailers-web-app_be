using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Models {
    public class Address {
        public int AddressId { get; set; }
        public string CompanyAddress{ get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        [ForeignKey("CompanyId")]
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }

    }
}
