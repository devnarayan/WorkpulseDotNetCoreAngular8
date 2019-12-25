import { Injectable, Inject } from '@angular/core';
import 'devextreme/data/odata/store';
import DataSource from 'devextreme/data/data_source';

import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { InjectionToken } from '@angular/core';
export const BASE_URL = new InjectionToken<string>('BASE_URL');
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserServiceServiceMock {

  constructor(private http: HttpClient) {

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
    return this.http.get<UserInfo[]>(BASE_URL + '/api/UserAdmin/SearchUsers?search=' + search).pipe(
      retry(1), // retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    )
  }

  //Get logged in user info.
  getSignInUserInfo() {
    return of({
      id: 'ABCD-EFGH-IJKL-MNOP',
      name: 'John Doe',
      userName: 'John.Doe'});
  }

  //Get list of roles.
  getRoles(userName: string) {
    return this.http.get<Array<RoleModel>>(BASE_URL + '/api/UserAdmin/GetRoles?userName=' + userName).pipe(
      retry(1), // retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    )
  }

  getUserHasRole(userName: string, roleId: string) {
    return this.http.get<boolean>(BASE_URL + '/api/UserAdmin/GetUserHasRole?userName=' + userName + '&roleId=' + roleId).pipe(
      retry(1), // retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    )
  }

  searchUserInAD(userName: string) {
    return this.http.get<any>(BASE_URL + '/api/UserAdmin/SearchUserInAD?userName=' + userName).pipe(
      retry(1), // retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    )
  }

  addNewUser(userName: string) {
    return this.http.get<boolean>(BASE_URL + '/api/UserAdmin/AddNewUser?userName=' + userName).pipe(
      retry(1), // retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    )
  }

  removeRoles(userName: string) {
    return this.http.get<UserInfo>(BASE_URL + '/api/UserAdmin/RemoveRoles?userName=' + userName).pipe(
      retry(1), // retry a failed request up to 3 times
      catchError(this.handleError) // then handle the error
    )
  }

  saveUserRoles(userRoles: Array<RoleModel>, userName: string) {
    console.log(userRoles);
    return this.http.post(BASE_URL + '/api/UserAdmin/UpdateUserRole?userName=' + userName, userRoles).pipe(
      retry(1), // retry a failed request up to 3 times
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
}

export interface RoleModel {
  roleId: string;
  roleName: string;
  assigned: boolean;
  groupName: string
}

