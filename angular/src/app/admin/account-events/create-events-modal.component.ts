import { Component, EventEmitter, Injector, Output, ViewChild,Input } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { findOneIana } from "windows-iana";

import {
    CountryServiceProxy,
    AddressDto,
    AppAccountServiceProxy,
    CreateAccountEventDto,
    AccountEventServiceProxy,
    TimingServiceProxy,
    SettingScopes, 
    NameValueDto
} from '@shared/service-proxies/service-proxies';
import {forkJoin} from "rxjs";
import * as moment from 'moment';
import { settings } from 'cluster';

@Component({
    selector: 'createEventsModal',
    templateUrl: './create-events-modal.component.html',
    
})
export class CreateEventsModalComponent extends AppComponentBase {
    @Input() defaultTimezoneScope: SettingScopes;
    @ViewChild('createModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    event: CreateAccountEventDto = new CreateAccountEventDto();
    countryList = [];
    stateList =[];
    accountList = [];
    stateDropdown = true;
    logo: string;
    startTime: Date = new Date();
    endTime: Date = new Date();
    timeZones: NameValueDto[] = [];
    isSelected = true;
    

    constructor(
        injector: Injector,
        private _countryService: CountryServiceProxy,
        private _accountService: AppAccountServiceProxy,
        private _eventService: AccountEventServiceProxy,
        private _timingService: TimingServiceProxy
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
        this.event = new CreateAccountEventDto();
        this.event.address = new AddressDto();

        forkJoin([
            this._countryService.getCountriesWithState(),
            this._accountService.getAllAccount(),
            this._timingService.getTimezones(SettingScopes.Application)
          ]).subscribe(allResults =>{
            this.countryList = allResults[0];
            this.event.address.countryUniqueId = allResults[0][0].countryUniqueId;
            this.loadStateList(this.event.address.countryUniqueId);
            this.accountList = allResults[1].items;
            this.event.eventStartDateTime = moment();
            this.event.eventEndDateTime = moment();
            this.timeZones = allResults[2].items;
        });
    }
    loadStateList(countryId):void{
        this.stateDropdown = false;
        this.stateList = this.countryList.find(x=> x.countryUniqueId === countryId);
    }

    getTimePart(dateTimeVal): string {
        var timePart = dateTimeVal.toTimeString().split(":");
        return timePart[0] + ":" + timePart[1];
    }

    save(): void {
        this.saving = true;
        var stime = this.getTimePart(this.startTime);
        var etime = this.getTimePart(this.endTime);
        var eventEndDate =   this.event.eventEndDateTime.local().format("YYYY-MM-DD");
        var eventStartDate = this.event.eventStartDateTime.local().format("YYYY-MM-DD");

        var selectedtimezoneId = findOneIana(this.event.timeZone);

        this.event.eventStartDateTime = moment.tz(eventStartDate + ' ' + stime, selectedtimezoneId);
        this.event.eventEndDateTime = moment.tz(eventEndDate + ' ' + etime, selectedtimezoneId);

        this._eventService.create(this.event)
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
