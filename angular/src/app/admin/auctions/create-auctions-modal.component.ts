import { Component, EventEmitter, Injector, Output, ViewChild,Input } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import {
    CountryServiceProxy,
    AddressDto,
    AppAccountServiceProxy,
    CreateAccountEventDto,
    AccountEventServiceProxy,
    CreateAuctionDto,
    AuctionServiceProxy
} from '@shared/service-proxies/service-proxies';
import {forkJoin} from "rxjs";
import * as moment from 'moment';

@Component({
    selector: 'createAuctionsModal',
    templateUrl: './create-auctions-modal.component.html',
    
})
export class CreateAuctionsModalComponent extends AppComponentBase {
    @ViewChild('createModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    auction: CreateAuctionDto = new CreateAuctionDto();
    countryList = [];
    stateList =[];
    accountList = [];
    eventList = [];
    stateDropdown = true;
    logo: string;
    startTime: Date = new Date();
    endTime: Date = new Date();
    isSelected = true;
    

    constructor(
        injector: Injector,
        private _countryService: CountryServiceProxy,
        private _accountService: AppAccountServiceProxy,
        private _eventService: AccountEventServiceProxy,
        private _auctionService: AuctionServiceProxy
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
        this.auction = new CreateAuctionDto();
        this.auction.address = new AddressDto();
        forkJoin([
            this._countryService.getCountriesWithState(),
            this._accountService.getAllAccount(),
            this._eventService.getAllAccountEvents()
          ]).subscribe(allResults =>{
            this.countryList = allResults[0];
            this.auction.address.countryUniqueId = allResults[0][0].countryUniqueId;
            this.loadStateList(this.auction.address.countryUniqueId);
            this.accountList = allResults[1].items;
            this.eventList = allResults[2].items;
            this.auction.auctionStartDateTime = moment(new Date());
            this.auction.auctionEndDateTime = moment(new Date());
        });
    }
    loadStateList(countryId):void{
        this.stateDropdown = false;
        this.stateList = this.countryList.find(x=> x.countryUniqueId === countryId);
    }

    save(): void {
        this.saving = true;
        
    
    }

  
    close(): void {
        this.active = false;
        this.modal.hide();
    }
   
}
