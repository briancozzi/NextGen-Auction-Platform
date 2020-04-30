import{ Component, Injector, ViewChild,Output,EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppAccountServiceProxy,CountryServiceProxy,AppAccountDto,AddressDto } from '@shared/service-proxies/service-proxies';
import {forkJoin} from "rxjs";
@Component({
    selector: 'editAccountsModal',
    templateUrl: './edit-accounts-modal.component.html'
})
export class EditAccountsModalComponent extends AppComponentBase{
    @ViewChild('editModal', {static : true}) modal: ModalDirective;
    @Output () modalSave: EventEmitter<any> = new EventEmitter<any>();

    account: AppAccountDto = new AppAccountDto();
    saving = false;
    active= false;
    stateList =[]; 
    countryList = [];
    countryUniqueId:string;
    stateUniqueId:string;
    constructor(
        injector: Injector,
        private _accountService: AppAccountServiceProxy,
        private _countryService: CountryServiceProxy
    ) {
        super(injector);
    }

    show(AccountId?: string):void{
        debugger;
        this.active = true;
        this.account = new AppAccountDto();
        this.account.address = new AddressDto();
        forkJoin([
            this._countryService.getCountriesWithState(),
            this._accountService.getAccountById(AccountId)
          ]).subscribe(allResults =>{
            this.countryList = allResults[0];
            this.account = allResults[1];
            this.account.address = allResults[1].address;
            this.stateList = this.countryList.find(x=>x.countryUniqueId === this.account.address.countryUniqueId);
            this.modal.show();
        });
          
    }
    
    close(): void {
        this.active = false;
        this.modal.hide();
    }
    save(): void {
        
    }
}