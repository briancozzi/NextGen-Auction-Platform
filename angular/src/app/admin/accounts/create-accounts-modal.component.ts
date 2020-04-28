import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import {
    AppAccountServiceProxy,
    StateServiceProxy,
    CreateAppAccountDto,
    CountryServiceProxy,
    AddressDto
} from '@shared/service-proxies/service-proxies';
@Component({
    selector: 'createAccountsModal',
    templateUrl: './create-accounts-modal.component.html'
})
export class CreateAccountsModalComponent extends AppComponentBase {

    @ViewChild('createModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    account: CreateAppAccountDto = new CreateAppAccountDto();
    countryList =[];
    stateList =[];
    stateDropdown = true;

    constructor(
        injector: Injector,
        private _accountsService:AppAccountServiceProxy,
        private _countryService: CountryServiceProxy,
        private _stateService: StateServiceProxy
    ) {
        super(injector);
    }

    show() {
        this.active = true;
        this.init();
        this.modal.show();
    }

    onShown(): void {
    }

    init(): void {
        debugger;
        this.account = new CreateAppAccountDto();
        this.account.address = new AddressDto();
        this._countryService.getAllCountry().subscribe(result => {
            this.countryList = result.items  
        });
    }
    loadStateList(countryId):void{
        debugger;
        this.stateDropdown = false;
    }
    save(): void {
        
    }
    close(): void {
        this.active = false;
        this.modal.hide();
    }
   
}