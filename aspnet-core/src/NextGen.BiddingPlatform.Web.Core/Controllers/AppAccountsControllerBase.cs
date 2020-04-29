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
        private readonly IWebHostEnvironment _env;
        public AppAccountsControllerBase(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task<JsonResult> UploadLogo()
        {
            try
            {
                var logoFile = Request.Form.Files.First();
                if(logoFile != null)
                {
                    var fileName = DateTime.Now.Ticks + "_" + logoFile.FileName;
                    var pathLocation = _env.WebRootPath + "/Uploads/AppAccountLogo";
                    
                    CommonFileUpload commonFileUpload = new CommonFileUpload();
                    var fullpath = await commonFileUpload.UploadFileRelativePath(logoFile, pathLocation, fileName);
                    return Json(new AjaxResponse(new { Path = pathLocation + "/" + fileName, Status = true }));
                }
                else
                {
                    return Json(new AjaxResponse(new { Status = true, Path="" }));
                }
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new { Status = false }));
            }
        }
      
       
    }
}