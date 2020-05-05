using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

        public async Task<string> UploadThumbnail(IFormFile fileBase, string uploadLocation, string fileName, int height, int width)
        {
            pathString = uploadLocation;

            pathString = pathString.Replace('\\', '/');

            var fileWithOutExt = Path.GetFileNameWithoutExtension(fileName);
            var thumbFileName = fileWithOutExt + "_thmb" + ImageFormat.Jpeg.ToString();

            fullPath = Path.Combine(uploadLocation, thumbFileName);

            fullPath = fullPath.Replace('\\', '/');

            SaveThumbnailFile(fileBase, height, width);

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

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await fileBase.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void SaveThumbnailFile(IFormFile fileBase, int height, int width)
        {
            try
            {
                bool isExists = System.IO.Directory.Exists(pathString);
                if (!isExists)
                    System.IO.Directory.CreateDirectory(pathString);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    Image image = Image.FromStream(fileBase.OpenReadStream());
                    var thumbImage = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);
                    thumbImage.Save(fileStream, ImageFormat.Jpeg);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}