import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ApplicationConfigurationsServiceProxy, CreateOrEditApplicationConfigurationDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';




@Component({
    selector: 'createOrEditApplicationConfigurationModal',
    templateUrl: './create-or-edit-applicationConfiguration-modal.component.html'
})
export class CreateOrEditApplicationConfigurationModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    applicationConfiguration: CreateOrEditApplicationConfigurationDto = new CreateOrEditApplicationConfigurationDto();




    constructor(
        injector: Injector,
        private _applicationConfigurationsServiceProxy: ApplicationConfigurationsServiceProxy
    ) {
        super(injector);
    }
    
    show(applicationConfigurationId?: number): void {
    

        if (!applicationConfigurationId) {
            this.applicationConfiguration = new CreateOrEditApplicationConfigurationDto();
            this.applicationConfiguration.id = applicationConfigurationId;


            this.active = true;
            this.modal.show();
        } else {
            this._applicationConfigurationsServiceProxy.getApplicationConfigurationForEdit(applicationConfigurationId).subscribe(result => {
                this.applicationConfiguration = result.applicationConfiguration;



                this.active = true;
                this.modal.show();
            });
        }
        
        
    }

    save(): void {
            this.saving = true;
            
			
			
            this._applicationConfigurationsServiceProxy.createOrEdit(this.applicationConfiguration)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }













    close(): void {
        this.active = false;
        this.modal.hide();
    }
    
     ngOnInit(): void {
        
     }    
}
