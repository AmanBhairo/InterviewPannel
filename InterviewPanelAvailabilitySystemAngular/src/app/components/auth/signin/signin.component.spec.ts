import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SigninComponent } from './signin.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { AuthService } from 'src/app/services/auth.service';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { of, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { InterviewerService } from 'src/app/services/interviewer.service';
import { ChangeDetectorRef } from '@angular/core';

describe('SigninComponent', () => {
  let component: SigninComponent;
  let fixture: ComponentFixture<SigninComponent>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let interviewerServiceSpy: jasmine.SpyObj<InterviewerService>;
  let routerSpy: Router;
  let cdrSpy: jasmine.SpyObj<ChangeDetectorRef>;

  beforeEach(() => {
    authServiceSpy = jasmine.createSpyObj('AuthService', ['signIn', 'getUserId'])
    interviewerServiceSpy = jasmine.createSpyObj('InterviewerService', ['GetIsChangedPasswordById'])
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    cdrSpy = jasmine.createSpyObj('ChangeDetectorRef', ['detectChanges']);
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, FormsModule, RouterTestingModule],
      declarations: [SigninComponent],
      providers: [
        {
          provide: AuthService, useValue: authServiceSpy
        },
        {
          provide: InterviewerService, useValue: interviewerServiceSpy
        },
        { provide: ChangeDetectorRef, useValue: cdrSpy }
      ]
    });
    fixture = TestBed.createComponent(SigninComponent);
    routerSpy = TestBed.inject(Router);
    component = fixture.componentInstance;
    // fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });



  it('should set alert message when response is false', () => {
    // Arrange
    component.username = "test";
    component.password = "fakePassword"
    const mockResponse: ApiResponse<string> = {
      data: '',
      success: false,
      message: 'Verification failed'
    }
    spyOn(window, 'alert')
    authServiceSpy.signIn.and.returnValue(of(mockResponse))

    // Act
    component.login()

    // Assert
    expect(authServiceSpy.signIn).toHaveBeenCalledOnceWith(component.username, component.password)
    expect(window.alert).toHaveBeenCalledWith(mockResponse.message)
  });

  it('should set alert message when api returns error', () => {
    // Arrange
    component.username = "test";
    component.password = "fakePassword"
    const mockError = { error: { message: 'HTTP Error' } }
    spyOn(window, 'alert')
    authServiceSpy.signIn.and.returnValue(throwError(mockError))

    // Act
    component.login()

    // Assert
    expect(authServiceSpy.signIn).toHaveBeenCalledOnceWith(component.username, component.password)
    expect(window.alert).toHaveBeenCalledWith(mockError.error.message)
  });

  it('should IsEployeePasswordChangesIsTrue success', () => {
    // Arrange
    const mockResponse: ApiResponse<boolean> = {
      success: true,
      message: '',
      data: true
    }

    spyOn(console, 'log')
    spyOn(routerSpy, 'navigate')

    interviewerServiceSpy.GetIsChangedPasswordById.and.returnValue(of(mockResponse));

    // Act
    component.IsEployeePasswordChangesIsTrue(1)

    // Assert
    expect(interviewerServiceSpy.GetIsChangedPasswordById).toHaveBeenCalledOnceWith(1)

  });

  it('should IsEployeePasswordChangesIsTrue loginFirst is false', () => {
    // Arrange
    const mockResponse: ApiResponse<boolean> = {
      success: true,
      message: '',
      data: false
    }
component.loginFirst==false;
    spyOn(console, 'log')
    spyOn(routerSpy, 'navigate')

    interviewerServiceSpy.GetIsChangedPasswordById.and.returnValue(of(mockResponse));

    // Act
    component.IsEployeePasswordChangesIsTrue(1)

    // Assert
    expect(interviewerServiceSpy.GetIsChangedPasswordById).toHaveBeenCalledOnceWith(1)

  });

  it('should IsEployeePasswordChangesIsTrue not success', () => {
    // Arrange
    const mockResponse: ApiResponse<boolean> = {
      success: false,
      message: '',
      data: true
    }

    spyOn(console, 'log')
    spyOn(routerSpy, 'navigate')

    interviewerServiceSpy.GetIsChangedPasswordById.and.returnValue(of(mockResponse));

    // Act
    component.IsEployeePasswordChangesIsTrue(1)

    // Assert
    expect(interviewerServiceSpy.GetIsChangedPasswordById).toHaveBeenCalledOnceWith(1)

  });

  it('should set loading to false after successful login', () => {

    component.username = 'testuser';
    component.password = 'testpassword';
    spyOn(component, 'getEmployeeId');
    spyOn(component, 'IsEployeePasswordChangesIsTrue');
    const mockResponse: ApiResponse<string> = {
      data: 'fakeToken',
      success: true,
      message: ''
    }
    authServiceSpy.signIn.and.returnValue(of(mockResponse))
    component.login();

    // Assert
    expect(component.loading).toBeFalse();
    expect(component.getEmployeeId).toHaveBeenCalled();
    expect(component.IsEployeePasswordChangesIsTrue).toHaveBeenCalled();
  });
  it('should set alert message when api returns error for IsEployeePasswordChangesIsTrue', () => {
    // Arrange
   component.employeeIntId=1;
    const mockError = { message: 'HTTP Error' } 
    spyOn(console, 'error')
    interviewerServiceSpy.GetIsChangedPasswordById.and.returnValue(throwError(mockError))

    // Act
    component.IsEployeePasswordChangesIsTrue(component.employeeIntId)

    // Assert
    expect(interviewerServiceSpy.GetIsChangedPasswordById).toHaveBeenCalledOnceWith(component.employeeIntId);
    expect(console.error).toHaveBeenCalledWith('Error fetching is changed password:',mockError)
  });
  it('should fetch empId', () => {
    // Arrange
    const mockResponse: string='';
component.employeeIntId=1;

    authServiceSpy.getUserId.and.returnValue(of(mockResponse));

    // Act
    component.getEmployeeId()

    // Assert
    expect(authServiceSpy.getUserId).toHaveBeenCalledOnceWith()

  });
});
