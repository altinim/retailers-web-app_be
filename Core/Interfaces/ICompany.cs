using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO;
using DB.Models;

namespace Core.Interfaces {
    public interface ICompany {
        Task DeleteExpiredBrochuresAsync(Guid companyId);
        Task DeleteAllCompanyBrochuresAsync(Guid companyId);
        Task DeleteBrochureAsync(int brochureId);
        Task<IEnumerable<Address>> GetAddressesByCompanyAsync();
        Task AddAddress(AddressDTO addressDTO);
    }
}
