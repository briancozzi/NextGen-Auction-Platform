import { Component, Injector, ViewChild, Output, EventEmitter, ElementRef, OnInit, AfterViewInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { ActivatedRoute, Router } from '@angular/router';
import {
   CategoryServiceProxy,
   UpdateCategoryDto
} from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { forkJoin } from "rxjs";
@Component({
  selector: 'editCategoryModal',
  templateUrl: './edit-category-modal.component.html'
})
export class EditCategoryModalComponent extends AppComponentBase implements OnInit {
  @ViewChild('editModal', { static: true }) modal: ModalDirective;
  @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    category: any;
    saving = false;
    active = false;
  constructor( injector: Injector,
              private _categoryService: CategoryServiceProxy,
              private _activatedRoute: ActivatedRoute,
              private _router: Router  ) {
    super(injector);
   }
  show(categoryId){
     forkJoin([
       this._categoryService.getCategoryById(categoryId)
    ]).subscribe(result => {
       this.category = result[0];
      this.active =true;
      this.modal.show();
    });
  }
  close(){
    this.active = false;
    this.modal.hide();
  }
  ngOnInit(): void {

  }
 save():void{
      this.saving = true;
      var updateCategory = new UpdateCategoryDto();
      updateCategory.uniqueId = this.category.uniqueId;
      updateCategory.categoryName = this.category.categoryName;
      this._categoryService.update(updateCategory)
      .pipe(finalize(() => this.saving = false))
      .subscribe(() => {
          this.notify.info(this.l('SavedSuccessfully'));
          this.close();
          this.modalSave.emit(null);
      });
  }

}
