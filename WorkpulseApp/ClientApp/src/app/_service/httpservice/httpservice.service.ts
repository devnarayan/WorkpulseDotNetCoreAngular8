import { Injectable, Inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class HttpserviceService {

private API_URL = environment;
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { 
  }

  getData(url):any {
    return this.http.get(this.baseUrl + url).pipe(
      retry(1), // retry a failed request up to 1 time
      catchError(this.handleError) // then handle the error
    )
   
  }

  postData(category: any, url?) {
    return this.http.post(this.baseUrl + url, category).pipe(
      retry(1), // retry a failed request up to 1 time
      catchError(this.handleError) // then handle the error
    )
  }

  deleteData(url,id){
    return this.http.delete(this.baseUrl + url + "/" + id).pipe(
      retry(1), // retry a failed request up to 1 time
      catchError(this.handleError) // then handle the error
    )

  }

  getCalenderData(url){
    return this.http.get(this.baseUrl + url).pipe(
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
