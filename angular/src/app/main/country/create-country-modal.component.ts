import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
     CreateCountryDto,
    CountryServiceProxy
} from '@shared/service-proxies/service-proxies';
@Component({
    selector: 'createCountryModal',
    templateUrl: './create-country-modal.component.html'
})
export class CreateCountryModalComponent extends AppComponentBase {

    @ViewChild('createModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active= false;
    saving = false;
    country: CreateCountryDto = new CreateCountryDto();
    constructor(
        injector: Injector,
        private _countryService: CountryServiceProxy
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
        this.country = new CreateCountryDto();
    }
    save(): void {
        const input = new CreateCountryDto();
        input.countryCode = this.country.countryCode.toUpperCase();
        input.countryName = this.country.countryName;        

        this._countryService.create(input)
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
