import { Component, Injector, ViewChild,OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AuctionItemServiceProxy, AuctionItemListDto } from '@shared/service-proxies/service-proxies';
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
  @ViewChild('createAuctionItem',{static: true}) createAuctionItem: CreateAuctionItemsComponent;

  filters: {
    filterText: string;
    } = <any>{};
  constructor(injector: Injector,
    private _itemService: AuctionItemServiceProxy) {
    super(injector)
  }

  getItems(event?: LazyLoadEvent): void {
    
    this.primengTableHelper.showLoadingIndicator();
    this._itemService.getAuctionItemsWithFilter(
    this.filters.filterText,
    this.primengTableHelper.getSorting(this.dataTable),
    this.primengTableHelper.getMaxResultCount(this.paginator, event),
    this.primengTableHelper.getSkipCount(this.paginator, event)
    )
        .pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
        .subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    deleteItem(item:AuctionItemListDto): void {
    
      this.message.confirm(
          this.l('DeleteItem', item.auctionItemId),
          this.l('AreYouSure'),
          isConfirmed => {
              if (isConfirmed) {
                  this._itemService.delete(item.auctionItemId).subscribe(() => {
                      this.getItems();
                      this.notify.success(this.l('SuccessfullyDeleted'));
                  });
              }
          }
      );
    }

  createItem():void{
    
    this.createAuctionItem.show();
  }

}
