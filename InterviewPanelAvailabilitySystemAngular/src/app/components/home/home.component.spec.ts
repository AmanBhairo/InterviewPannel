import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeComponent } from './home.component';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { AuthService } from 'src/app/services/auth.service';
import { Interviewer } from 'src/app/models/interviewer.model';
import { InterviewerService } from 'src/app/services/interviewer.service';
import { of } from 'rxjs';
import { Router } from '@angular/router';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let interviewService: jasmine.SpyObj<InterviewerService>;
  let routerSpy: Router;

  beforeEach(() => {
    interviewService = jasmine.createSpyObj('InterviewerService', ['getEmployeeById'])
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, HttpClientTestingModule],
      declarations: [HomeComponent],
      providers: [
        {
          provide: InterviewerService, useValue: interviewService
        }
      ]
    });
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    routerSpy = TestBed.inject(Router);
    // fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should get user successfully', () => {
    // Arrange
    let employee:Interviewer={
      employeeId: 0,
      firstName: '',
      lastName: '',
      email: '',
      jobRoleId: 0,
      interviewRoundId: 0,
      isRecruiter: false,
      isAdmin: false,
      changePassword: false,
      jobRole: {
        jobRoleId: 1,
        jobRoleName: ''
      },
      interviewRound: {
        interviewRoundId: 1,
        interviewRoundName: ''
      },
    }
    const mockResponse: ApiResponse<Interviewer> = {
      success: true,
      message: '',
      data: employee
    }
    component.Name=mockResponse.data.firstName
    spyOn(console, 'log')
    interviewService.getEmployeeById.and.returnValue(of(mockResponse));

    // Act
    component.getUser()

    // Assert
    expect(interviewService.getEmployeeById).toHaveBeenCalled()

  });

  it('should not get user', () => {
    // Arrange
    let employee:Interviewer={
      employeeId: 0,
      firstName: '',
      lastName: '',
      email: '',
      jobRoleId: 0,
      interviewRoundId: 0,
      isRecruiter: false,
      isAdmin: false,
      changePassword: false,
      jobRole: {
        jobRoleId: 1,
        jobRoleName: ''
      },
      interviewRound: {
        interviewRoundId: 1,
        interviewRoundName: ''
      },
    }
    const mockResponse: ApiResponse<Interviewer> = {
      success: false,
      message: '',
      data: employee
    }
    component.Name=mockResponse.data.firstName
    spyOn(console, 'log')
    interviewService.getEmployeeById.and.returnValue(of(mockResponse));

    // Act
    component.getUser()

    // Assert
    expect(interviewService.getEmployeeById).toHaveBeenCalled()

  });
});
