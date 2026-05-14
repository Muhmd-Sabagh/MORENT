import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ILoginRequest, IRegisterRequest, IAuthResponse } from '../interfaces/auth';
import { IResult } from '../interfaces/result';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private apiUrl = `${environment.apiUrl}/auth`;
  
  // Holds the current user state for the UI to react to (e.g. hiding/showing login buttons)
  private currentUserSubject = new BehaviorSubject<IAuthResponse | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {
    this.loadUserFromStorage();
  }

  public get currentUserValue(): IAuthResponse | null {
    return this.currentUserSubject.value;
  }

  login(request: ILoginRequest): Observable<IResult<IAuthResponse>> {
    // withCredentials: true is CRITICAL for the backend to set the HttpOnly cookie!
    return this.http.post<IResult<IAuthResponse>>(`${this.apiUrl}/login`, request, { withCredentials: true })
      .pipe(tap(response => {
        if (response.isSuccess && response.dataObject) {
          this.setCurrentUser(response.dataObject);
        }
      }));
  }

  register(request: IRegisterRequest): Observable<IResult<IAuthResponse>> {
    return this.http.post<IResult<IAuthResponse>>(`${this.apiUrl}/register`, request, { withCredentials: true })
      .pipe(tap(response => {
        if (response.isSuccess && response.dataObject) {
          this.setCurrentUser(response.dataObject);
        }
      }));
  }

  refreshToken(): Observable<IResult<IAuthResponse>> {
    // We don't need to send a body. The browser automatically sends the HttpOnly 'refreshToken' cookie.
    return this.http.post<IResult<IAuthResponse>>(`${this.apiUrl}/refresh-token`, {}, { withCredentials: true })
      .pipe(tap(response => {
        if (response.isSuccess && response.dataObject) {
          this.setCurrentUser(response.dataObject);
        }
      }));
  }

  logout(): Observable<IResult<any>> {
    return this.http.post<IResult<any>>(`${this.apiUrl}/logout`, {}, { withCredentials: true })
      .pipe(tap(() => {
        localStorage.removeItem('currentUser');
        this.currentUserSubject.next(null);
      }));
  }

  private setCurrentUser(user: IAuthResponse): void {
    localStorage.setItem('currentUser', JSON.stringify(user));
    this.currentUserSubject.next(user);
  }

  private loadUserFromStorage(): void {
    const userJson = localStorage.getItem('currentUser');
    if (userJson) {
      this.currentUserSubject.next(JSON.parse(userJson));
    }
  }
}