import { Component, EventEmitter, Injector, Output, ViewChild, OnInit, ElementRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import {
  ItemServiceProxy, ItemDto
} from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { forkJoin } from "rxjs";

@Component({
  selector: 'app-create-item-modal',
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
  account: ItemDto = new ItemDto();
  uploadedFiles: any[] = [];
  logo: string;
  isSelected = true;

  constructor(
    injector: Injector,
    private _itemService: ItemServiceProxy
  ) {
    super(injector);
   }

  ngOnInit(): void {
  }
  show(){
    this.active = true;
    this.modal.show();
  }

}
