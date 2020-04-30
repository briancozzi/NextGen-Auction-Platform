import { Component,Injector,ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AppAccountServiceProxy,AppAccountDto } from '@shared/service-proxies/service-proxies';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';
import { LazyLoadEvent } from 'primeng/public_api';
import {CreateAccountsModalComponent} from './create-accounts-modal.component'
import {EditAccountsModalComponent} from './edit-accounts-modal.component'

import { AppConsts } from '@shared/AppConsts';

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html'
})
export class AccountsComponent extends AppComponentBase {
  @ViewChild('createAccountsModal',{static: true}) createAccountsModal: CreateAccountsModalComponent;
  @ViewChild('editAccountsModal',{static: true}) editAccountsModal: EditAccountsModalComponent;

  @ViewChild('dataTable', {static: true}) dataTable: Table;
  @ViewChild('paginator', {static: true}) paginator: Paginator;

  constructor(injector: Injector,
    private _appAccountService: AppAccountServiceProxy) { 
    super(injector)
  }

  webHostUrl = AppConsts.remoteServiceBaseUrl;
  Logo : any;
  filters: {
    filterText: string;
    } = <any>{};

    createAccount(){
      this.createAccountsModal.show();
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
    deleteAccount(account: AppAccountDto): void {
      this.message.confirm(
          this.l('DeletingAccount', account.email),
          this.l('AreYouSure'),
          isConfirmed => {
              if (isConfirmed) {
                  this._appAccountService.delete(account.uniqueId).subscribe(() => {
                      this.getAccounts();
                      this.notify.success(this.l('SuccessfullyDeleted'));
                  });
              }
          }
      );
    }
}
