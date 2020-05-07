import { Component, Injector, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AuctionServiceProxy, AuctionListDto } from '@shared/service-proxies/service-proxies';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';
import { LazyLoadEvent } from 'primeng/public_api';
import { CreateAuctionsModalComponent } from './create-auctions-modal.component';
import { EditAuctionsModalComponent } from './edit-auctions-modal.component';
import {forkJoin} from "rxjs";
import * as moment from 'moment';
@Component({
  selector: 'app-auctions',
  templateUrl: './auctions.component.html',
})
export class AuctionsComponent extends AppComponentBase {
  @ViewChild('createAuctionsModal', { static: true }) createAuctionsModal: CreateAuctionsModalComponent;
  @ViewChild('editAuctionsModal', { static: true }) editAuctionsModal: EditAuctionsModalComponent;
  @ViewChild('dataTable', { static: true }) dataTable: Table;
  @ViewChild('paginator', { static: true }) paginator: Paginator;

  constructor(injector: Injector,
    private _auctionService: AuctionServiceProxy,
  ) {
    super(injector)
  }
 

  filters: {
    filterText: string;
  } = <any>{};
  
  getAuctions(event?: LazyLoadEvent): void {
    debugger;
    this.primengTableHelper.showLoadingIndicator();
      this._auctionService.getAllAuctionFilter(
        this.filters.filterText,
        this.primengTableHelper.getSorting(this.dataTable),
        this.primengTableHelper.getMaxResultCount(this.paginator, event),
        this.primengTableHelper.getSkipCount(this.paginator, event)
      ).pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
    .subscribe(result => {
      this.primengTableHelper.totalRecordsCount = result.totalCount;
      this.primengTableHelper.records = result.items;
      this.primengTableHelper.hideLoadingIndicator();
    });
      
  }
  createAuction(): void {
    this.createAuctionsModal.show();
  }
  deleteAuction(auction: AuctionListDto): void {
    this.message.confirm(
        this.l('DeleteAuction', auction.auctionType),
        this.l('AreYouSure'),
        isConfirmed => {
            if (isConfirmed) {
                this._auctionService.delete(auction.uniqueId).subscribe(() => {
                    this.getAuctions();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        }
    );
  }
}
