import { Component, EventEmitter, Injector, Output, ViewChild,Input, OnInit, AfterViewInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import {
    CountryServiceProxy,
    AddressDto,
    AppAccountServiceProxy,
    AccountEventServiceProxy,
    CreateAuctionDto,
    AuctionServiceProxy
} from '@shared/service-proxies/service-proxies';
import {forkJoin} from "rxjs";
import * as moment from 'moment';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Router } from '@angular/router';

@Component({
   // selector: 'createAuction',
    templateUrl: './create-auction.component.html',
    animations: [appModuleAnimation()]
})
export class CreateAuctionComponent extends AppComponentBase implements OnInit, AfterViewInit {
  
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
        private _auctionService: AuctionServiceProxy,
        private _router: Router
    ) {
        super(injector);
    }
  
    ngOnInit() {
        this.active = true;
        this.init();
       
    }

    ngAfterViewInit(): void {
    }

    init(): void {
        this.auction = new CreateAuctionDto();
        this.auction.address = new AddressDto();
        this.auction.accountUniqueId = "";
        this.auction.eventUniqueId = "";
        this.auction.address.stateUniqueId = "";
        forkJoin([
            this._countryService.getCountriesWithState(),
            this._accountService.getAllAccount(),
            this._eventService.getAllAccountEvents(),
          ]).subscribe(allResults =>{
            this.countryList = allResults[0];
            this.auction.address.countryUniqueId = allResults[0][0].countryUniqueId;
            this.loadStateList(this.auction.address.countryUniqueId);
            this.accountList = allResults[1].items;
            this.eventList = allResults[2].items;
            this.auction.auctionStartDateTime = moment();
            this.auction.auctionEndDateTime = moment();
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
        var EndDate =   this.auction.auctionEndDateTime.local().format("YYYY-MM-DD");
        var StartDate = this.auction.auctionStartDateTime.local().format("YYYY-MM-DD");
        this.auction.auctionStartDateTime = moment(StartDate + ' ' + stime);
        this.auction.auctionEndDateTime = moment(EndDate + ' ' + etime);
        this._auctionService.createAuction(this.auction)
        .pipe(finalize(() => this.saving = false))
        .subscribe(() => {
            this.notify.info(this.l('SavedSuccessfully'));
            this.close();
           
        });
    }
  
    close(): void {
        this.active = false;
        this._router.navigate(['app/admin/auctions']);
    }
   
}
