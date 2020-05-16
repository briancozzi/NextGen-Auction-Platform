import { Component, Injector, ViewChild, Output, EventEmitter, ElementRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { findOneIana } from "windows-iana";

import {
    AddressDto,
    AccountEventServiceProxy,
    UpdateAccountEventDto,
    CountryServiceProxy,
    AppAccountServiceProxy,
    AccountEventDto,
    SettingScopes,
    NameValueDto,
    TimingServiceProxy,
    CountryStateDto
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
    stateList: CountryStateDto = new CountryStateDto();
    countryList = [];
    accountList = [];
    countryUniqueId: string;
    stateUniqueId: string;
    timeZones: NameValueDto[] = [];
    startTime: Date = new Date();
    endTime: Date = new Date();
    stateDropdown = true;
    isSelected = true;

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
            this.countryList = allResults[0];
            this.event = allResults[1];
            this.event.address = allResults[1].address;
            this.stateList = this.countryList.find(x => x.countryUniqueId === this.event.address.countryUniqueId);
            this.accountList = allResults[2].items;
            this.timeZones = allResults[3].items;

            var selectedtimezoneId = findOneIana(this.event.timeZone);

            this.event.eventStartDateTime = this.event.eventStartDateTime.clone().tz(selectedtimezoneId);
            this.event.eventEndDateTime = this.event.eventEndDateTime.clone().tz(selectedtimezoneId);

            var startTimeDt = this.event.eventStartDateTime.clone().toDate();
            startTimeDt.setHours(parseInt(this.event.eventStartDateTime.format("HH")));
            startTimeDt.setMinutes(parseInt(this.event.eventStartDateTime.format("mm")));
            this.startTime = startTimeDt;

            var endTimeDt = this.event.eventEndDateTime.clone().toDate();
            endTimeDt.setHours(parseInt(this.event.eventEndDateTime.format("HH")));
            endTimeDt.setMinutes(parseInt(this.event.eventEndDateTime.format("mm")));
            this.endTime = endTimeDt;

            this.modal.show();
        });

    }
    loadStateList(countryId): void {
        this.stateDropdown = false;
        this.stateList = this.countryList.find(x => x.countryUniqueId === countryId);
    }
    close(): void {
        this.active = false;
        this.modal.hide();
    }
    getTimePart(dateTimeVal): string {
        var timePart = dateTimeVal.toTimeString().split(":");
        return timePart[0] + ":" + timePart[1];
    }
    save(): void {
        this.saving  = true;
        var stime = this.getTimePart(this.startTime);
        var etime = this.getTimePart(this.endTime);

        var eventEndDate = moment(this.event.eventEndDateTime).local().format("YYYY-MM-DD");
        var eventStartDate = moment(this.event.eventStartDateTime).local().format("YYYY-MM-DD");

        var selectedtimezoneId = findOneIana(this.event.timeZone);

        this.event.eventStartDateTime = moment.tz(eventStartDate + ' ' + stime, selectedtimezoneId);
        this.event.eventEndDateTime = moment.tz(eventEndDate + ' ' + etime, selectedtimezoneId);

        this._eventService.update(this.event)
            .pipe(finalize(() => this.saving = false))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

}
