using Microsoft.AspNetCore.Http;

namespace Orders.Core.Contracts
{
    public interface IFileService
    {
        public Task<(string error, string fileName, bool saved)> SaveFile(string folder, string fileName, IFormFile file);
    }
}
