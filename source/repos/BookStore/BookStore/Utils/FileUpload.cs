using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using Microsoft.AspNetCore.Hosting;

namespace BookStore.Utils
{
    public class FileUpload
    {
        public static string ProcessUploadedFile(IFormFile photo, IWebHostEnvironment hostingEnvironment,
           string folder)
        {
            string uniqueFileName = null;
            if (photo != null)
            {
                string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, folder);
                uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
