import { Component, EventEmitter, Injector, Output, ViewChild, OnInit, ElementRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import {
  ItemServiceProxy, ItemDto, CategoryServiceProxy,AppAccountServiceProxy, AuctionServiceProxy, AuctionItemServiceProxy, AuctionItemDto, CreateAuctionDto, CreateAuctionItemDto
} from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { FileUploader, FileUploaderOptions, FileLikeObject } from 'ng2-file-upload';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import { forkJoin } from "rxjs";


@Component({
  selector: 'createAuctionItem',
  templateUrl: './create-auction-items.component.html'
})
export class CreateAuctionItemsComponent extends AppComponentBase implements OnInit{

  @ViewChild('createModal', { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  auctionTypeList:any;
  itemList:any;
  active = false;
  saving = false;
  item: CreateAuctionItemDto = new CreateAuctionItemDto();
  constructor(
    injector: Injector,
    private _itemService: ItemServiceProxy,
    private _auctionService: AuctionServiceProxy,
    private _auctionItemService: AuctionItemServiceProxy
  ) {
    super(injector);
   }

  ngOnInit(): void {
  }
  show():void{
    debugger;
    this.active = true;
    this.init();
    this.modal.show();
  }

  init(): void {
    debugger;
    forkJoin([
      this._itemService.getItems(),
      this._auctionService.getAuctions()
    ]).subscribe(result => {
      this.itemList = result[0],
      this.auctionTypeList = result[1];
      this.item.auctionId = '';
      this.item.itemId = '';
    });
    
  }
  close(): void {
    this.active = false;
    this.modal.hide();
  }
  save(): void {
    this._auctionItemService.create(this.item)
        .pipe(finalize(() => this.saving = false))
        .subscribe(() => {
            this.notify.info(this.l('SavedSuccessfully'));
            this.close();
            this.modalSave.emit(null);
        });
}

}
