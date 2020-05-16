import { Component, Injector, ViewChild, Output, EventEmitter, ElementRef, OnInit, AfterViewInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { ActivatedRoute, Router } from '@angular/router';

import {
    AddressDto,
    AccountEventServiceProxy,
    UpdateAccountEventDto,
    CountryServiceProxy,
    AppAccountServiceProxy,
    AccountEventDto,
    SettingScopes,
    UpdateAuctionDto,
    AuctionServiceProxy,
    CountryStateDto
} from '@shared/service-proxies/service-proxies';
import { forkJoin } from "rxjs";
import { AppConsts } from '@shared/AppConsts';
import * as moment from 'moment';
@Component({
    templateUrl: './edit-auction.component.html',
    animations: [appModuleAnimation()]
})
export class EditAuctionComponent extends AppComponentBase implements OnInit {
    
    auction: UpdateAuctionDto = new UpdateAuctionDto();
    saving = false;
    active = false;
    stateList: CountryStateDto = new CountryStateDto();
    countryList = [];
    accountList = [];
    eventList = [];
    countryUniqueId: string;
    stateUniqueId: string;
    startTime: Date = new Date();
    endTime: Date = new Date();
    stateDropdown = true;
    isSelected = true;

    constructor(
        injector: Injector,
        private _eventService: AccountEventServiceProxy,
        private _countryService: CountryServiceProxy,
        private _accountService: AppAccountServiceProxy,
        private _auctionService: AuctionServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _router: Router 
    ) {
        super(injector);
    }

    ngOnInit (): void {
        debugger;
        var AuctionId = this._activatedRoute.snapshot.queryParams['auctionId'];
        this.active = true;
        this.auction = new UpdateAuctionDto();
        this.auction.address = new AddressDto();
        forkJoin([
            this._countryService.getCountriesWithState(),
            this._auctionService.getAuctionById(AuctionId),
            this._eventService.getAllAccountEvents(),
            this._accountService.getAllAccount(),
        ]).subscribe(allResults => {
            this.countryList = allResults[0];
            this.auction = allResults[1];
            this.auction.address = allResults[1].address;
            this.stateList = this.countryList.find(x => x.countryUniqueId === this.auction.address.countryUniqueId);
            this.eventList = allResults[2].items;
            this.accountList = allResults[3].items;
            
            var startTimeDt = this.auction.auctionStartDateTime.clone().toDate();
            startTimeDt.setHours(parseInt(this.auction.auctionStartDateTime.format("HH")));
            startTimeDt.setMinutes(parseInt(this.auction.auctionStartDateTime.format("mm")));
            this.startTime = startTimeDt;

            var endTimeDt = this.auction.auctionEndDateTime.clone().toDate();
            endTimeDt.setHours(parseInt(this.auction.auctionEndDateTime.format("HH")));
            endTimeDt.setMinutes(parseInt(this.auction.auctionEndDateTime.format("mm")));
            this.endTime = endTimeDt;


        });
    }

    loadStateList(countryId): void {
        this.stateDropdown = false;
        this.stateList = this.countryList.find(x => x.countryUniqueId === countryId);
    }

    close(): void {
        this.active = false;
        this._router.navigate(['app/admin/auctions']);
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
        this._auctionService.updateAuction(this.auction)
        .pipe(finalize(() => this.saving = false))
        .subscribe(() => {
            this.notify.info(this.l('SavedSuccessfully'));
            this.close();
        });
    }

}
