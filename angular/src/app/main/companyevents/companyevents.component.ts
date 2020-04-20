import { Component,Injector,ViewChild } from '@angular/core';
import {AppComponentBase} from '@shared/common/app-component-base';
import { CreateEventModalComponent } from './create-event-modal.component'


@Component({
  selector: 'app-companyevents',
  templateUrl: './companyevents.component.html',
  styleUrls: ['./companyevents.component.css'],

})
export class CompanyeventsComponent extends AppComponentBase {
  @ViewChild('createEventModal', {static: true}) createEventModal: CreateEventModalComponent;

  constructor(
    injector: Injector,
    ) { 
    super(injector)
  }
  createCompanyEvent(): void {
    this.createEventModal.show();
  }
}
