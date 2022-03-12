namespace TestOrders.Contracts
{
    public interface IFileService
    {
        public Task<(string error, string fileName, bool saved)> SaveFile(string folder, string fileName, IFormFile file);
    }
}
