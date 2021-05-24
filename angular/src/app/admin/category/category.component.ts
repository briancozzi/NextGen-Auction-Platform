import { Component, Injector, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';
import { LazyLoadEvent } from 'primeng/public_api';
import { AppConsts } from '@shared/AppConsts';
import {  CategoryServiceProxy } from '@shared/service-proxies/service-proxies'

import { CreateCategoryModalComponent } from './create-category-modal.component';
import { EditCategoryModalComponent } from './edit-category-modal.component';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html'
})
export class CategoryComponent extends AppComponentBase{
  @ViewChild('dataTable', {static: true}) dataTable: Table;
  @ViewChild('paginator', {static: true}) paginator: Paginator;

  @ViewChild('createCategoryModal',{static: true}) createCategoryModal: CreateCategoryModalComponent;
  @ViewChild('editCategoryModal',{static: true}) editCategoryModal: EditCategoryModalComponent;


 filters: {
    filterText: string;
    } = <any>{};
  constructor(injector: Injector,
              private _categoryService:CategoryServiceProxy ) {
       super(injector)
  }

  ngOnInit(): void {

  }
  createCategory(){
    this.createCategoryModal.show();
  }
  deleteCategory(categoryId): void{
    this.message.confirm(
        this.l('DeleteCategory', categoryId),
        this.l('AreYouSure'),
        isConfirmed => {
            if (isConfirmed) {
                this._categoryService.delete(categoryId).subscribe(() => {
                    this.getCategory();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        }
    );
  }
  getCategory(event?: LazyLoadEvent): void {
    
    this.primengTableHelper.showLoadingIndicator();
    this._categoryService.getCategoryWithFilter(
    this.filters.filterText,
    this.primengTableHelper.getSorting(this.dataTable),
    this.primengTableHelper.getMaxResultCount(this.paginator, event),
    this.primengTableHelper.getSkipCount(this.paginator, event)
    )
        .pipe(finalize(() => this.primengTableHelper.hideLoadingIndicator()))
        .subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }
}
