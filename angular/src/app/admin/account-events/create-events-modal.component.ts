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
    TimingServiceProxy,
    SettingScopes, 
    NameValueDto,
    AccountEventServiceProxy
} from '@shared/service-proxies/service-proxies';
import {forkJoin} from "rxjs";
import { ControlValueAccessor, FormControl, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
    selector: 'createEventsModal',
    templateUrl: './create-events-modal.component.html',
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => CreateEventsModalComponent),
        multi: true,
    }]
})
export class CreateEventsModalComponent extends AppComponentBase implements OnInit, ControlValueAccessor{

    @ViewChild('createModal', { static: true }) modal: ModalDirective;
    @Input() defaultTimezoneScope: SettingScopes;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();


    timeZoneList: NameValueDto[] = [];
    selectedTimeZone = new FormControl('');
    onTouched: any = () => { };

    active = false;
    saving = false;
    event: CreateAccountEventDto = new CreateAccountEventDto();
    countryList = [];
    stateList =[];
    accountList = [];
    stateDropdown = true;
    logo: string;
    startTime: any;
    endTime:any;

    constructor(
        injector: Injector,
        private _countryService: CountryServiceProxy,
        private _timingService: TimingServiceProxy,
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

    ngOnInit(): void {
        debugger;
        let self = this;
        self._timingService.getTimezones(self.defaultTimezoneScope).subscribe(result => {
            self.timeZoneList = result.items;
        });
    }
    writeValue(obj: any): void {
        if (this.selectedTimeZone) {
            this.selectedTimeZone.setValue(obj);
        }
    }

    registerOnChange(fn: any): void {
        this.selectedTimeZone.valueChanges.subscribe(fn);
    }

    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }

    setDisabledState?(isDisabled: boolean): void {
        if (isDisabled) {
            this.selectedTimeZone.disable();
        } else {
            this.selectedTimeZone.enable();
        }
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
        this.saving = true;
        let stime = this.event.eventStartTime.split(":");
        this.startTime = new Date(0, 0, 0, parseInt(stime[0]), parseInt(stime[1]), 0, 0);
        let ltime = this.event.eventEndTime.split(":");
        this.endTime = new Date(0, 0, 0, parseInt(ltime[0]), parseInt(ltime[1]), 0, 0);
        this.event.eventStartTime = this.startTime;
        this.event.eventEndTime = this.endTime;
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