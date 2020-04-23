import { Component, OnInit,Injector,ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CountryServiceProxy,CountryDto, CountryListDto, CreateCountryDto } from '@shared/service-proxies/service-proxies';
import { Table } from 'primeng/table';
import { LazyLoadEvent } from 'primeng/public_api';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-country',
  templateUrl: './country.component.html',
  styleUrls: ['./country.component.css']
})
export class CountryComponent extends AppComponentBase implements OnInit {

  @ViewChild('dataTable', {static: true}) dataTable: Table;
  @ViewChild('paginator', {static: true}) paginator: Paginator;
  constructor( injector: Injector,
    private _countryService: CountryServiceProxy) 
    { 
      super(injector);
    }
  ngOnInit(): void {
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
