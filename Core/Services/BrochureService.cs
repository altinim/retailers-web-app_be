using Core.DTO;
using Core.Interfaces;
using DB.Data;
using DB.Models;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Core.Services {

    public class BrochureService : IBrochure {

        public string RootPath { get; set; }

        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public BrochureService(AppDbContext context, IHttpContextAccessor httpContextAccessor) {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            RootPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\.."));
        }

        public async Task AddBrochureAsync(BrochureDTO brochureDto) {
            if (brochureDto == null) {
                throw new ArgumentNullException(nameof(brochureDto), "Brochure cannot be null.");
            }

            try {
                string fullPath = null;
                if (brochureDto.File != null && brochureDto.File.Length > 0) {
                    string relativePath = Path.Combine("Brochures", $"{brochureDto.Title.Trim().Replace(" ", "_")}.pdf");
                    fullPath = Path.Combine(RootPath, relativePath);

                    using (var stream = new FileStream(fullPath, FileMode.Create)) {
                        await brochureDto.File.CopyToAsync(stream);
                    }
                }

                var user = _httpContextAccessor.HttpContext.User;
                var userEmail = user.Identity.Name;

                var dbUser = await _context.Users
                    .Include(u => u.Company)
                    .FirstOrDefaultAsync(u => u.Email == userEmail);

                if (dbUser != null) {
                    Brochure brochure = (Brochure)brochureDto;
                    brochure.Path = fullPath;
                    brochure.CompanyId = dbUser.CompanyId;

                    _context.Brochures.Add(brochure);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex) {
                throw new ApplicationException($"Error occurred while adding brochure to the database: {ex.Message}\n{ex.StackTrace}");
            }
        }




        public async Task<IEnumerable<Brochure>> GetUserCompanyBrochuresAsync() {
            try {
                var userEmail = _httpContextAccessor.HttpContext.User.Identity.Name;

                var user = await _context.Users
                    .Include(u => u.Company)
                    .FirstOrDefaultAsync(u => u.Email == userEmail);

                if (user != null) {
                    return await _context.Brochures
                        .Include(b => b.Company)
                        .Where(b => b.CompanyId == user.CompanyId)
                        .AsNoTracking()
                        .ToListAsync();
                }

                return Enumerable.Empty<Brochure>();
            }
            catch (Exception ex) {
                throw new ApplicationException("Error occurred while retrieving brochures from the database.", ex);
            }
        }



        public async Task<IEnumerable<Brochure>> ReadBrochuresAsync() {
            try {
                return await _context.Brochures.AsNoTracking().ToListAsync();
            }
            catch (Exception ex) {
                throw new ApplicationException("Error occurred while retrieving brochures from the database.", ex);
            }
        }




        public async Task<Brochure> GetBrochureByIdAsync(int id) {
            try {
                return await _context.Brochures.FindAsync(id);
            }
            catch (Exception ex) {
                throw new ApplicationException($"Error occurred while retrieving brochure by ID: {ex.Message}", ex);
            }
        }



        public List<string> ReadPdfAndExtractImages(string pdfFilePath) {
            List<string> imageInfos = new List<string>();

            PdfReader pdfReader = new PdfReader(pdfFilePath);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);

            for (int page = 1 ; page <= pdfDoc.GetNumberOfPages() ; page++) {
                var strategy = new ImageExtractionStrategy();
                new PdfCanvasProcessor(strategy).ProcessPageContent(pdfDoc.GetPage(page));

                var images = strategy.GetImages();

                imageInfos.AddRange(images);
            }

            pdfDoc.Close();
            pdfReader.Close();

            return imageInfos;
        }
        public string ReadThumbnail(string pdfFilePath) {
            try {
                PdfReader pdfReader = new PdfReader(pdfFilePath);
                PdfDocument pdfDoc = new PdfDocument(pdfReader);

                int firstPage = 1;

                var strategy = new ImageExtractionStrategy();
                new PdfCanvasProcessor(strategy).ProcessPageContent(pdfDoc.GetPage(firstPage));

                var images = strategy.GetImages();

                pdfDoc.Close();
                pdfReader.Close();

                if (images.Count > 0) {
                    return images.First();
                }
                else {
                    throw new InvalidOperationException("No valid images found in the PDF.");
                }
            }
            catch (Exception ex) {
                throw new ApplicationException($"Error occurred while extracting the first image: {ex.Message}", ex);
            }
        }
        public async Task<Address> GetBrochureAddressAsync(int brochureId) {
            return await _context.Brochures
                .Include(b => b.Address)
                .Where(b => b.Id == brochureId)
                .Select(b => b.Address)
                .SingleOrDefaultAsync();
        }
        public List<Brochure> GetBrochuresByCompanyId(Guid companyId) {
            return _context.Brochures.Where(b => b.CompanyId == companyId).ToList();
        }

        public async Task<string> GetCompanyNameByBrochureIdAsync(int brochureId) {
            try {
                var companyName = await _context.Brochures
                    .Where(b => b.Id == brochureId)
                    .Select(b => b.Company.CompanyName)
                    .FirstOrDefaultAsync();

                if (companyName == null) {
                    throw new InvalidOperationException("Company name not found for the given brochure ID.");
                }

                return companyName;
            }
            catch (Exception ex) {
                throw new ApplicationException($"Error fetching company name for brochure ID {brochureId}", ex);
            }
        }

    }
}




