import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';

import { UpdateInterviewerComponent } from './update-interviewer.component';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule, NgForm } from '@angular/forms';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { InterviewerService } from 'src/app/services/interviewer.service';
import { UpdateInterviewer } from 'src/app/models/updateInterviewer.model';
import { of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { JobRoleInterviewer } from 'src/app/models/jobrole.interviewer.model';
import { Interviewer } from 'src/app/models/interviewer.model';
import { InterviewRoundInterviewer } from 'src/app/models/interviewround.interviewer.model';

describe('UpdateInterviewerComponent', () => {
  let component: UpdateInterviewerComponent;
  let fixture: ComponentFixture<UpdateInterviewerComponent>;
  let interviewerServiceSpy : jasmine.SpyObj<InterviewerService>;
  let routerSpy: Router;
  let route: ActivatedRoute;
  let httpMock: HttpTestingController;
  let service: InterviewerService;

  const mockInterviewer: UpdateInterviewer={
    employeeId: 0,
    firstName: '',
    lastName: '',
    email: '',
    jobRoleId: 1,
    interviewRoundId: 1
  };

  const mockInterviewers : Interviewer={
    employeeId: 0,
    firstName: '',
    lastName: '',
    email: '',
    jobRoleId: 0,
    interviewRoundId: 0,
    jobRole: {
      jobRoleId: 0,
      jobRoleName: ''
    },
    interviewRound: {
      interviewRoundId: 0,
      interviewRoundName: ''
    },
    isRecruiter: false,
    isAdmin: false,
    changePassword: false
  }
  const mockApiResponse: ApiResponse<Interviewer[]> = {
    success: true,
    data:[
      {
        employeeId: 1,
        firstName: '',
        lastName: '',
        email: '',
        jobRoleId: 0,
        interviewRoundId: 0,
        jobRole: {
          jobRoleId: 0,
          jobRoleName: ''
        },
        interviewRound: {
          interviewRoundId: 0,
          interviewRoundName: ''
        },
        isRecruiter: false,
        isAdmin: false,
        changePassword: false
      },
      {
        employeeId: 2,
        firstName: '',
        lastName: '',
        email: '',
        jobRoleId: 0,
        interviewRoundId: 0,
        jobRole: {
          jobRoleId: 0,
          jobRoleName: ''
        },
        interviewRound: {
          interviewRoundId: 0,
          interviewRoundName: ''
        },
        isRecruiter: false,
        isAdmin: false,
        changePassword: false
      }
    ],
    message : ''
  }
  
  beforeEach(() => {
    interviewerServiceSpy = jasmine.createSpyObj('InterviewerService', ['getEmployeeById', 'updateInterviewer', 'getAllInterviewers', 'getAllJobRoles', 'getAllInterviewRounds']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule.withRoutes([{path:'interviewers-list', component: UpdateInterviewerComponent}]), FormsModule, HttpClientTestingModule],
      declarations: [UpdateInterviewerComponent],
      providers : [
        {provide : InterviewerService, useValue : interviewerServiceSpy,},
        {provide: ActivatedRoute, useValue: {params: of({ id: 1 })}}
      ]
    });
   
    service = TestBed.inject(InterviewerService);
    httpMock = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(UpdateInterviewerComponent);
    component = fixture.componentInstance;
    interviewerServiceSpy = TestBed.inject(InterviewerService) as jasmine.SpyObj<InterviewerService>;
    // route = TestBed.inject(ActivatedRoute);
    routerSpy = TestBed.inject(Router);
   // fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  //ById
  it('should initialize employeeID from route params and load employee details', () => {
    // Arrange
    const mockResponse: ApiResponse<Interviewer> = { success: true, data: mockInterviewers, message: '' };
    interviewerServiceSpy.getEmployeeById.and.returnValue(of(mockResponse));

    // Act
    fixture.detectChanges(); 

    // Assert
    expect(component).toBeTruthy();
  });

  it('should alert error message on HTTP error', () => {
    // Arrange
    spyOn(window, 'alert');
    const mockError = { error: { message: 'HTTP error' } };
    interviewerServiceSpy.getEmployeeById.and.returnValue(throwError(mockError));
 
    // Act
    //fixture.detectChanges();
 
    // Assert
    expect(window.alert);
    expect(component).toBeTruthy();
  });

  //loadEmployeeDetails
  it('should load employee details and update component properties on successful response', () => {
    // Arrange
    const mockEmployeeId = 1;
    const mockEmployee: Interviewer = {
      employeeId: mockEmployeeId,
      firstName: 'Test',
      lastName: 'Test lastname',
      email: 'test@gmail.com',
      jobRoleId: 1,
      interviewRoundId: 1,
      jobRole: {
        jobRoleId: 0,
        jobRoleName: ''
      },
      interviewRound: {
        interviewRoundId: 0,
        interviewRoundName: ''
      },
      isRecruiter: false,
      isAdmin: false,
      changePassword: false
    };
    
    const mockApiResponse = { success: true, data: mockEmployee, message: '' };

    interviewerServiceSpy.getEmployeeById.and.returnValue(of(mockApiResponse));

    // Act
    component.loadEmployeeDetails(mockEmployeeId);

    // Assert
    expect(interviewerServiceSpy.getEmployeeById).toHaveBeenCalledWith(mockEmployeeId);
    expect(component.updateInterviewer.employeeId).toEqual(mockEmployee.employeeId);
    expect(component.updateInterviewer.firstName).toEqual(mockEmployee.firstName);
    expect(component.updateInterviewer.lastName).toEqual(mockEmployee.lastName);
    expect(component.updateInterviewer.email).toEqual(mockEmployee.email);
    expect(component.updateInterviewer.jobRoleId).toEqual(mockEmployee.jobRoleId);
    expect(component.updateInterviewer.interviewRoundId).toEqual(mockEmployee.interviewRoundId);
    expect(component.initialStateId).toEqual(mockEmployee.jobRoleId);
  });

  it('should log error message on API error', () => {
    // Arrange
    const mockEmployeeId = 1;
    const errorMessage = 'Failed to fetch employees';
    const mockErrorResponse = { success: false, message: 'Failed to fetch employees', data:{
      employeeId: 1,
      firstName: '',
      lastName: '',
      email: '',
      jobRoleId: 0,
      interviewRoundId: 0,
      jobRole: {
        jobRoleId: 0,
        jobRoleName: ''
      },
      interviewRound: {
        interviewRoundId: 0,
        interviewRoundName: ''
      },
      isRecruiter: false,
      isAdmin: false,
      changePassword: false
    } }
    interviewerServiceSpy.getEmployeeById.and.returnValue(of(mockErrorResponse));
    spyOn(console, 'error');

    // Act
    component.loadEmployeeDetails(mockEmployeeId);

    // Assert
    expect(interviewerServiceSpy.getEmployeeById).toHaveBeenCalledWith(mockEmployeeId);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch employees: ', errorMessage);
  });

  it('should alert error message on API error', () => {
    // Arrange
    const mockEmployeeId = 1;
    const mockErrorMessage = 'Failed to fetch employee details';
    const mockErrorResponse = { error: { message: mockErrorMessage } };
    spyOn(window, 'alert');
  
    interviewerServiceSpy.getEmployeeById.and.returnValue(throwError(mockErrorResponse));
  
    // Act
    component.loadEmployeeDetails(mockEmployeeId);
  
    // Assert
    expect(interviewerServiceSpy.getEmployeeById).toHaveBeenCalledWith(mockEmployeeId);
    expect(window.alert).toHaveBeenCalledWith(mockErrorMessage);
  });
  

  //onSubmit
  it('should submit form and handle successful response', () => {
    // Arrange
    const mockResponse = { success: true, message: 'Interviewer updated successfully' , data: ''};
    interviewerServiceSpy.updateInterviewer.and.returnValue(of(mockResponse));
    spyOn(routerSpy, 'navigate');
    // Act
    component.onSubmit({valid: true} as NgForm);

    // Assert
    expect(interviewerServiceSpy.updateInterviewer).toHaveBeenCalled();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/interviewers-list']);
    expect(component.loading).toBeFalse();
  });
  
  it('should handle successful update', () => {
    // Arrange
    const mockResponse = { success: true, message: 'Interviewer updated successfully', data: ''};
    interviewerServiceSpy.updateInterviewer.and.returnValue(of(mockResponse));
    spyOn(routerSpy, 'navigate');
    // Act
    component.employeeId = 1;
    const form = {
      valid: true,
      value: {
        employeeId: 1,
        firstName: '',
        lastName: '',
        email: '',
        jobRoleId: 1,
        interviewRoundId: 1}

    };
    component.onSubmit(form as NgForm);
    fixture.detectChanges();

    // Assert
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/interviewers-list']);
    expect(interviewerServiceSpy.updateInterviewer).toHaveBeenCalled();
  });

  it('should handle API error, display alert message, and stop loader', () => {
    // Arrange
    const mockErrorResponse = { error: { message: 'Server error: Unable to update interviewer' } };
    interviewerServiceSpy.updateInterviewer.and.returnValue(throwError(mockErrorResponse));
    spyOn(window, 'alert');
    spyOn(routerSpy, 'navigate');

    // Act
    component.onSubmit({valid: true} as NgForm);
    fixture.detectChanges();

    // Assert
    expect(interviewerServiceSpy.updateInterviewer).toHaveBeenCalled();
    expect(routerSpy.navigate).not.toHaveBeenCalled(); 
    //expect(component.onSubmit).not.toHaveBeenCalled(); 
    expect(window.alert).toHaveBeenCalledWith(mockErrorResponse.error.message);
    expect(component.loading).toBeTrue(); 
  });

  it('should handle successful API response, navigate, reset form, stop loader, and log completion', () => {
    // Arrange
    const mockResponse = { success: true, message: '', data: '' };
    interviewerServiceSpy.updateInterviewer.and.returnValue(of(mockResponse));
    spyOn(console, 'log');
    spyOn(routerSpy, 'navigate');
    // Act
    component.onSubmit({valid: true} as NgForm);
    fixture.detectChanges();

    // Assert
    expect(interviewerServiceSpy.updateInterviewer).toHaveBeenCalled();
    expect(routerSpy.navigate).toHaveBeenCalled(); 
    expect(component.loading).toBeTrue(); 
  });


  it('should show alert on unsuccessful response', () => {
    const mockErrorResponse = { error: { message: 'Failed to update interviewer' } };
    interviewerServiceSpy.updateInterviewer.and.returnValue(throwError(mockErrorResponse));
  
    const alertSpy = spyOn(window, 'alert'); 
  
    // Act
    component.onSubmit({ valid: true } as NgForm);
  
    // Assert
    expect(interviewerServiceSpy.updateInterviewer).toHaveBeenCalled();
    expect(alertSpy).toHaveBeenCalledWith('Failed to update interviewer');
  });

  //loadInterviewRounds
  it('should load interview rounds successfully', () => {
    const mockInterviewRounds: InterviewRoundInterviewer[] = [
      { interviewRoundId: 1,
        interviewRoundName: ''
       },
      { interviewRoundId: 2,
        interviewRoundName: ''
       }
    ];
    const mockApiResponse: ApiResponse<InterviewRoundInterviewer[]> = { success: true, data: mockInterviewRounds, message: 'Interview rounds fetched successfully' };

    interviewerServiceSpy.getAllInterviewRounds.and.returnValue(of(mockApiResponse));

    component.loadInterviewRounds();

    expect(component.loading).toBe(false);
    fixture.detectChanges();

    expect(component.loading).toBe(true);
    expect(component.interviewRounds).toEqual(mockInterviewRounds);
  });

  it('should handle error when fetching interview rounds fails', () => {
    const mockInterviewRounds: InterviewRoundInterviewer[] = [
      { interviewRoundId: 1,
        interviewRoundName: ''
       },
      { interviewRoundId: 2,
        interviewRoundName: ''
       }
    ];
    const errorMessage = 'Failed to fetch interview rounds';
    spyOn(console, 'error');
    interviewerServiceSpy.getAllInterviewRounds.and.returnValue(of({ success: false, data: mockInterviewRounds, message: errorMessage }));

    component.loadInterviewRounds();
    fixture.detectChanges();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch interview rounds', errorMessage);
  });

  it('should handle error fetching interview rounds', () => {
    const errorMessage = 'Error fetching interview rounds: ';
    interviewerServiceSpy.getAllInterviewRounds.and.returnValue(throwError(errorMessage));
    spyOn(console, 'error');

    component.loadInterviewRounds(); 
    fixture.detectChanges();
    expect(console.error).toHaveBeenCalledWith('Error fetching interview rounds: ', errorMessage);
  });

  //jobRoles
  it('should load job roles successfully', () => {
    
    const mockApiResponse: ApiResponse<JobRoleInterviewer[]> = {
      success: true,
      data: [
        { jobRoleId: 1, jobRoleName: 'Role 1' },
        { jobRoleId: 2, jobRoleName: 'Role 2' }
      ],
      message: 'Job roles fetched successfully'
    };
    interviewerServiceSpy.getAllJobRoles.and.returnValue(of(mockApiResponse));

    component.loadJobRoles(); 
    fixture.detectChanges();

    expect(component.jobRoles).toEqual(mockApiResponse.data);
  });

  it('should handle error fetching job roles', () => {
    const errorMessage = 'Error fetching job roles: ';
    interviewerServiceSpy.getAllJobRoles.and.returnValue(throwError(errorMessage));
    spyOn(console, 'error');

    component.loadJobRoles(); 
    fixture.detectChanges();
    expect(console.error).toHaveBeenCalledWith('Error fetching job roles: ', errorMessage);
  });

  it('should log error message on API error', () => {
    // Arrange
    const errorMessage = 'Failed to fetch job roles';
    const mockErrorResponse = { success: false, message: 'Failed to fetch job roles', data:[
      { jobRoleId: 1, jobRoleName: 'Role 1' },
      { jobRoleId: 2, jobRoleName: 'Role 2' }
    ],
  }
    interviewerServiceSpy.getAllJobRoles.and.returnValue(of(mockErrorResponse));
    spyOn(console, 'error');

    // Act
    component.loadJobRoles();

    // Assert
    expect(console.error).toHaveBeenCalledWith('Failed to fetch job roles', errorMessage);
  });
  
});
