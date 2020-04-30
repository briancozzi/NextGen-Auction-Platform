import{ Component, Injector, ViewChild,Output,EventEmitter,ElementRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { AppAccountServiceProxy,CountryServiceProxy,AppAccountDto,AddressDto, UpdateAppAccountDto } from '@shared/service-proxies/service-proxies';
import {forkJoin} from "rxjs";
import { AppConsts } from '@shared/AppConsts';
import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
@Component({
    selector: 'editAccountsModal',
    templateUrl: './edit-accounts-modal.component.html'
})
export class EditAccountsModalComponent extends AppComponentBase{
    @ViewChild('editModal', {static : true}) modal: ModalDirective;
    @Output () modalSave: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('inputFile') inputFile: ElementRef;

    logoUploader: FileUploader;
    webHostUrl = AppConsts.remoteServiceBaseUrl;
    account: UpdateAppAccountDto = new UpdateAppAccountDto();
    saving = false;
    active= false;
    stateList =[]; 
    countryList = [];
    countryUniqueId:string;
    stateUniqueId:string;
    isLogo = false;
    constructor(
        injector: Injector,
        private _accountService: AppAccountServiceProxy,
        private _countryService: CountryServiceProxy,
        private _tokenService: TokenService
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this.initUploaders();
    }
    createUploader(url: string, success?: (result: any) => void): FileUploader {
        const uploader = new FileUploader({ url: AppConsts.remoteServiceBaseUrl + url });

        uploader.onAfterAddingFile = (file) => {
            file.withCredentials = false;
        };

        uploader.onSuccessItem = (item, response, status) => {
            const ajaxResponse = <IAjaxResponse>JSON.parse(response);
            if (ajaxResponse.success) {
                if (success) {
                    success(ajaxResponse.result);
                }
            } else {
                this.message.error(ajaxResponse.error.message);
            }
        };

        const uploaderOptions: FileUploaderOptions = {};
        uploaderOptions.authToken = 'Bearer ' + this._tokenService.getToken();
        uploaderOptions.removeAfterUpload = true;
        uploader.setOptions(uploaderOptions);
        return uploader;
    }
    initUploaders(): void {
        this.logoUploader = this.createUploader(
            '/AppAccounts/UploadLogo',
            result => {
                this.account.logo = result.path;
                this.updateAccount();
            }
        );
    }
    show(AccountId?: string):void{
        debugger;
        this.active = true;
        this.account = new AppAccountDto();
        this.account.address = new AddressDto();
        forkJoin([
            this._countryService.getCountriesWithState(),
            this._accountService.getAccountById(AccountId)
          ]).subscribe(allResults =>{
            this.countryList = allResults[0];
            this.account = allResults[1];
            this.account.address = allResults[1].address;
            this.stateList = this.countryList.find(x=>x.countryUniqueId === this.account.address.countryUniqueId);
            if(this.account.logo != '' && this.account.logo !=null){
                this.isLogo = true;
            }
            this.modal.show();
        });
          
    }
    clearLogo():void{
        this.isLogo = false;
        this.account.logo = "";
    }
    
    close(): void {
        this.active = false;
        this.modal.hide();
    }
    save(): void {
        if(!this.isLogo){
            if(this.inputFile.nativeElement.value !=""){
                this.logoUploader.uploadAll();
            }
            else{
                this.updateAccount();
            }
        }
        else{
            this.updateAccount();
        }
    }
    updateAccount():void{
        this._accountService.update(this.account)
            .pipe(finalize(() => this.saving = false))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }
}