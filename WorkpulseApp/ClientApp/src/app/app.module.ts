import { NgModule, Component, Pipe, PipeTransform, enableProdMode, Inject, CUSTOM_ELEMENTS_SCHEMA, APP_INITIALIZER } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule, ɵgetDOM } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule, FormControl, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent} from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material.module';
import { MatFormFieldModule, MatInputModule, MatAutocompleteModule, MatButtonModule } from '@angular/material';
import {
    DxDataGridModule,
    DxBulletModule,
    DxTemplateModule,
    DxSelectBoxModule,
    DxCheckBoxModule
} from 'devextreme-angular';
import DataSource from 'devextreme/data/data_source';
import { UserServiceService } from './_service/user-service.service';
import { HttpserviceService } from './_service/httpservice/httpservice.service';
import { UserServiceServiceMock } from './mocks/user-service.service.mock';
import { HttpserviceServiceMock } from './mocks/httpservice.service.mock';
import { SecurityProviderMock, SecurityUserMock } from './mocks/SecurityProvider.mock';
import { LeftNavMenuComponent } from './left-nav-menu/left-nav-menu.component';
import { TopNavMenuComponent } from './top-nav-menu/top-nav-menu.component';
import { ConfirmDialog } from './component/shared/confirm.dialog';
import { HomeComponent } from './home/home.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { UserManagementListComponent, AddUserDialog, EditUserUserDialog } from './component/userManagement/list/list.component';
import { UserManagementAuditHistoryComponent } from './component/userManagement/userManagement-audit-history/userManagement-audit-history.component';

import { AlertModule } from './component/alert/alert.module';
import { MatNativeDateModule } from '@angular/material/core';
import { AutocompleteLibModule } from 'angular-ng-autocomplete';
import { ErrorInterceptor } from '../app/_helpers/error.interceptor';
import { JwtInterceptor } from '../app/_helpers/jwt.interceptor';
import { NgxMaskModule } from 'ngx-mask'
//import { MatDatepickerModule } from '@angular/material/datepicker';
//import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { TextMaskModule } from 'angular2-text-mask';
import { SecurityProvider, SecurityUser } from './SecurityProvider/SecurityProvider';
import { async } from '@angular/core/testing';
import { QlikReportComponent } from './component/qlik-report/qlik-report.component';
import { PrintLayoutComponent } from './print-layout/print-layout.component';
import { PrintService } from './_service/print.service';
import { ConfigurationService } from './_service/configuration.service';

@NgModule({
    entryComponents: [
        ConfirmDialog,
        AddUserDialog,
        EditUserUserDialog],
    declarations: [
        AppComponent,
        HomeComponent,
        DashboardComponent,
        LeftNavMenuComponent,
        TopNavMenuComponent,
        UserManagementListComponent,
        ConfirmDialog,
        AddUserDialog,
        EditUserUserDialog,
        QlikReportComponent,
        PrintLayoutComponent,
        UserManagementAuditHistoryComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        FormsModule,
        BrowserAnimationsModule,
        MaterialModule,
        ReactiveFormsModule,
        AlertModule,
        DxDataGridModule,
        DxTemplateModule,
        DxBulletModule,
        DxSelectBoxModule,
        DxCheckBoxModule,
        MatInputModule,
        MatAutocompleteModule,
        MatFormFieldModule,
        MatButtonModule,
        MaterialModule,
        MatNativeDateModule,
        ReactiveFormsModule,
        AutocompleteLibModule,
        TextMaskModule,
        NgxMaskModule.forRoot()
    ],
    providers: [UserServiceService,
        HttpserviceService,
        UserServiceServiceMock,
        HttpserviceServiceMock,
        SecurityProvider,
        SecurityUser,
        SecurityProviderMock,
        PrintService,
        ConfigurationService,
        SecurityUserMock,
        { provide: APP_INITIALIZER, useFactory: SecurityProviderFactory, deps: [SecurityProvider], multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },],
    bootstrap: [AppComponent],
    schemas: [CUSTOM_ELEMENTS_SCHEMA],

})
export class AppModule { }

export function SecurityProviderFactory(provider: SecurityProvider) {
    return () => provider.loadUserInfo();
}


export enum EnumContainerModel {
  Container = 0,
  CashLogContainer = 1,
  CashReceiptContainer = 2,
  DepositContainer = 3,
  TransferContainer = 4,
  AccountCodeContainer = 5
}
