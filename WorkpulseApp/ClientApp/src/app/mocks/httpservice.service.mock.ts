import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { InjectionToken } from '@angular/core';
export const BASE_URL = new InjectionToken<string>('BASE_URL');

@Injectable({
  providedIn: 'root'
})
export class HttpserviceServiceMock {

  constructor(private http: HttpClient) {
  }

  getData(url): any {
    return this.http.get(BASE_URL + url).pipe(
      retry(1), // retry a failed request up to 1 time
      catchError(this.handleError) // then handle the error
    )

  }

  postData(category: any, url?) {
    return this.http.post(BASE_URL + url, category).pipe(
      retry(1), // retry a failed request up to 1 time
      catchError(this.handleError) // then handle the error
    )
  }

  deleteData(url, id) {
    return this.http.delete(BASE_URL + url + "/" + id).pipe(
      retry(1), // retry a failed request up to 1 time
      catchError(this.handleError) // then handle the error
    )

  }

  getCalenderData(url) {
    return this.http.get(BASE_URL + url).pipe(
      retry(1), // retry a failed request up to 1 time
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
      //console.error(
      //  `Backend returned code ${error.status}, ` +
      //  `body was: ${error.error}`);
      return throwError(error.message);
    }
    // return an observable with a user-facing error message
    return throwError(
      'Something bad happened; please try again later.');
  };

}
