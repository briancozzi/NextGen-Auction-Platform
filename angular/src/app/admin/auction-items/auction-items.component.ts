import { Component, Injector, ViewChild,OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AuctionItemServiceProxy } from '@shared/service-proxies/service-proxies';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';
import { LazyLoadEvent } from 'primeng/public_api';
import { AppConsts } from '@shared/AppConsts';
import { CreateAuctionItemsComponent } from './create-auction-items.component';
import { EditAuctionItemsComponent } from './edit-auction-items.component';

@Component({
  selector: 'app-auction-items',
  templateUrl: './auction-items.component.html'
})
export class AuctionItemsComponent extends AppComponentBase {

  @ViewChild('dataTable', {static: true}) dataTable: Table;
  @ViewChild('paginator', {static: true}) paginator: Paginator;
  @ViewChild('createItemModal',{static: true}) createItemModal: CreateAuctionItemsComponent;
  @ViewChild('editItemModal',{static: true}) editItemModal: EditAuctionItemsComponent;

  constructor(injector: Injector,
    private _itemService: AuctionItemServiceProxy) {
    super(injector)
  }

  getItems(event?: LazyLoadEvent): void {
    
    }

  deleteItem():void{

  }

}
