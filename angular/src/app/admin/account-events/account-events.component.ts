import { Component,Injector,ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AccountEventServiceProxy } from '@shared/service-proxies/service-proxies';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';
import { LazyLoadEvent } from 'primeng/public_api';
import { CreateEventsModalComponent } from  './create-events-modal.component';
import { EditEventsModalComponent } from  './edit-events-modal.component';


@Component({
  selector: 'app-account-events',
  templateUrl: './account-events.component.html',
})
export class AccountEventsComponent extends AppComponentBase{
  @ViewChild('createEventsModal',{static: true}) createEventsModal: CreateEventsModalComponent;
  @ViewChild('editEventsModal',{static: true}) editEventsModal: EditEventsModalComponent;
  @ViewChild('dataTable', {static: true}) dataTable: Table;
  @ViewChild('paginator', {static: true}) paginator: Paginator;

  constructor(injector:Injector,
    private _eventService : AccountEventServiceProxy) { 
    super(injector)
  }
  filters: {
    filterText: string;
    } = <any>{};
    getEvents(event?: LazyLoadEvent): void {
      this.primengTableHelper.showLoadingIndicator();
      this._eventService.getAccountEventsWithFilter(
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
      createEvent():void{
        this.createEventsModal.show();
      }
}
