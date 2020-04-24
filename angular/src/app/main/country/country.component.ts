import { Component,Injector,ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CountryServiceProxy } from '@shared/service-proxies/service-proxies';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';
import { CreateCountryModalComponent } from './create-country-modal.component'

@Component({
  selector: 'app-country',
  templateUrl: './country.component.html',
  styleUrls: ['./country.component.css']
})
export class CountryComponent extends AppComponentBase{
  @ViewChild('createCountryModal', {static: true}) createCountryModal: CreateCountryModalComponent;

  @ViewChild('dataTable', {static: true}) dataTable: Table;
  @ViewChild('paginator', {static: true}) paginator: Paginator;
  constructor( injector: Injector,
    private _countryService: CountryServiceProxy) 
    { 
      super(injector);
    }
    createCountryEvent(): void {
      this.createCountryModal.show();
    }
  getCountries(): void {
    this.primengTableHelper.showLoadingIndicator();
    this._countryService.getAllCountry()
        .pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
        .subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.items.length;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
}
}
