import { Component, EventEmitter, Injector, Output, ViewChild, OnInit, ElementRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import {
  ItemServiceProxy, ItemDto
} from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { FileUploader, FileUploaderOptions,FileLikeObject  } from 'ng2-file-upload';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { forkJoin } from "rxjs";

@Component({
  selector: 'createItemModal',
  templateUrl: './create-item-modal.component.html'
})

export class CreateItemModalComponent extends AppComponentBase implements OnInit {
  @ViewChild('inputFile') inputFile: ElementRef;
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
  dropdowns: any;
  AdditionalFiles: any[] = [];
  fileList: FileList; 

  

  constructor(
    injector: Injector,
    private _itemService: ItemServiceProxy,
    private _tokenService: TokenService
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.initUploaders();
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
      form.append('AdditionalFile', JSON.stringify(this.AdditionalFiles));
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
      }
    );
  }
  show() {
    this.active = true;
    this.init();
    this.modal.show();
  }
  init(): void {
    debugger;
    forkJoin([
      this._itemService.getDropdowns()
    ]).subscribe(result => {
      this.dropdowns = result[0];
    });
  }
  close(): void {
    this.active = false;
    this.modal.hide();
  }
  save(): void {
    this.saving = true;
    this.logoUploader.uploadAll();
  }
  getFiles(event):void {
    this.fileList = event.target.files;
   this.AdditionalFiles = Array.from(this.fileList);
    console.log(this.AdditionalFiles);
    console.log(JSON.stringify(this.AdditionalFiles));
  }
}
