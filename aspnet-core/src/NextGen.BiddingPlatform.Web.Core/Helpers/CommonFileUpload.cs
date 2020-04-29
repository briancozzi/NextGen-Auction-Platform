using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CHI.UI.Web.Helper
{
    public class CommonFileUpload
    {
        bool isCdn = false;
        string cdnName = "";
        string userName = "";
        string password = "";

        string pathString = "";
        string newPathString = "";
        
        string fullPath { get; set; }
        string newFullPath { get; set; }
        public async Task<string> UploadFileRelativePath(IFormFile fileBase, string uploadLocation, string fileName)
        {
            pathString = uploadLocation;

            pathString = pathString.Replace('\\', '/');

            fullPath = Path.Combine(uploadLocation, fileName);

            fullPath = fullPath.Replace('\\', '/');
            
            await SaveFile(fileBase);

            return fullPath;
        }


        public bool FileExists(string filePath)
        {
            try
            {
                var fileExists = File.Exists(filePath);
                return fileExists;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private async Task SaveFile(IFormFile fileBase)
        {
            try
            {
                bool isExists = System.IO.Directory.Exists(pathString);
                if (!isExists)
                    System.IO.Directory.CreateDirectory(pathString);
                using(var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await fileBase.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}