
using System;
using System.Linq;
using System.Threading.Tasks;
using Core.DTO;
using Core.Interfaces;
using DB.Data;
using DB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static Core.Services.CompanyService;

namespace Core.Services {
    public class CompanyService : ICompany {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyService(AppDbContext context, IHttpContextAccessor httpContextAccessor) {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task DeleteAllCompanyBrochuresAsync(Guid companyId) {
            throw new NotImplementedException();
        }

        public async Task DeleteExpiredBrochuresAsync(Guid companyId) {
            try {
                var brochuresToDelete = await _context.Brochures
                    .Where(b => b.CompanyId == companyId && b.ExpiryDate < DateTime.Now)
                    .ToListAsync();

                _context.Brochures.RemoveRange(brochuresToDelete);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                throw new ApplicationException($"Error occurred while deleting expired brochures: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Address>> GetAddressesByCompanyAsync() {


            var user = _httpContextAccessor.HttpContext.User;

            var userEmail = user.Identity.Name;

            var dbUser = await _context.Users
                .Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (dbUser != null) {
                var companyId = dbUser.CompanyId;

                return await _context.Addresses
                    .Where(address => address.CompanyId == companyId)
                    .ToListAsync();
            }

            return Enumerable.Empty<Address>();
        }

        public async Task AddAddress(AddressDTO addressDTO) {
            try {
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated) {
                    var userEmail = _httpContextAccessor.HttpContext.User.Identity.Name;

                    var dbUser = await _context.Users
                        .Include(u => u.Company)
                        .FirstOrDefaultAsync(u => u.Email == userEmail);

                    if (dbUser != null) {
                        Address address = (Address)addressDTO;
                        address.CompanyId = dbUser.CompanyId;

                        _context.Addresses.Add(address);
                        await _context.SaveChangesAsync();
                    }
                    else {
                        throw new ApplicationException("User not found.");
                    }
                }
                else {
                    throw new ApplicationException("User is not authenticated.");
                }
            }
            catch (DbUpdateException ex) {
                throw new ApplicationException($"Error occurred while adding address to the database: {ex.Message}\n{ex.StackTrace}");
            }
        }

            public async Task DeleteBrochureAsync(int brochureId) {
                var brochure = await _context.Brochures.FindAsync(brochureId);

                if (brochure != null) {
                    _context.Brochures.Remove(brochure);
                    await _context.SaveChangesAsync();
                }
            }



        }

}

