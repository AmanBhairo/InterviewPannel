import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { LocalstorageService } from './helpers/localstorage.service';
import { LocalStorageKeys } from './helpers/localstoragekeys';
import { ApiResponse } from '../models/ApiResponse{T}';
import { ChangePassword } from '../models/changepassword.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = "http://localhost:5263/api/Auth/"
  private authState = new BehaviorSubject<boolean>(this.localStorageHelper.hasItem(LocalStorageKeys.TokenName));
  private usernameSubject = new BehaviorSubject<string | null | undefined>(this.localStorageHelper.getItem(LocalStorageKeys.LoginId));
  private userIdSubject = new BehaviorSubject<string | null | undefined>(this.localStorageHelper.getItem(LocalStorageKeys.UserId));
  
  constructor(private http: HttpClient, private localStorageHelper: LocalstorageService) { }
  signIn(username: string, password: string): Observable<ApiResponse<string>> {
    const body = { username, password };
    return this.http.post<ApiResponse<string>>(this.apiUrl + 'Login', body).pipe(
      tap(response => {
        if (response.success) {
          const token = response.data;
          const payload = token.split('.')[1];
          const decodedPayload = JSON.parse(atob(payload));
          const userId = decodedPayload.EmployeeId;
          this.localStorageHelper.setItem(LocalStorageKeys.TokenName, token);
          this.localStorageHelper.setItem(LocalStorageKeys.LoginId, username);
          this.localStorageHelper.setItem(LocalStorageKeys.UserId, userId);
          this.authState.next(this.localStorageHelper.hasItem(LocalStorageKeys.TokenName));
          this.usernameSubject.next(username);
          this.userIdSubject.next(userId);
        }
      })
    );
  }
  signOut() {
    this.localStorageHelper.removeItem(LocalStorageKeys.TokenName);
    this.localStorageHelper.removeItem(LocalStorageKeys.LoginId);
    this.localStorageHelper.removeItem(LocalStorageKeys.UserId);
    this.authState.next(false);
    this.usernameSubject.next(null);
    this.userIdSubject.next(null);
  }
  isAuthenticated() {
    return this.authState.asObservable();
  }
  getUsername(): Observable<string | null | undefined> {
    return this.usernameSubject.asObservable();
  }
  getUserId(): Observable<string | null | undefined> {
    return this.userIdSubject.asObservable();
  }
  changePassword(changePassword: ChangePassword): Observable<ApiResponse<string>> {
    return this.http.put<ApiResponse<string>>(this.apiUrl + 'ChangePassword', changePassword)
  }
}
