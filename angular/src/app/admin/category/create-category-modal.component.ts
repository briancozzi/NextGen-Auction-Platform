import { Component, EventEmitter, Injector, Output, ViewChild, OnInit, ElementRef } from '@angular/core';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/common/app-component-base';

import {  CategoryServiceProxy, CreateCategoryDto } from '@shared/service-proxies/service-proxies'
@Component({
  selector: 'createCategoryModal',
  templateUrl: './create-category-modal.component.html'
})
export class CreateCategoryModalComponent extends AppComponentBase implements OnInit {
  @ViewChild('createModal', { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
  active = false;
  saving = false;
  category: CreateCategoryDto = new CreateCategoryDto;
  constructor(injector: Injector,
              private _categoryService: CategoryServiceProxy ) {
      super(injector);
   }

  ngOnInit(): void {
  }
  close(){
    this.active = false;

    this.modal.hide();
  }
  save():void{
      
      this.saving = true;
      this._categoryService.create(this.category)
      .pipe(finalize(() => this.saving = false))
      .subscribe(() => {
          this.notify.info(this.l('SavedSuccessfully'));
          this.close();
          this.modalSave.emit(null);
      });
  }
  show(){
    this.category = new CreateCategoryDto();
    this.category.categoryName = "";
    this.active = true;
    this.modal.show();
  }

}
