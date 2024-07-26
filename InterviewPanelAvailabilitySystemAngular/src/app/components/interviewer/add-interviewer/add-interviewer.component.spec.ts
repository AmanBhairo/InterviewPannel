import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddInterviewerComponent } from './add-interviewer.component';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule, NgForm } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { InterviewerService } from 'src/app/services/interviewer.service';
import { Router } from '@angular/router';
import { HomeComponent } from '../../home/home.component';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { of, throwError } from 'rxjs';
import { JobRoleInterviewer } from 'src/app/models/jobrole.interviewer.model';
import { InterviewRoundInterviewer } from 'src/app/models/interviewround.interviewer.model';
import { InterviewerListComponent } from '../interviewer-list/interviewer-list.component';

describe('AddInterviewerComponent', () => {
  let component: AddInterviewerComponent;
  let fixture: ComponentFixture<AddInterviewerComponent>;
  let interviewerServiceSpy: jasmine.SpyObj<InterviewerService>;
  let routerSpy: Router;
  let routerSpyy: jasmine.SpyObj<Router>;

  beforeEach(() => {
    let interviewerServiceSpyObj = jasmine.createSpyObj('InterviewerService', [
      'getAllJobRoles',
      'getAllInterviewRounds',
      'addInterviewer',
      'resetForm'
    ]);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        FormsModule,
        RouterTestingModule.withRoutes([
          { path: 'home', component: HomeComponent }, {path:'interviewers-list', component:InterviewerListComponent}
        ]),
      ],
      declarations: [AddInterviewerComponent],
      providers: [
        { provide: InterviewerService, useValue: interviewerServiceSpyObj },
      ],
    });
    fixture = TestBed.createComponent(AddInterviewerComponent);
    component = fixture.componentInstance;
    interviewerServiceSpy = TestBed.inject(InterviewerService) as jasmine.SpyObj<InterviewerService>;
    routerSpy = TestBed.inject(Router);
    // fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should load job roles and interviewer rounds on init', () => {
    let mockJobRoles: JobRoleInterviewer[] = [
      {
        jobRoleId: 1,
        jobRoleName: 'jobrole',
      },
    ];
    let mockInterviewRounds: InterviewRoundInterviewer[] = [
      {
        interviewRoundId: 1,
        interviewRoundName: 'interview round',
      },
    ];
    let mockApiResponse: ApiResponse<JobRoleInterviewer[]> = {
      data: mockJobRoles,
      success: true,
      message: '',
    };
    let mockApiResponseForInterviewRounds: ApiResponse<InterviewRoundInterviewer[]> = {
      data: mockInterviewRounds,
      success: true,
      message: '',
    };
    interviewerServiceSpy.getAllJobRoles.and.returnValue(of(mockApiResponse));
    interviewerServiceSpy.getAllInterviewRounds.and.returnValue(
      of(mockApiResponseForInterviewRounds)
    );
    fixture.detectChanges();

    expect(interviewerServiceSpy.getAllJobRoles).toHaveBeenCalled();
    expect(component.jobRoles).toEqual(mockJobRoles);
    expect(interviewerServiceSpy.getAllInterviewRounds).toHaveBeenCalled();
    expect(component.interviewRounds).toEqual(mockInterviewRounds);
  });
  it('should set job roles when job roles are loaded successfully', () => {
    let mockJobRoles: JobRoleInterviewer[] = [
      {
        jobRoleId: 1,
        jobRoleName: 'jobrole',
      },
    ];

    let mockApiResponse: ApiResponse<JobRoleInterviewer[]> = {
      data: mockJobRoles,
      success: true,
      message: '',
    };

    interviewerServiceSpy.getAllJobRoles.and.returnValue(of(mockApiResponse));
    component.loadJobRoles();

    expect(interviewerServiceSpy.getAllJobRoles).toHaveBeenCalled();
    expect(component.jobRoles).toEqual(mockJobRoles);
  });
  it('should set console error when loading job roles fails', () => {
    let mockApiResponse: ApiResponse<JobRoleInterviewer[]> = {
      data: [],
      success: false,
      message: 'Error fetching job roles',
    };
    spyOn(console, 'error');

    interviewerServiceSpy.getAllJobRoles.and.returnValue(of(mockApiResponse));
    component.loadJobRoles();

    expect(interviewerServiceSpy.getAllJobRoles).toHaveBeenCalledWith();
    expect(console.error).toHaveBeenCalledWith(
      'Failed to fetch job roles',
      mockApiResponse.message
    );
  });
  it('should set console error when loading job roles fails', () => {
    let mockError = { error: { message: 'Failed' } };
    spyOn(console, 'error');

    interviewerServiceSpy.getAllJobRoles.and.returnValue(throwError(mockError));
    component.loadJobRoles();

    expect(interviewerServiceSpy.getAllJobRoles).toHaveBeenCalledWith();
    expect(console.error).toHaveBeenCalledWith(
      'Error fetching job roles: ',
      mockError
    );
  });
  // ---
  it('should set console error when loading interview rounds fails', () => {
    let mockApiResponse: ApiResponse<InterviewRoundInterviewer[]> = {
      data: [],
      success: false,
      message: 'Failed to fetch interview rounds',
    };
    spyOn(console, 'error');

    interviewerServiceSpy.getAllInterviewRounds.and.returnValue(
      of(mockApiResponse)
    );
    component.loadInterviewRounds();

    expect(interviewerServiceSpy.getAllInterviewRounds).toHaveBeenCalledWith();
    expect(console.error).toHaveBeenCalledWith(
      'Failed to fetch interview rounds',
      mockApiResponse.message
    );
  });
  it('should set console error when loading interview rounds fails', () => {
    let mockError = { error: { message: 'Failed' } };
    spyOn(console, 'error');

    interviewerServiceSpy.getAllInterviewRounds.and.returnValue(
      throwError(mockError)
    );
    component.loadInterviewRounds();

    expect(interviewerServiceSpy.getAllInterviewRounds).toHaveBeenCalledWith();
    expect(console.error).toHaveBeenCalledWith(
      'Error fetching interview rounds: ',
      mockError
    );
  });
  it('should navigate to /interviewers-list on successful interviewer addition', () => {
    const mockResponse: ApiResponse<string> = {
      success: true,
      data: '',
      message: '',
    };
    interviewerServiceSpy.addInterviewer.and.returnValue(of(mockResponse));

    const addInterviewerForm = <NgForm>(<unknown>{
      valid: true,
      value: {
        firstName: 'firstname',
        lastName: 'lastname',
        email: 'test@gmail.com',
        jobRoleId: 1,
        interviewRoundId: 2,
      },
      controls: {
        firstName: { value: 'firstname' },
        lastName: { value: 'lastname' },
        email: { value: 'test@gmail.com' },
        jobRoleId: { value: 1 },
        interviewRoundId: { value: 2 },
      },
    });
    component.onSubmit(addInterviewerForm);
    // expect(routerSpyy.navigate).toHaveBeenCalledWith(['/interviewers-list']);
    expect(component.loading).toBe(false);
  });
  it('should set alert when response is false', () => {
    spyOn(routerSpy, 'navigate');
    spyOn(window, 'alert');
    const mockResponse: ApiResponse<string> = {
      success: false,
      data: '',
      message: 'Something went wrong, please try after sometime',
    };
    interviewerServiceSpy.addInterviewer.and.returnValue(of(mockResponse));

    const addInterviewerForm = <NgForm>(<unknown>{
      valid: true,
      value: {
        firstName: 'firstname',
        lastName: 'lastname',
        email: 'test@gmail.com',
        jobRoleId: 1,
        interviewRoundId: 2,
      },
      controls: {
        firstName: { value: 'firstname' },
        lastName: { value: 'lastname' },
        email: { value: 'test@gmail.com' },
        jobRoleId: { value: 1 },
        interviewRoundId: { value: 1 },
      },
    });
    const formValue = {
      firstName: 'firstname',
        lastName: 'lastname',
        email: 'test@gmail.com',
        jobRoleId: 1,
        interviewRoundId: 1,
    }
    component.newInterviewer.firstName = 'firstname'
    component.newInterviewer.lastName = 'lastname'
    component.newInterviewer.email = 'test@gmail.com'
    component.newInterviewer.jobRoleId = 1
    component.newInterviewer.interviewRoundId = 1
    component.onSubmit(addInterviewerForm);
    expect(interviewerServiceSpy.addInterviewer).toHaveBeenCalledWith(formValue);
    expect(component.loading).toBe(false);
    expect(window.alert).toHaveBeenCalledOnceWith(mockResponse.message);
  });
  it('should set alert when api returns error', () => {
    spyOn(routerSpy, 'navigate');
    spyOn(window, 'alert');
    const mockError = { error: { message: 'HTTP error' } };
    interviewerServiceSpy.addInterviewer.and.returnValue(throwError(mockError));

    const addInterviewerForm = <NgForm>(<unknown>{
      valid: true,
      value: {
        firstName: 'firstname',
        lastName: 'lastname',
        email: 'test@gmail.com',
        jobRoleId: 1,
        interviewRoundId: 2,
      },
      controls: {
        firstName: { value: 'firstname' },
        lastName: { value: 'lastname' },
        email: { value: 'test@gmail.com' },
        jobRoleId: { value: 1 },
        interviewRoundId: { value: 1 },
      },
    });
    const formValue = {
      firstName: 'firstname',
        lastName: 'lastname',
        email: 'test@gmail.com',
        jobRoleId: 1,
        interviewRoundId: 1,
    }
    component.onSubmit(addInterviewerForm);

    expect(component.loading).toBe(false);
    expect(window.alert).toHaveBeenCalledOnceWith(mockError.error.message);
  });
});
