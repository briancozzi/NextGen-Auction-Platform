﻿using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using CHI.UI.Web.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NextGen.BiddingPlatform.Items;
using NextGen.BiddingPlatform.Items.Dto;
using NextGen.BiddingPlatform.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace NextGen.BiddingPlatform.Web.Controllers
{
    public abstract class ItemsControllerBase : BiddingPlatformControllerBase
    {
        private const int MaxProfilePictureSize = 5242880; //5MB
        private const int MaxThumbnailHeight = 100;
        private const int MaxThumbnailWidth = 100;
        private readonly IWebHostEnvironment _env;
        private readonly IItemAppService _itemService;
        private const string LogoImagePath = "/Uploads/ItemImages/";
        public ItemsControllerBase(IItemAppService itemService, IWebHostEnvironment env)
        {
            _itemService = itemService;
            _env = env;
        }

        public async Task<JsonResult> AddItem()
        {
            try
            {
                var isCreated = Convert.ToBoolean(Request.Form["isCreated"]);

                var AdditionalImage = Request.Form["AdditionalFile"];
                if (isCreated)
                    await CreateItem();
                else
                    await UpdateAccount();

                return Json(new AjaxResponse(new { Status = true }));
            }
            catch (Exception ex)
            {
                return Json(new AjaxResponse(new { Status = false }));
            }
        }
        public async Task CreateItem()
        {
            var formData = Request.Form["itemDto"];
            ItemDto itemDto = JsonConvert.DeserializeObject<ItemDto>(formData);
            if (itemDto == null)
                throw new Exception("Posted data is null. Please try again.");


            var uploadedImages = await UploadImages(false);

            itemDto.MainImageName = uploadedImages.MainImageName;
            itemDto.ThumbnailImage = uploadedImages.ThumbnailImage;
            foreach (var item in uploadedImages.AdditionImages)
            {
                itemDto.ItemImages.Add(new ItemGalleryDto
                {
                    ImageName = item.Logo,
                    Thumbnail = item.ThumbnailImage,
                });
            }

            await _itemService.CreateItem(itemDto);
        }
        public async Task UpdateAccount()
        {
            var formData = Request.Form["updateItemDto"];
            UpdateItemDto updateItemDto = JsonConvert.DeserializeObject<UpdateItemDto>(formData);
            if (updateItemDto == null)
                throw new Exception("Posted data is null. Please try again.");

            var uploadedFiles = Request.Form.Files.Count;
            if (uploadedFiles > 0)
            {
                var isMainImgUploaded = string.IsNullOrEmpty(updateItemDto.MainImageName) ? false : true;

                var uploadedImages = await UploadImages(isMainImgUploaded);
                if (!isMainImgUploaded)
                {
                    updateItemDto.MainImageName = uploadedImages.MainImageName;
                    updateItemDto.ThumbnailImage = uploadedImages.ThumbnailImage;
                }

                foreach (var item in uploadedImages.AdditionImages)
                {
                    updateItemDto.ItemImages.Add(new ItemGalleryDto
                    {
                        ImageName = item.Logo,
                        Thumbnail = item.ThumbnailImage,
                    });
                }
            }

            await _itemService.UpdateItem(updateItemDto);
        }
        public async Task<ItemImages> UploadImages(bool isMainImgUploaded)
        {
            ItemImages images = new ItemImages();

            var files = Request.Form.Files;
            if (!isMainImgUploaded && files.Count > 0)
            {
                var mainFile = files.First();
                if (mainFile != null)
                {
                    var image = await Upload(mainFile);
                    images.MainImageName = image.Logo;
                    images.ThumbnailImage = image.ThumbnailImage;
                }
            }

            var skipCount = isMainImgUploaded ? 0 : 1;

            var additionalFiles = files.Skip(skipCount);
            foreach (var logoFile in additionalFiles)
            {
                if (logoFile != null)
                {
                    var additionalImage = await Upload(logoFile);
                    images.AdditionImages.Add(additionalImage);
                }
            }
            return images;
        }
        private async Task<Logos> Upload(IFormFile file)
        {
            Logos logos = new Logos();
            byte[] fileBytes;
            using (var stream = file.OpenReadStream())
            {
                fileBytes = stream.GetAllBytes();
            }

            var imageFormat = ImageFormatHelper.GetRawImageFormat(fileBytes);
            if (!imageFormat.IsIn(ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif))
            {
                throw new UserFriendlyException(L("File_Invalid_Type_Error"));
            }

            var fileName = DateTime.Now.Ticks + "_" + file.FileName;
            var pathLocation = _env.WebRootPath + LogoImagePath;

            CommonFileUpload commonFileUpload = new CommonFileUpload();
            await commonFileUpload.UploadFileRelativePath(file, pathLocation, fileName);
            var thumbnailImageName = await commonFileUpload.UploadThumbnail(file, pathLocation, fileName, MaxThumbnailHeight, MaxThumbnailWidth);

            logos.Logo = LogoImagePath + fileName;
            logos.ThumbnailImage = LogoImagePath + thumbnailImageName;
            return logos;
        }
    }

    public class ItemImages
    {
        public ItemImages()
        {
            AdditionImages = new List<Logos>();
        }
        public string MainImageName { get; set; }
        public string ThumbnailImage { get; set; }
        public List<Logos> AdditionImages { get; set; }
    }
}
