import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

import { UserServiceService } from '../_service/user-service.service';
import { SecurityProvider, SecurityUser, User } from '../SecurityProvider/SecurityProvider';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private userServiceService: UserServiceService, private securityProvider: SecurityProvider, private securityUser: SecurityUser) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // add authorization header with jwt token if available
    let userName = this.securityUser.getUserName();
    let token = this.securityUser.getToken();

    if (userName && token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(request);
  }
}
