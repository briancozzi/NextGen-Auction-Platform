import { Component, EventEmitter, Injector, Output, ViewChild,OnInit,forwardRef,Input } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import {
    CountryServiceProxy,
    AddressDto,
    AppAccountServiceProxy,
    CreateAccountEventDto,
    AccountEventServiceProxy
} from '@shared/service-proxies/service-proxies';
import {forkJoin} from "rxjs";
import * as moment from 'moment';

@Component({
    selector: 'createEventsModal',
    templateUrl: './create-events-modal.component.html',
    
})
export class CreateEventsModalComponent extends AppComponentBase {

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

    constructor(
        injector: Injector,
        private _countryService: CountryServiceProxy,
        private _accountService: AppAccountServiceProxy,
        private _eventService: AccountEventServiceProxy
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
          ]).subscribe(allResults =>{
            this.countryList = allResults[0];
            this.event.address.countryUniqueId = allResults[0][0].countryUniqueId;
            this.loadStateList(this.event.address.countryUniqueId);
            this.accountList = allResults[1].items;
        });
    }
    loadStateList(countryId):void{
        this.stateDropdown = false;
        this.stateList = this.countryList.find(x=> x.countryUniqueId === countryId);
    }

    save(): void {
        debugger;
        this.saving = true;
        var stime = moment(this.startTime).format("HH:mm");
        var etime = moment(this.endTime).format("HH:mm");
        var eventEndDate =   this.event.eventEndDateTime.local().format().split("T")[0];
        var eventStartDate =  this.event.eventStartDateTime.local().format().split("T")[0]; 

        this.event.eventEndDateTime = moment(eventEndDate + ' ' + etime);
        this.event.eventStartDateTime = moment(eventStartDate + ' ' + stime);

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