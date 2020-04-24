import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import {
    CountryDto,
   CountryServiceProxy
} from '@shared/service-proxies/service-proxies';
@Component({
    selector: 'editCountryModal',
    templateUrl: './edit-country-modal.component.html'
})
export class EditCountryModalComponent extends AppComponentBase {

    @ViewChild('editModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    country: CountryDto = new CountryDto();
    constructor(
        injector: Injector,
        private _countryService: CountryServiceProxy
    ) {
        super(injector);
    }

    show(countryId?: string):void {
        this.active = true;
        this._countryService.getCountryById(countryId).subscribe(countryResult => {
                this.country.countryCode = countryResult.countryCode;
                this.country.countryName = countryResult.countryName;
                this.country.uniqueId = countryResult.uniqueId;
                this.modal.show();
        });
    }

    onShown(): void {
    }

    init(): void {
       
    }
    save(): void {
        const input = new CountryDto();
        input.uniqueId = this.country.uniqueId;
        input.countryCode = this.country.countryCode;
        input.countryName = this.country.countryName;        

        this._countryService.update(input)
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