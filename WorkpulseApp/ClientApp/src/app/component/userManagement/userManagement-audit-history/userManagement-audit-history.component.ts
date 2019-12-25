import { Component, OnInit, Inject, ChangeDetectorRef, ViewChild } from '@angular/core';
import DataSource from 'devextreme/data/data_source';
import { HttpserviceService } from '../../../_service/httpservice/httpservice.service';
import { FormBuilder, FormControl } from '@angular/forms';
import { AlertService } from '../../alert';
import { Router } from '@angular/router';
import { UserServiceService } from '../../../_service/user-service.service';
import { MatDialog } from '@angular/material';
import { SecurityUser } from '../../../SecurityProvider/SecurityProvider';
import { UserMangementAuditHistoryModel } from  '../../../models/UserManagement/UserInfo';
import { DxDataGridComponent } from 'devextreme-angular';
import { DatePipe } from '@angular/common';
import { reject, resolve } from 'q';


@Component({
    selector: 'user-management-audit-history',
    templateUrl: './userManagement-audit-history.component.html',
    styleUrls: ['./userManagement-audit-history.component.scss']
})

export class UserManagementAuditHistoryComponent implements OnInit{
    userName: string;
    userMgmtAuditHistList: Array<UserMangementAuditHistoryModel>;
    startDate = new FormControl(new Date()); 
    endDate = new FormControl(new Date()); 
    componentList: string[];
    selectedComponent: string;
    userMgmtAuditHistDataSource: DataSource;


    

    @ViewChild('dataGridRef', { static: false }) dataGrid: DxDataGridComponent;

    constructor(private httpservice: HttpserviceService, private formBuilder: FormBuilder, @Inject('BASE_URL') private baseUrl: string, private cdr: ChangeDetectorRef,
    private alertService: AlertService, private router: Router, private userService: UserServiceService, public dialog: MatDialog, private user: SecurityUser) { }

  ngOnInit() {
    this.userName = "";
    this.getListOfGroups();
    this.searchData();
  }

  // Get List of Components
  getListOfGroups = () => {
  // getListOfGroups(){
  //   this.componentList = ["Admin","Debit Memo"];
    return new Promise(async resolve => {
      var item1 = await this.userService.getGroups().toPromise().then(result => {
        this.componentList = result;
        resolve(true);
      }, error => {
        console.log('Error :'+ error);
        this.alertService.warn("Error Occured While Loading Debit Memos: " + String(error));
        reject(false);
      })
    });
  }

  searchData(): void{
    var searchValues = new UserMangementAuditHistoryModel();

    if(this.startDate != null && this.endDate != null){
      searchValues.modifiedStartDate = this.getFormattedDate(this.startDate.value);
      searchValues.modifiedEndDate = this.getFormattedDate(this.endDate.value);
    }
    searchValues.componentName = this.selectedComponent;
    searchValues.userName = this.userName;
    this.userService.getUserManagementAuditHistory(searchValues).subscribe(result => {
      this.userMgmtAuditHistList = result;
      this.userMgmtAuditHistDataSource = new DataSource(this.userMgmtAuditHistList);
      console.log(result);
    }, error =>{
      console.log('Error :' + error);
        this.alertService.warn("Error Occured While Loading UserManagementAuditHistory: " + String(error));
    });
    
  }

  addDays(date: Date, days: number): Date {
    console.log('adding ' + days + ' days');
    console.log(date);
    date.setDate(date.getDate() + Number(days));
    console.log(date);
    return date;
  }

  getFormattedDate(date) {
    var datePipe = new DatePipe("en-US");
    return datePipe.transform(date, 'MM/dd/yyyy');
  }

  getDateformatwithTime() {
    var datePipe = new DatePipe("en-US");
    return datePipe.transform(new Date(), 'MM-dd-yyyy hh:mm:ss');
  }

}