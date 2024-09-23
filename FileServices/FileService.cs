using EmployMe.Models.Domain;
using Microsoft.AspNetCore.Hosting;

namespace EmployMe.FileUploadService
{
    public class FileService
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public string? UploadFile(string FolderName, IFormFile? newFile)
        {
            if (newFile != null && newFile.Length > 0)
            {
                // Generate a unique file name for the image
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(newFile.FileName);
                // Define the path to save the image in your 'images' folder
                var filePath = Path.Combine(webHostEnvironment.WebRootPath, FolderName, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    newFile.CopyTo(stream);
                }
                return filePath;
            }
            return null;
          
        }

        public void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                    File.Delete(filePath);
            }
        }

        public string? UpdateFile(string FolderName, IFormFile? newFile,string? oldFilePath)
        {
            // Delete the old file from the server's file system
            if (oldFilePath != null && File.Exists(oldFilePath))
            {
                DeleteFile(oldFilePath);
            }

            if(newFile !=null)
            {
                return UploadFile(FolderName, newFile);
            }
            
            return null;
            
        }
    
    }
}
