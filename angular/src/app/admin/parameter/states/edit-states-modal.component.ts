import{ Component, Injector, ViewChild,Output,EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { StateServiceProxy, StateDto, UpdateStateDto, CountryServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'editStateModal',
    templateUrl: './edit-states-modal.component.html'
})

export class EditStateModalComponent extends AppComponentBase{
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
        this.active = true;
        this.init();
       this._stateService.getStateById(stateId).subscribe((stateResult) =>{
            this.state.countryUniqueId = stateResult.countryUniqueId;
            this.state.stateCode = stateResult.stateCode;
            this.state.stateName = stateResult.stateName;
            this.state.uniqueId = stateResult.uniqueId;
        });
        this.modal.show();
    }
    init(){
        this._countryService.getAllCountry().subscribe(result => {
            this.countryList = result.items  
        });
    }
    close(): void {
        this.active = false;
        this.modal.hide();
    }
    save(): void {
        const input = new UpdateStateDto();
        input.countryUniqueId = this.state.countryUniqueId;
        input.stateName = this.state.stateName;        
        input.stateCode = this.state.stateCode;
        input.uniqueId = this.state.uniqueId;
        this._stateService.update(input)
            .pipe(finalize(() => this.saving = false))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }
}