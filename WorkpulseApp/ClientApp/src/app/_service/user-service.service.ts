import { Injectable, Inject } from '@angular/core';
import 'devextreme/data/odata/store';
import DataSource from 'devextreme/data/data_source';

import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, retry, map } from 'rxjs/operators';
import { UserMangementAuditHistoryModel } from '../models/UserManagement/UserInfo';

@Injectable({
  providedIn: 'root'
})
export class UserServiceService {


  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

  }

  // Get data for DxDataGrid Sample Data
  getDataSource() {
    return new DataSource({
      store: {
        type: "odata",
        url: "https://js.devexpress.com/Demos/SalesViewer/odata/DaySaleDtoes",
        beforeSend: function (request) {
          request.params.startDate = "2018-05-10";
          request.params.endDate = "2018-05-15";

        }
      }
    });
  }

  // Search user by username or email.
  getSearchUser(search: string) {
    return this.http.get<UserInfo[]>(this.baseUrl + '/api/UserAdmin/SearchUsers?search=' + search).pipe(
      retry(1), // retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    )
  }

  // Search user by username or email.
  getAllUsers() {
    return this.http.get<Array<UserInfo>>(this.baseUrl + '/api/UserAdmin/GetAllUsers').pipe(
      retry(1), // retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    )
  }

  //Get logged in user info.
  getSignInUserInfo() {
    return this.http.get<UserInfo>(this.baseUrl + '/api/UserAdmin/GetCurrentUser').pipe(
      retry(1), // retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    )
  }

  //Get logged in user info.
  getSignInSecurityUserInfo() {
    return this.http.get<SecurityUserInfo>(this.baseUrl + '/api/UserAdmin/GetCurrentSecurityUser').pipe(
      retry(1), // retry a failed request up to 3 times
      catchError(this.handleError), // then handle the error
      map(user => {
        if (user != null) {
          sessionStorage.setItem('name', user.name);
          sessionStorage.setItem('userName', user.userName);
          sessionStorage.setItem('token', user.token);
          sessionStorage.setItem('userRoles', JSON.stringify(user.userRoles));
          sessionStorage.setItem('CORTNE_User', JSON.stringify(user));
        }
        return user;
      })
    )
  }

  //Get list of counties.
  getCounties(userName: string) {
    return this.http.get<Array<CountyModel>>(this.baseUrl + '/api/UserAdmin/GetCounties?userName=' + userName).pipe(
      retry(1), // retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    )
  }

  //Get list of roles.
  getRoles(userName: string) {
    return this.http.get<Array<RoleModel>>(this.baseUrl + '/api/UserAdmin/GetRoles?userName=' + userName).pipe(
      retry(1), // retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    )
  }
  //Get list of current user roles.
  getCurrentUserRoles(userName: string) {
    return this.http.get<Array<RoleModel>>(this.baseUrl + '/api/UserAdmin/GetCurrentUserRoles?userName=' + userName).pipe(
      retry(1), // retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    )
  }

  getUserHasRole(userName: string, roleId: string) {
    return this.http.get<boolean>(this.baseUrl + '/api/UserAdmin/GetUserHasRole?userName=' + userName + '&roleId=' + roleId).pipe(
      retry(1), // retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    )
  }

  searchUserInAD(userName: string) {
    return this.http.get<any>(this.baseUrl + '/api/UserAdmin/SearchUserInAD?userName=' + userName).pipe(
      retry(1), // retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    )
  }

  addNewUser(userName: string) {
    var data = new VMUpdateUserRole();
    data.userName = userName;
    data.updatedBy = sessionStorage.getItem('userName');

    return this.http.post<boolean>(this.baseUrl + '/api/UserAdmin/AddNewUser?userName=' + userName, data).pipe(
      retry(1), // retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    )
  }

  removeRoles(userName: string) {
    var data = new VMUpdateUserRole();
    data.userName = userName;
    data.updatedBy = sessionStorage.getItem('userName');

    return this.http.post<UserInfo>(this.baseUrl + '/api/UserAdmin/RemoveRoles?userName=' + userName, data).pipe(
      retry(1), // retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    )
  }

  saveUserRoles(userRoles: Array<RoleModel>, userName: string, userCounties: Array<CountyModel>) {
    var data = new VMUpdateUserRole();
    data.userName = userName;
    data.userCounties = userCounties;
    data.userRoles = userRoles;
    data.updatedBy = sessionStorage.getItem('userName');

    return this.http.post(this.baseUrl + '/api/UserAdmin/UpdateUserRole?userName=' + userName, data ).pipe(
      retry(1), // retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    )
  }

  // Get list of Components based on AspNetRoles
  getGroups(){
    return this.http.get<Array<string>>(this.baseUrl + '/api/UserAdmin/GetGroups').pipe(
      retry(1), // retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    )
  }

  // get the list of UserManagement_AuditHistory records
  getUserManagementAuditHistory(userMgmtAuditHistSearch: UserMangementAuditHistoryModel){
    return this.http.post<Array<UserMangementAuditHistoryModel>>(this.baseUrl + '/api/UserAdmin/SearchUserManagementAuditHistory',userMgmtAuditHistSearch).pipe(
      retry(1),// retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    )
  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    // return an observable with a user-facing error message
    return throwError(
      'Something bad happened; please try again later.');
  };

}

interface UserInfo {
  id: string;
  name: string;
  userName: string;
  email: string;
  phoneNumber: string;
}

export interface RoleModel {
  roleId: string;
  roleName: string;
  assigned: boolean;
  groupName: string
}
export class VMUpdateUserRole {
  userRoles: Array<RoleModel>;
  userCounties: Array<CountyModel>;
  userName: string;
  updatedBy: string;
}

export interface SecurityUserInfo {
  id: string;
  name: string;
  userName: string;
  email: string;
  phoneNumber: string;
  userRoles: RoleModel[];
  token: string;
}
export interface CountyModel {
  countyId: string;
  countyName: string;
  assigned: boolean;

}

