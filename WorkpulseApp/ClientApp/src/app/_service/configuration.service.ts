import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class ConfigurationService {
    [x: string]: any;

    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

    }

    // Search user by username or email.
    getDebitMemoConfigurations() {
        return this.http.get<IApplicationConfiguration[]>(this.baseUrl + '/api/ApplicationConfiguration/GetDebitMemoConfigurations').pipe(
            retry(1), // retry a failed request up to 3 times
            catchError(this.handleError) // then handle the error
        )
    }

    // Search user by username or email.
    getMFCConfigurations() {
        return this.http.get<IApplicationConfiguration[]>(this.baseUrl + '/api/ApplicationConfiguration/GetMFCConfigurations').pipe(
            retry(1), // retry a failed request up to 3 times
            catchError(this.handleError) // then handle the error
        )
    }

    // Search user by username or email.
    getAOBConfigurations() {
        return this.http.get<IApplicationConfiguration[]>(this.baseUrl + '/api/ApplicationConfiguration/GetAOBConfigurations').pipe(
            retry(1), // retry a failed request up to 3 times
            catchError(this.handleError) // then handle the error
        )
    }

    createConfiguration(data: any) {
        return this.http.post(this.baseUrl + '/api/ApplicationConfiguration/CreateConfiguration', data).pipe(
            retry(1), // retry a failed request up to 1 time
            catchError(this.handleError) // then handle the error
        )
    }

    updateConfiguration(data: any) {
        return this.http.post(this.baseUrl + '/api/ApplicationConfiguration/UpdateConfiguration', data).pipe(
            retry(1), // retry a failed request up to 1 time
            catchError(this.handleError) // then handle the error
        )
    }

    deleteConfiguration(data: any) {
        return this.http.post(this.baseUrl + '/api/ApplicationConfiguration/DeleteConfiguration', data).pipe(
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

export interface IApplicationConfiguration {
    id: number;
    description: string
    value: string;    
}

