using Core.DTO;
using DB.Models;

namespace Core.Interfaces {
    public interface IBrochure {
        string RootPath { get; }
        Task<Brochure> GetBrochureByIdAsync(int id);
        Task AddBrochureAsync(BrochureDTO brochureDto);
        Task<IEnumerable<Brochure>> ReadBrochuresAsync();
        Task<IEnumerable<Brochure>> GetUserCompanyBrochuresAsync();
        List<string> ReadPdfAndExtractImages(string pdfFilePath);
        string ReadThumbnail(string pdfFilePath);
        Task<Address> GetBrochureAddressAsync(int brochureId);
        List<Brochure> GetBrochuresByCompanyId(Guid companyId);
        Task<string> GetCompanyNameByBrochureIdAsync(int brochureId);
    }
}
