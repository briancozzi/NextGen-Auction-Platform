import { Component,Injector,ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CountryServiceProxy,CountryListDto } from '@shared/service-proxies/service-proxies';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-states',
  templateUrl: './states.component.html',
})
export class StatesComponent extends AppComponentBase {

  constructor(injector:Injector) {
    super(injector)
   }
 

}
