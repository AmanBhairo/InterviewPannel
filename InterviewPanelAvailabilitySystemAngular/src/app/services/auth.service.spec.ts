import { TestBed } from '@angular/core/testing';

import { AuthService } from './auth.service';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ApiResponse } from '../models/ApiResponse{T}';
import { ChangePassword } from '../models/changepassword.model';
import { LocalstorageService } from './helpers/localstorage.service';
import { BehaviorSubject } from 'rxjs';
import { LocalStorageKeys } from './helpers/localstoragekeys';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock : HttpTestingController;
  let localStorageHelper: Partial<LocalstorageService>;
  let authStateSubject: BehaviorSubject<boolean>;
  let usernameSubject: BehaviorSubject<string | null | undefined>;
  let userIdSubject: BehaviorSubject<string | null | undefined>;

  beforeEach(() => {
    localStorageHelper = {
      removeItem: jasmine.createSpy(),
      getItem: jasmine.createSpy(),
      hasItem: jasmine.createSpy().and.returnValue(true),
    };

    authStateSubject = new BehaviorSubject<boolean>(true); 
    usernameSubject = new BehaviorSubject<string | null | undefined>('testUser');
    userIdSubject = new BehaviorSubject<string | null | undefined>('testUserId');


    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, HttpClientTestingModule],
      providers:[AuthService,  { provide: LocalstorageService, useValue: localStorageHelper }]
    });
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
    service = TestBed.inject(AuthService);
    service['authState'] = authStateSubject; 
    service['usernameSubject'] = usernameSubject; 
    service['userIdSubject'] = userIdSubject;
  });

  afterEach(()=>{
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });


  //change password
  it('should change password successfully', () => {
    // Arrange
    const mockChangePassword: ChangePassword = {
      email: undefined,
      oldPassword: '',
      newPassword: '',
      newConfirmPassword: ''
    };
    const mockApiResponse = { success: true, data: '', message: 'Password changed successfully' };

    // Act
    service.changePassword(mockChangePassword).subscribe(response => {
      // Assert
      expect(response).toEqual(mockApiResponse);
    });
    const req = httpMock.expectOne('http://localhost:5263/api/Auth/ChangePassword');
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(mockChangePassword);
    req.flush(mockApiResponse);
  });
  

  //signOut
  it('should sign out and clear local storage and subjects', () => {
    // Act
    service.signOut();

    // Assert
    expect(localStorageHelper.removeItem).toHaveBeenCalledWith(LocalStorageKeys.TokenName);
    expect(localStorageHelper.removeItem).toHaveBeenCalledWith(LocalStorageKeys.LoginId);
    expect(localStorageHelper.removeItem).toHaveBeenCalledWith(LocalStorageKeys.UserId);
    expect(authStateSubject.value).toBeFalse();
    expect(usernameSubject.value).toBeNull();
    expect(userIdSubject.value).toBeNull();
  });

  
});
