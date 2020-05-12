import { Component, EventEmitter, Injector, Output, ViewChild, OnInit, ElementRef } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as _ from 'lodash';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import {
    AppAccountServiceProxy,
    CreateAppAccountDto,
    CountryServiceProxy,
    AddressDto,
    UserServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';
import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';
import { IAjaxResponse, TokenService } from 'abp-ng2-module';
import {forkJoin} from "rxjs";

@Component({
    selector: 'createAccountsModal',
    templateUrl: './create-accounts-modal.component.html'
})
export class CreateAccountsModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('inputFile') inputFile: ElementRef;
    @ViewChild('createModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    logoUploader: FileUploader;
    remoteServiceBaseUrl = AppConsts.remoteServiceBaseUrl;
    active = false;
    saving = false;
    account: CreateAppAccountDto = new CreateAppAccountDto();
    countryList = [];
    stateList = [];
    userList = [];
    stateDropdown = true;
    uploadedFiles: any[] = [];
    logo: string;
    isSelected = true;

    constructor(
        injector: Injector,
        private _accountsService: AppAccountServiceProxy,
        private _countryService: CountryServiceProxy,
        private _tokenService: TokenService,
        private _userService: UserServiceProxy
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this.initUploaders();
    }
    createUploader(url: string ,success?: (result: any) => void): FileUploader {
        
        const uploader = new FileUploader({ url: AppConsts.remoteServiceBaseUrl + url,

           });

        uploader.onAfterAddingFile = (file) => {
            file.withCredentials = false;
        };
        uploader.onBuildItemForm = (fileItem: any, form: any) => {
            form.append('createAppAccountDto', JSON.stringify(this.account)); 
            form.append('isCreated', true);
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
                if (result.status) {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modalSave.emit(null);
                }
            }
        );
    }
    show() {
        this.active = true;
        this.init();
        this.modal.show();
    }

    onShown(): void {
    }
    clearLogo(): void {
        this.inputFile.nativeElement.value = '';
    }
    init(): void {
        this.account = new CreateAppAccountDto();
        this.account.address = new AddressDto();
        forkJoin([
            this._countryService.getCountriesWithState(),
            this._userService.getUsers(undefined,undefined,undefined,undefined,undefined,undefined,undefined)
            // this._userService.getUsers(null,null,2,false,null,1000,0),
        ]).subscribe(result => {
            this.countryList = result[0];
            this.account.address.countryUniqueId = result[0][0].countryUniqueId;
            this.loadStateList(this.account.address.countryUniqueId);
            this.userList = result[1].items;
        });
    }
    loadStateList(countryId): void {
        this.stateDropdown = false;
        this.stateList = this.countryList.find(x => x.countryUniqueId === countryId);
    }

    save(): void {
        this.saving = true;
        this.logoUploader.uploadAll();
        if(this.inputFile.nativeElement.value !=""){
            this.logoUploader.uploadAll();
        }
        else{
            this.saveAccount();
        }
    }
    saveAccount(): void {
        this._accountsService.create(this.account)
            .pipe(finalize(() => this.saving = false))
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
   }