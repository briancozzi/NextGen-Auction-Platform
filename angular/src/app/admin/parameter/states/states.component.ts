import { Component,Injector,ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { StateServiceProxy, StateDto } from '@shared/service-proxies/service-proxies';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';
import { LazyLoadEvent } from 'primeng/public_api';
import{ CreateStateModalComponent } from './create-states-modal.component'
import{ EditStateModalComponent } from './edit-states-modal.component'

@Component({
  selector: 'app-states',
  templateUrl: './states.component.html'
})
export class StatesComponent extends AppComponentBase {
  @ViewChild('createStateModal',{static: true}) createStateModal: CreateStateModalComponent;
  @ViewChild('editStateModal', {static: true}) editStateModal: EditStateModalComponent;
  @ViewChild('dataTable', {static: true}) dataTable: Table;
  @ViewChild('paginator', {static: true}) paginator: Paginator;

  constructor(
    injector:Injector,
    private _stateService: StateServiceProxy
    ) {
    super(injector)
   }
   filters: {
    filterText: string;
    } = <any>{};

  createState(){
    this.createStateModal.show();
  }
   getStates(event?: LazyLoadEvent): void {
    this.primengTableHelper.showLoadingIndicator();
    this._stateService.getStatesByFilter(
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
    deleteState(state: StateDto): void {
      this.message.confirm(
          this.l('DeletingState', state.stateName),
          this.l('AreYouSure'),
          isConfirmed => {
              if (isConfirmed) {
                  this._stateService.delete(state.uniqueId).subscribe(() => {
                      this.getStates();
                      this.notify.success(this.l('SuccessfullyDeleted'));
                  });
              }
          }
      );
    }
}
