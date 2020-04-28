import { Component,Injector,ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AppAccountServiceProxy } from '@shared/service-proxies/service-proxies';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';
import { LazyLoadEvent } from 'primeng/public_api';

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html'
})
export class AccountsComponent extends AppComponentBase {
  @ViewChild('dataTable', {static: true}) dataTable: Table;
  @ViewChild('paginator', {static: true}) paginator: Paginator;

  constructor(injector: Injector,
    private _appAccountService: AppAccountServiceProxy) { 
    super(injector)
  }

  filters: {
    filterText: string;
    } = <any>{};

    createAccount(){
      alert("Hi");
    }
  getAccounts(event?: LazyLoadEvent): void {
    
    this.primengTableHelper.showLoadingIndicator();
    this._appAccountService.getAllAccountFilter(
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
}
