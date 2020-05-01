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
    NameValueDto
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
    @Input() defaultTimezoneScope: SettingScopes.Tenant;
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

    constructor(
        injector: Injector,
        private _countryService: CountryServiceProxy,
        private _timingService: TimingServiceProxy,
        private _accountService: AppAccountServiceProxy
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
        this.saveAccount();
    
    }
    saveAccount():void{
       
    }
    close(): void {
        this.active = false;
        this.modal.hide();
    }
   
}