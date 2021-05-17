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
  procurementState :ListDto[] = []; 
  categoryList = [];
  accountList = [];
  isLogo = false;

  constructor(
    injector: Injector,
    private _itemService: ItemServiceProxy,
    private _categoryService: CategoryServiceProxy,
    private _tokenService: TokenService,
    private _accountService: AppAccountServiceProxy
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
      debugger;
      this.item = allResults[0];
      this.categoryList = allResults[1].items;
      this.dropdowns = allResults[2];
      this.procurementState = this.dropdowns.procurementStates;
      this.accountList = allResults[3].items;
      this.isLogo = true;
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
    if (!this.isLogo) {
      if (this.inputFile.nativeElement.value != "") {
        this.logoUploader.uploadAll();
      }
      else {
        this.updateItem();
      }
    }
    else {
      this.updateItem();
    }
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
}
