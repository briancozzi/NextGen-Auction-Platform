import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import {
    CountryServiceProxy,
    StateServiceProxy,
    CreateStateDto
} from '@shared/service-proxies/service-proxies';
@Component({
    selector: 'createStateModal',
    templateUrl: './create-states-modal.component.html'
})
export class CreateStateModalComponent extends AppComponentBase {

    @ViewChild('createModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    state : CreateStateDto;

    countryList =[];
    constructor(
        injector: Injector,
        private _countryService:CountryServiceProxy,
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
        this.state =  new CreateStateDto();
        this._countryService.getAllCountry().subscribe(result => {
            this.countryList = result.items  
        });
        this.state.countryUniqueId = this.countryList[0].uniqueId;
    }
    save(): void {
        const input = new CreateStateDto();
        input.countryUniqueId = this.state.countryUniqueId;
        input.stateName = this.state.stateName;        
        input.stateCode = this.state.stateCode.toUpperCase();
        this._stateService.create(input)
            .pipe(finalize(() => this.saving = false))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }
    close(): void {
        this.active = false;
        this.modal.hide();
    }
   
}
