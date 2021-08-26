import { Component, EventEmitter, Injector, Output, ViewChild, OnInit, ElementRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { catchError, finalize } from 'rxjs/operators';
import {
  ItemServiceProxy, ItemDto, CategoryServiceProxy, AppAccountServiceProxy, ListDto, Dropdowns
} from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { FileUploader, FileUploaderOptions, FileLikeObject } from 'ng2-file-upload';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { forkJoin } from "rxjs";
import { Dropdown } from 'primeng/dropdown';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'createItemModal',
  templateUrl: './create-item-modal.component.html'
})

export class CreateItemModalComponent extends AppComponentBase implements OnInit {
  @ViewChild('inputFile') inputFile: ElementRef;
  @ViewChild('input1') input1: ElementRef;
  @ViewChild('input2') input2: ElementRef;
  @ViewChild('input3') input3: ElementRef;
  @ViewChild('input4') input4: ElementRef;
  @ViewChild('input5') input5: ElementRef;
  @ViewChild('input6') input6: ElementRef;
  @ViewChild('createModal', { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

  logoUploader: FileUploader;
  remoteServiceBaseUrl = AppConsts.remoteServiceBaseUrl;
  active = false;
  saving = false;
  item: ItemDto = new ItemDto();
  uploadedFiles: any[] = [];
  logo: string;
  isSelected = true;
  dropdowns: Dropdowns = new Dropdowns();
  AdditionalFiles: string;
  fileList: FileList;
  categoryList = [];
  accountList = [];
  images: FileDto[] = [];
  isMainFileAdded: boolean = false;


  constructor(
    injector: Injector,
    private _itemService: ItemServiceProxy,
    private _tokenService: TokenService,
    private _categoryService: CategoryServiceProxy,
    private _accountService: AppAccountServiceProxy,
    private _httpClient: HttpClient
  ) {
    super(injector);
  }

  ngOnInit(): void {
    //this.initUploaders();
  }
  clearLogo(): void {
    this.inputFile.nativeElement.value = '';
    this.images = this.images.filter(s => s.isMainImg === false);
    this.isMainFileAdded = false;
  }

  clearAddImg(inputType: number): void {
    if (inputType === 1) {
      this.input1.nativeElement.value = '';
    }
    else if (inputType === 2) {
      this.input2.nativeElement.value = '';
    }
    else if (inputType === 3) {
      this.input3.nativeElement.value = '';
    }
    else if (inputType === 4) {
      this.input4.nativeElement.value = '';
    }
    else if (inputType === 5) {
      this.input5.nativeElement.value = '';
    }
    else if (inputType === 6) {
      this.input6.nativeElement.value = '';
    }
  }

  createUploader(url: string, success?: (result: any) => void): FileUploader {

    const uploader = new FileUploader({
      url: AppConsts.remoteServiceBaseUrl + url,

    });

    uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };
    uploader.onBuildItemForm = (fileItem: any, form: any) => {
      form.append('itemDto', JSON.stringify(this.item));
      form.append('isCreated', true);
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
        
        if (result.status) {
          this.notify.info(this.l('SavedSuccessfully'));
          this.close();
          this.modalSave.emit(null);
        }
        else {
          this.notify.error(this.l('Error occured!!'));
          this.saving = false;
        }
      }
    );
  }
  show() {
    this.active = true;
    this.init();
    this._itemService.getNextItemNumber().subscribe(result => {
      this.item = new ItemDto();
      this.item.itemNumber = result;
      this.item.isHide = true;

      this.modal.show();
    });

  }
  init(): void {
    forkJoin([
      this._itemService.getDropdowns(),
      this._categoryService.getAllCategory(),
      this._accountService.getAllAccount()
    ]).subscribe(result => {
      this.dropdowns = result[0]
      this.categoryList = result[1].items;
      this.accountList = result[2].items;
    });
  }
  close(): void {
    this.active = false;
    this.modal.hide();
  }
  save(): void {
    let mainImages = this.images.filter(s => s.isMainImg === true);
    // if (mainImages.length === 0) {
    //   this.notify.error("Please upload main image");
    //   return;
    // }
    this.saving = true;
    var formData = new FormData();


    for (var i = 0; i < mainImages.length; i++) {
      formData.append("mainImage[]", mainImages[i].file);
    }

    let additionalImages = this.images.filter(s => s.isMainImg === false);
    for (var x = 0; x < additionalImages.length; x++) {
      formData.append("additionalImages[]", additionalImages[x].file);
    }
    formData.append("isCreated", "true");
    formData.append('itemDto', JSON.stringify(this.item));
    this.saving = true;
    var url = AppConsts.remoteServiceBaseUrl + '/Items/AddItem';
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
  CreateItem(): void {

    this._itemService.createItem(this.item)
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
          this.isMainFileAdded = true;
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