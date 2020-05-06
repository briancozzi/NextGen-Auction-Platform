import { Component, Injector, ViewChild, Output, EventEmitter, ElementRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import {
    AddressDto,
    AccountEventServiceProxy,
    UpdateAccountEventDto,
    CountryServiceProxy,
    AppAccountServiceProxy,
    AccountEventDto,
    SettingScopes,
    NameValueDto,
    TimingServiceProxy
} from '@shared/service-proxies/service-proxies';
import { forkJoin } from "rxjs";
import { AppConsts } from '@shared/AppConsts';
import * as moment from 'moment';
@Component({
    selector: 'editAuctionsModal',
    templateUrl: './edit-auctions-modal.component.html'
})
export class EditAuctionsModalComponent extends AppComponentBase {
   

    constructor(
        injector: Injector,
        private _eventService: AccountEventServiceProxy,
        private _countryService: CountryServiceProxy,
        private _accountService: AppAccountServiceProxy,
        private _timingService: TimingServiceProxy
    ) {
        super(injector);
    }

    show(EventId?: string): void {
      

    }

    close(): void {
     
    }
    save(): void {
    }

}