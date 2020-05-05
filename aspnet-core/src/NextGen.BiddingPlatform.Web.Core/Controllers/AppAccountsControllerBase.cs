using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetZeroCore.Net;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using CHI.UI.Web.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NextGen.BiddingPlatform.AppAccount;
using NextGen.BiddingPlatform.AppAccount.Dto;
using NextGen.BiddingPlatform.Authorization.Users.Profile.Dto;
using NextGen.BiddingPlatform.Dto;
using NextGen.BiddingPlatform.Storage;
using NextGen.BiddingPlatform.Web.Helpers;

namespace NextGen.BiddingPlatform.Web.Controllers
{
    public abstract class AppAccountsControllerBase : BiddingPlatformControllerBase
    {

        private const int MaxProfilePictureSize = 5242880; //5MB
        private const int MaxThumbnailHeight = 100;
        private const int MaxThumbnailWidth = 100;
        private readonly IWebHostEnvironment _env;
        private readonly IAppAccountAppService _appAccountAppService;
        private const string LogoImagePath = "/Uploads/AppAccountLogo/";
        public AppAccountsControllerBase(IWebHostEnvironment env, IAppAccountAppService appAccountAppService)
        {
            _env = env;
            _appAccountAppService = appAccountAppService;
        }
        public async Task<JsonResult> UploadLogo()
        {
            try
            {
                var logoFile = Request.Form.Files.First();
                var createAppAccountDto = new CreateAppAccountDto();
                var str = Request.Form["createAppAccountDto"];
                createAppAccountDto = JsonConvert.DeserializeObject<CreateAppAccountDto>(str);
                
                if (logoFile != null)
                {
                    byte[] fileBytes;
                    using (var stream = logoFile.OpenReadStream())
                    {
                        fileBytes = stream.GetAllBytes();
                    }

                    var imageFormat = ImageFormatHelper.GetRawImageFormat(fileBytes);
                    if (!imageFormat.IsIn(ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif))
                    {
                        throw new UserFriendlyException(L("File_Invalid_Type_Error"));
                    }

                    var fileName = DateTime.Now.Ticks + "_" + logoFile.FileName;
                    var pathLocation = _env.WebRootPath + LogoImagePath;

                    CommonFileUpload commonFileUpload = new CommonFileUpload();
                    var fullpath = await commonFileUpload.UploadFileRelativePath(logoFile, pathLocation, fileName);
                    var thumbnailImageName = await commonFileUpload.UploadThumbnail(logoFile, pathLocation, fileName, MaxThumbnailHeight, MaxThumbnailWidth);

                    createAppAccountDto.Logo = LogoImagePath + fileName;
                    createAppAccountDto.ThumbnailImage = LogoImagePath + thumbnailImageName;
                }
                else
                {
                    createAppAccountDto.Logo = "";
                    createAppAccountDto.ThumbnailImage = "";
                }
                await _appAccountAppService.Create(createAppAccountDto);
                return Json(new AjaxResponse(new { Status = true }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new { Status = false }));
            }
        }


    }
}