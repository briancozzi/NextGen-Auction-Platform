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
    selector: 'editEventsModal',
    templateUrl: './edit-events-modal.component.html'
})
export class EditEventsModalComponent extends AppComponentBase {
    @ViewChild('editModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('inputFile') inputFile: ElementRef;

    webHostUrl = AppConsts.remoteServiceBaseUrl;
    event: UpdateAccountEventDto = new UpdateAccountEventDto();
    saving = false;
    active = false;
    stateList = [];
    countryList = [];
    accountList = [];
    countryUniqueId: string;
    stateUniqueId: string;
    timeZones: NameValueDto[] = [];
    startTime: any;
    endTime: any;

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
        this.active = true;
        this.event = new UpdateAccountEventDto();
        this.event.address = new AddressDto();
        forkJoin([
            this._countryService.getCountriesWithState(),
            this._eventService.getAccountEventById(EventId),
            this._accountService.getAllAccount(),
            this._timingService.getTimezones(SettingScopes.Application)
        ]).subscribe(allResults => {
            debugger;
            this.countryList = allResults[0];
            this.event = allResults[1];
            this.event.address = allResults[1].address;
            this.stateList = this.countryList.find(x => x.countryUniqueId === this.event.address.countryUniqueId);
            this.accountList = allResults[2].items;
            this.timeZones = allResults[3].items;
            this.startTime = this.event.eventStartDateTime.toDate();
            this.endTime = this.event.eventEndDateTime.toDate();
            this.modal.show();
        });

    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
    save(): void {
        this.saving  = true;
        var stime = moment(this.startTime).local().format("HH:mm");
        var etime = moment(this.endTime).local().format("HH:mm");
        var eventEndDate =   this.event.eventEndDateTime.local().format().split("T")[0];
        var eventStartDate =  this.event.eventStartDateTime.local().format().split("T")[0]; 
        this.event.eventEndDateTime = moment(eventEndDate + ' ' + etime);
        this.event.eventStartDateTime = moment(eventStartDate + ' ' + stime);
        this._eventService.update(this.event)
            .pipe(finalize(() => this.saving = false))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

}