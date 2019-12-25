import { Injectable } from "@angular/core";
import { UserServiceService, RoleModel } from "../_service/user-service.service";
import { async } from 'rxjs/internal/scheduler/async';

@Injectable()
export class SecurityProviderMock {
  name: string;
  userName: string;
  userRoles: RoleModel[];

  user: User;

  constructor(private service: UserServiceService) {
    this.user = new User();
  }

  loadUserInfo() {
   
  }

  loadUser = () => {
   }

  loadUserRoles = (userName: string) => {
   
  }

  saveSessionUserModel = () => {
    sessionStorage.setItem('CORTNE_User', JSON.stringify(this.user));
  }

  clearSessionUserModel = () => {
    sessionStorage.removeItem('CORTNE_User');
  }
}

export class User {
  name: string;
  userName: string;
  userRoles: RoleModel[];
}

export class SecurityUserMock {
  user = new User();

  constructor() {
   
  }

  getSessionUserModel = () => {
   
  }

  getName = () => {
    return 'John Doe';
  }

  getUserName = () => {
    return 'John.Doe@flhealth.gov';
  }

  hasRoleByID = (id: string) => {

  }

  hasRoleByName = (roleName: string) => {

  }

  IsSystemAdmin = () => {
    return false;
  }

  IsAOBAdmin = () => {
    return false;
  }

  IsAOBUser = () => {
    return false;
  }

  IsBudgetAnalyst = () => {
    return false;
  }

  IsDOAdmin = () => {
    return false;
  }

  IsDODirector = () => {
    return false;
  }

  IsDOLockUnlock = () => {
    return false;
  }

  IsARSAdmin = () => {
    return false;
  }

  IsARSUser = () => {
    return false;
  }

  IsDebitMemoAdmin = () => {
    return false;
  }

  IsDebitMemoCommenter = () => {
    return false;
  }

  IsDebitMemoUser = () => {
    return true;
  }

  IsDebitMemoViewer = () => {
    return false;
  }

  IsDPRAdmin = () => {
    return false;
  }

  IsDPRUser = () => {
    return false;
  }

  IsLogManagement = () => {
    return false;
  }

  IsUserManagement = () => {
    return false;
  }

  IsMFCAdmin = () => {
    return false;
  }

  IsMFCUser = () => {
    return false;
  }

  IsOCAApprover = () => {
    return false;
  }

  IsOCARequester = () => {
    return false;
  }

  IsReportViewer = () => {
    return false;
  }

  IsCORTNEDev = () => {
    return false;
  }

  IsEmployee = () => {
    return false;
  }

}
