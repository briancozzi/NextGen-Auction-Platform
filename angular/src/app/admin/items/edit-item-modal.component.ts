import { Component, Injector, ViewChild, Output, EventEmitter, ElementRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import {
  ItemServiceProxy,
  CategoryServiceProxy,
  UpdateItemDto,
  AppAccountServiceProxy,
  ListDto

} from '@shared/service-proxies/service-proxies';
import { forkJoin } from "rxjs";
import { AppConsts } from '@shared/AppConsts';
import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'editItemModal',
  templateUrl: './edit-item-modal.component.html'
})
export class EditItemModalComponent extends AppComponentBase {

  @ViewChild('editModal', { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  @ViewChild('inputFile') inputFile: ElementRef;

  logoUploader: FileUploader;
  webHostUrl = AppConsts.remoteServiceBaseUrl;
  item: UpdateItemDto = new UpdateItemDto();
  saving = false;
  active = false;
  dropdowns: any;
  procurementState: ListDto[] = [];
  categoryList = [];
  accountList = [];
  isLogo = false;
  AdditionalFiles: string;;
  images: FileDto[] = [];
  additionalImgs: string[] = [];

  constructor(
    injector: Injector,
    private _itemService: ItemServiceProxy,
    private _categoryService: CategoryServiceProxy,
    private _tokenService: TokenService,
    private _accountService: AppAccountServiceProxy,
    private _httpClient: HttpClient
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.initUploaders();
  }
  createUploader(url: string, success?: (result: any) => void): FileUploader {
    const uploader = new FileUploader({ url: AppConsts.remoteServiceBaseUrl + url });

    uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };

    uploader.onBuildItemForm = (fileItem: any, form: any) => {
      form.append('updateItemDto', JSON.stringify(this.item)); //note comma separating key and value
      form.append('isCreated', false);
      form.append('AdditionalFile', this.AdditionalFiles);
    };

    uploader.onSuccessItem = (item, response, status) => {
      const ajaxResponse = <IAjaxResponse>JSON.parse(response);
      if (ajaxResponse.success) {
        if (success) {
          success(ajaxResponse.result);
        }
      } else {
        this.message.error(ajaxResponse.error.message);
      }
    };

    const uploaderOptions: FileUploaderOptions = {};
    uploaderOptions.authToken = 'Bearer ' + this._tokenService.getToken();
    uploaderOptions.removeAfterUpload = true;
    uploader.setOptions(uploaderOptions);
    return uploader;
  }
  initUploaders(): void {
    this.logoUploader = this.createUploader(
      '/Items/UploadLogo',
      result => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.close();
        this.modalSave.emit(null);
      }
    );
  }
  show(ItemId?: string): void {
    this.active = true;
    this.item = new UpdateItemDto();
    forkJoin([
      this._itemService.getItemById(ItemId),
      this._categoryService.getAllCategory(),
      this._itemService.getDropdowns(),
      this._accountService.getAllAccount()
    ]).subscribe(allResults => {

      this.item = allResults[0];
      this.categoryList = allResults[1].items;
      this.dropdowns = allResults[2];
      this.procurementState = this.dropdowns.procurementStates;
      this.accountList = allResults[3].items;
      this.isLogo = true;
      let additionalImages = this.item.itemImages;
      this.additionalImgs = [];
      for (var i = 0; i < additionalImages.length; i++) {
        let url = this.webHostUrl + additionalImages[i].thumbnail;
        this.additionalImgs.push(url);
      }
      this.modal.show();
    });

  }
  clearLogo(): void {
    this.isLogo = false;
    this.item.mainImageName = "";
  }


  close(): void {
    this.active = false;
    this.modal.hide();
  }
  save(): void {
    debugger;
    let mainImages = this.images.filter(s => s.isMainImg === true);
    if (!this.isLogo) {
      if (mainImages.length === 0) {
        this.notify.error("Please upload main image");
        return;
      }
    }

    this.saving = true;
    var formData = new FormData();


    for (var i = 0; i < mainImages.length; i++) {
      formData.append("mainImage[]", mainImages[i].file);
    }

    let additionalImages = this.images.filter(s => s.isMainImg === false);
    for (var x = 0; x < additionalImages.length; x++) {
      formData.append("additionalImages[]", additionalImages[x].file);
    }
    formData.append("isCreated", "false");
    formData.append('updateItemDto', JSON.stringify(this.item));
    this.saving = true;
    var url = AppConsts.remoteServiceBaseUrl + '/Items/UploadLogo';
    this._httpClient.post(url, formData).subscribe(result => {

      if (result["result"].status === true) {
        this.notify.info(this.l('SavedSuccessfully'));
        this.close();
        this.modalSave.emit(null);
      }
      else {
        this.notify.error(this.l('Error occured!!'));
        this.saving = false;
      }
    });
  }
  updateItem(): void {
    this._itemService.updateItem(this.item)
      .pipe(finalize(() => this.saving = false))
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.close();
        this.modalSave.emit(null);
      });
  }

  getFiles(event): void {
    var files = event.target.files;
    var myArray = [];
    var file = {};
    debugger;
    if (files.length > 6) {
      this.notify.error("You'll be able to select file upto 6.");
      event.target.value = "";
      return;
    }
    else {
      for (var i = 0; i < files.length; i++) {
        file = {
          'lastMod': files[i].lastModified,
          'lastModDate': files[i].lastModifiedDate,
          'name': files[i].name,
          'size': files[i].size,
          'type': files[i].type,
        }

        myArray.push(file)
      }
      this.AdditionalFiles = JSON.stringify(myArray);
    }
  }


  onSelectedFile(args, isMainImg: boolean) {
    let isImage = true;
    const files = args.target.files;

    if (args.target.files.length > 6) {
      this.message.warn("You'll be able to select 6 additional images only.");
      return;
    }


    //TODO - Convert images to base 64 and create Images object and upload to api
    if (args.target.files.length > 0) {
      for (let index = 0; index < files.length; index++) {
        // if (files.item(index).type.match('image.*')) {
        var reader = new FileReader();
        reader.readAsDataURL(files[index]);
        var currFile = files[index];
        var fileSize = Math.round((currFile.size / 1024));
        var fileType = this.getExtension(files[index].name);

        if (fileType.toLowerCase() == "png" ||
          fileType.toLowerCase() == 'jpg' ||
          fileType.toLowerCase() == "jpeg") {
        }
        else {
          isImage = false;
          this.message.warn('File is in invalid format.', 'file type issue!');
          return false;
        }
        if (fileSize >= 3072) {
          isImage = false;
          this.message.warn('File must be less than 3mb.', 'file size issue!');
          return false;
        }

        reader.onload = (_event) => {
          currFile.url = reader.result;
        }
        var fileObj = new FileDto();
        fileObj.file = currFile;
        fileObj.isMainImg = isMainImg;

        if (isMainImg) {
          this.images.splice(0, 0, fileObj);
        }
        else {
          this.images.push(fileObj);
        }
        console.log(this.images);

      }
    }
  }
  getExtension(filename: any) {
    return filename.split('.').pop();
  }
  removeImage(index: number) {
    this.images.splice(index, 1);

  }
}
export class FileDto {
  file: File;
  isMainImg: boolean;
}

