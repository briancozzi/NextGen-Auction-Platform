import{ Component, Injector, ViewChild,Output,EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { StateServiceProxy, StateDto, UpdateStateDto, CountryServiceProxy } from '@shared/service-proxies/service-proxies';
import {forkJoin} from "rxjs";
@Component({
    selector: 'editAccountsModal',
    templateUrl: './edit-accounts-modal.component.html'
})
export class EditAccountsModalComponent extends AppComponentBase{
    @ViewChild('editModal', {static : true}) modal: ModalDirective;
    @Output () modalSave: EventEmitter<any> = new EventEmitter<any>();

    saving = false;
    active= false;
    state: StateDto =  new StateDto();
    countryList = [];
    constructor(
        injector: Injector,
        private _stateService: StateServiceProxy,
        private _countryService: CountryServiceProxy
    ) {
        super(injector);
    }

    show(stateId?: string):void{
        
    }
    
    close(): void {
        this.active = false;
        this.modal.hide();
    }
    save(): void {
        
    }
}