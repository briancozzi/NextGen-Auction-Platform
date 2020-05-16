import { Component, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'createEventModal',
    templateUrl: './create-event-modal.component.html'
})
export class CreateEventModalComponent extends AppComponentBase {

    @ViewChild('createModal', { static: true }) modal: ModalDirective;

    active = false;

    constructor(
        injector: Injector,
    ) {
        super(injector);
    }

    show() {
        this.active = true;
        this.init();
        this.modal.show();

    }

    onShown(): void {
    }

    init(): void {
       
    }
   
}
