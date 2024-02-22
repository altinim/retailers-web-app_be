using DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.DTO {
    public class CompanyDTO {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }

        public static explicit operator CompanyDTO(Company v) => new  CompanyDTO
            {
                CompanyId = v.CompanyId,
                CompanyName = v.CompanyName
            };
        }
    }
