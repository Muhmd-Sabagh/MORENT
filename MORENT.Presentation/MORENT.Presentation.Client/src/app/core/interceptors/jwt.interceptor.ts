import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, filter, switchMap, take } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

  constructor(private authService: AuthService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // 1. Attach JWT Access Token to all outbound requests
    const currentUser = this.authService.currentUserValue;
    if (currentUser && currentUser.token) {
      request = this.addTokenHeader(request, currentUser.token);
    }

    // Ensure cross-origin requests send the refresh cookie
    request = request.clone({
      withCredentials: true 
    });

    // 2. Handle Responses
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        // If we get a 401 Unauthorized, the token expired. Time to refresh!
        if (error.status === 401 && currentUser) {
          return this.handle401Error(request, next);
        }
        return throwError(() => error);
      })
    );
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      // Call the refresh token endpoint
      return this.authService.refreshToken().pipe(
        switchMap((response) => {
          this.isRefreshing = false;
          this.refreshTokenSubject.next(response.dataObject.token);
          
          // Retry the original request with the new token!
          return next.handle(this.addTokenHeader(request, response.dataObject.token));
        }),
        catchError((err) => {
          this.isRefreshing = false;
          // If refresh fails (e.g. cookie expired), log the user out
          this.authService.logout().subscribe();
          return throwError(() => err);
        })
      );
    } else {
      // If a refresh is already in progress, wait for it to finish then retry
      return this.refreshTokenSubject.pipe(
        filter(token => token !== null),
        take(1),
        switchMap((token) => next.handle(this.addTokenHeader(request, token)))
      );
    }
  }

  private addTokenHeader(request: HttpRequest<any>, token: string) {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }
}