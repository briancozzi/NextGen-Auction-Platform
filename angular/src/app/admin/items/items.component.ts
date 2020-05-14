import { Component, Injector, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ItemServiceProxy, ItemListDto } from '@shared/service-proxies/service-proxies';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';
import { LazyLoadEvent } from 'primeng/public_api';
import { CreateItemModalComponent } from './create-item-modal.component';
import { EditItemModalComponent } from './edit-item-modal.component';
import { AppConsts } from '@shared/AppConsts';


@Component({
  selector: 'app-items',
  templateUrl: './items.component.html'
})
export class ItemsComponent extends AppComponentBase {
  @ViewChild('createItemModal',{static: true}) createItemModal: CreateItemModalComponent;
  @ViewChild('editItemModal',{static: true}) editItemModal: EditItemModalComponent;

  @ViewChild('dataTable', {static: true}) dataTable: Table;
  @ViewChild('paginator', {static: true}) paginator: Paginator;

  webHostUrl = AppConsts.remoteServiceBaseUrl;

  filters: {
    filterText: string;
    } = <any>{};


  constructor(injector: Injector,
    private _itemService: ItemServiceProxy) {
    super(injector)
  }

  createItem(){
    this.createItemModal.show();
  }
  getItems(event?: LazyLoadEvent): void {
    debugger;
    this.primengTableHelper.showLoadingIndicator();
    this._itemService.getItemsWithFilter(
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
    deleteAccount(item: ItemListDto): void {
      this.message.confirm(
          this.l('DeleteItem', item.itemName),
          this.l('AreYouSure'),
          isConfirmed => {
              if (isConfirmed) {
                  this._itemService.deleteItem(item.uniqueId).subscribe(() => {
                      this.getItems();
                      this.notify.success(this.l('SuccessfullyDeleted'));
                  });
              }
          }
      );
    }

}
