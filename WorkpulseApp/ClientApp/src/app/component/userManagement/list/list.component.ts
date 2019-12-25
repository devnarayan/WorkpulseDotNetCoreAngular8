import { Component, OnInit, Inject } from '@angular/core';
import {
  DxDataGridModule,
  DxBulletModule,
  DxTemplateModule
} from 'devextreme-angular';
import DataSource from 'devextreme/data/data_source';
import { UserServiceService, RoleModel, CountyModel } from '../../../_service/user-service.service';
import { MyErrorStateMatcher } from '../../../_common/MyErrorStateMatcher';
import { FormControl, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { NgModule } from '@angular/core';
import { ErrorStateMatcher } from '@angular/material/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ConfirmDialog } from '../../shared/confirm.dialog';
import { fadeInContent } from '@angular/material/select';
import { NavigationStart, Router } from '@angular/router';
import { filter } from 'rxjs/operators';
import { Observable, Subject } from 'rxjs';
import { SecurityUser } from '../../../SecurityProvider/SecurityProvider';
//import { IDropdownSettings } from 'ng-multiselect-dropdown';
@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss'],
  providers: [UserServiceService]
})

export class UserManagementListComponent implements OnInit {
  vm: any;
  canReadWriteUserManagement: boolean = false;

  constructor(private service: UserServiceService, public dialog: MatDialog, private user: SecurityUser) {
    this.vm = this;
    this.canReadWriteUserManagement = this.user.CanReadWriteUserManagement();
  
  }
  dropdownSettings = {};
  ngOnInit() {
   // this.serchFormControl.setValue("fl");
    //this.searchUsers();
  }

  serchFormControl = new FormControl('', [
    //Validators.required,
  ]);

  matcher = new MyErrorStateMatcher();

  searchUsers = () => {

    this.service.getSearchUser(this.serchFormControl.value).subscribe(result => {
      var userInfo = result;
      this.userDataSource = new DataSource(result);
    }, error => console.error(error));

  }

  userDataSource: DataSource;
  collapsed = false;
  contentReady = (e) => {
    if (!this.collapsed) {
      this.collapsed = true;
      e.component.expandRow(["EnviroCare"]);
    }
  };
  customizeTooltip = (pointsInfo) => {
    return { text: parseInt(pointsInfo.originalValue) + "%" };
  }

  selectedRows: number[];


  selectionChangedHandler() {
    console.log(this.selectedRows);
  }

  editUserRoleClick(userName): void {
    const dialogRef = this.dialog.open(EditUserUserDialog, {
      data: { userName: userName }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      console.log("result", result);
      this.name = result;
    });
  }

  deactivateUserClick(userName) {
    const dialogRef = this.vm.dialog.open(ConfirmDialog, {
      width: '350px',
      data: { title: 'Confirm to remove all roles?' }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result == true) {
        this.service.removeRoles(userName).subscribe(result => {
          var userInfo = result;
        }, error => console.error(error));
      }
    });
  }

  name: string;

  addUserDialog(): void {
    const dialogRef = this.dialog.open(AddUserDialog, {
      width: '350px',
      data: { name: this.name, userName: this.name }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      console.log("result", result);
      this.name = result;
    });
  }


}


@Component({
  selector: 'add-user-dialog',
  templateUrl: 'add-user-dialog.html',
})
export class AddUserDialog {
  message: string;
  addDisabled: boolean = true;
  canReadWriteUserManagement: boolean = false;
  constructor(
    public dialogRef: MatDialogRef<AddUserDialog>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData, private service: UserServiceService, private user: SecurityUser ) {

    this.canReadWriteUserManagement = this.user.CanReadWriteUserManagement();
   
  }
  emailFormControl = new FormControl('', [
    Validators.required,
    Validators.email,
  ]);

  matcher = new MyErrorStateMatcher();

  searchUsers(): void {
    this.service.searchUserInAD(this.emailFormControl.value).subscribe(result => {
      console.log(result);
      var userInfo = result;
      if (userInfo.success == true) {
        this.addDisabled = false;
        this.message = userInfo.message;
      }
      else {
        this.message = userInfo.message;
        this.addDisabled = true;
      }
    }, error => console.error(error));
  }

  addUserClick(): void {
    this.service.addNewUser(this.emailFormControl.value).subscribe(result => {
      var userInfo = result;
      if (result == true) {
        this.message = "New user added successfully.";
      }
      else{
        this.message = "Please try agian.";
      }
    }, error => console.error(error));
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}


@Component({
  selector: 'edit-user-role-dialog',
  templateUrl: 'edit-user-role-dialog.html',
})
export class EditUserUserDialog {
  message: string;
  addDisabled: boolean = false;
  isAdmin: boolean = false;
  isDebitmemoAdmin: boolean = false;
  isCashReceiptAdmin: boolean = false;
  isDebitmemoMFQAdmin: boolean = false;
  isMfcAdmin: boolean = false;
  isDprAdmin: boolean = false;
  isArsAdmin: boolean = false;
  isAobAdmin: boolean = false;
  canReadWriteUserManagement: boolean = false;


  userRoles: RoleModel[];
  saveRoles: RoleModel[];
  currentUserRoles: RoleModel[];
  userCounties: CountyModel[];
  selectedCounties: CountyModel[];
  dropdownSettings = {};
  constructor(
    public dialogRef: MatDialogRef<EditUserUserDialog>, @Inject(MAT_DIALOG_DATA) public data: DialogData, private service: UserServiceService, private user: SecurityUser) {
    this.canReadWriteUserManagement = this.user.CanReadWriteUserManagement();
    
    this.dropdownSettings = {
      singleSelection: false,
      idField: 'countyId',
      textField: 'countyName',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 15,
      allowSearchFilter: true
    };

    this.service.getRoles(data.userName).subscribe(result => {
      this.userRoles = result;
      

      console.log("Roles ", this.userRoles);
    }, error => console.error(error));

   this.service.getCounties(data.userName).subscribe(result => {
     this.userCounties = result;
     this.selectedCounties = this.userCounties.filter(function (county) {
       return county.assigned;
     });
     console.log("Countiess ", this.userCounties);
    }, error => console.error(error));

    var editingUser = this.user.getUserName();    
    this.service.getCurrentUserRoles(editingUser).subscribe(result => {
      this.currentUserRoles = result;
      this.isAdmin = this.currentUserRoles.some(item => item.roleName == 'System Admin' && item.assigned == true);
      this.isDebitmemoAdmin = this.currentUserRoles.some(item => (item.roleName == 'Debit Memo Admin' && item.assigned == true));
      this.isCashReceiptAdmin = this.currentUserRoles.some(item => (item.roleName == 'CR Admin' && item.assigned == true));
      this.isMfcAdmin = this.currentUserRoles.some(item => (item.roleName == 'MFC Admin' && item.assigned == true));
      this.isDprAdmin = this.currentUserRoles.some(item => (item.roleName == 'DPR Admin' && item.assigned == true));
      this.isArsAdmin = this.currentUserRoles.some(item => (item.roleName == 'ARS Admin' && item.assigned == true));
      this.isAobAdmin = this.currentUserRoles.some(item => (item.roleName == 'AOB Admin' && item.assigned == true));
      this.isDebitmemoMFQAdmin = this.currentUserRoles.some(item => (item.roleName == 'Debit Memo MQA Admin' && item.assigned == true));
      

      console.log("Roles ", this.userRoles);
    }, error => console.error(error));
   
  }

  requiredCounty(): boolean {
    console.log(this.userRoles);
    var systemAdminRoles = this.userRoles.filter(
      r => r.groupName === 'Admin' && r.roleName === 'System Admin')[0];
    var debitMemoAdminRoles = this.userRoles.filter(
      r => r.groupName === 'Debit Memo' && r.roleName === 'Debit Memo Admin')[0];
    var debitMemoCommenterRoles = this.userRoles.filter(
      r => r.groupName === 'Debit Memo' && r.roleName === 'Debit Memo Commenter')[0];
    var debitMemoUserRoles = this.userRoles.filter(
      r => r.groupName === 'Debit Memo' && r.roleName === 'Debit Memo User')[0];
    var debitMemoViewerRoles = this.userRoles.filter(
      r => r.groupName === 'Debit Memo' && r.roleName === 'Debit Memo Viewer')[0];

    return (!systemAdminRoles.assigned && !debitMemoAdminRoles.assigned && !debitMemoCommenterRoles.assigned && !debitMemoUserRoles.assigned  && debitMemoViewerRoles.assigned);
  }


  updateUserRoleClick(): void {
    console.log(this.userRoles);
    //debugger;
    //alert(this.selectedCounties);
    this.service.saveUserRoles(this.userRoles, this.data.userName, this.selectedCounties).subscribe(result => {
      var userInfo = result;
    }, error => {
      console.error(error);
    });
    this.dialogRef.close();
    alert("Successfully Saved User Roles");
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}


export interface DialogData {
  userName: string;
  name: string;
}

