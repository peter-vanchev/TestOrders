using TestOrders.Contracts;

namespace TestOrders.Services
{
    public class FileService : IFileService
    {
        public async Task<(string error, string fileName, bool saved)> SaveFile(string folder, string fileName, IFormFile file)
        {

            string error = null;
            var saved = false;

            string folderName = Path.GetFullPath(@"./wwwroot/Image");
            var pathString = Path.Combine(folderName, folder);
                                                                                        
            char[] separators = new char[] { ' ', ';', ',', '"' };
            string[] temp = fileName.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var name = String.Join("", temp);             
            var fileType = Path.GetExtension(file.FileName);

            fileName = (name + fileType);
            pathString = Path.Combine(pathString, fileName);

            try
            {
                using (var stream = new FileStream(pathString, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                saved = true;
            }
            catch (Exception)
            {
                error = "Cant save file";
                return (error, fileName, saved);
            }

            return (error, fileName, saved);
        }
    }
}
