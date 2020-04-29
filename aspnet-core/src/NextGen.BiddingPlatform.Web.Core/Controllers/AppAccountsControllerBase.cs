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
        private readonly IAppAccountAppService _appAccountAppService;
        public AppAccountsControllerBase(IAppAccountAppService appAccountAppService)
        {
            _appAccountAppService = appAccountAppService;
        }
        public async Task<JsonResult> SaveWithLogo(CreateAppAccountDto createAppAccountDto)
        {
            try
            {
                var logoFile = Request.Form.Files.First();
                CommonFileUpload commonFileUpload = new CommonFileUpload();
                var fullpath = await commonFileUpload.UploadFileRelativePath(logoFile, "/AppAccounts/Logo", "logo");
                createAppAccountDto.Logo = fullpath;
                await _appAccountAppService.Create(createAppAccountDto);
                return Json(new AjaxResponse(new { status = true }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new { status = false }));
            }
        }
       
    }
}