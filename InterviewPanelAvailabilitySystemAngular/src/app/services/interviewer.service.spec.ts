import { TestBed } from '@angular/core/testing';

import { InterviewerService } from './interviewer.service';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Interviewer } from '../models/interviewer.model';
import { AddInterviewer } from '../models/add-interviewer.model';
import { JobRole } from '../models/jobrole.model';
import { JobRoleInterviewer } from '../models/jobrole.interviewer.model';
import { UpdateInterviewer } from '../models/updateInterviewer.model';
import { InterviewRoundInterviewer } from '../models/interviewround.interviewer.model';
import { catchError, of } from 'rxjs';

describe('InterviewerService', () => {
  let service: InterviewerService;
  let httpMock : HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, HttpClientTestingModule],
      providers:[InterviewerService]
    });
    service = TestBed.inject(InterviewerService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(()=>{
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });


  //getAllInterviewers
  it('should fetch all interviewers without search parameter', () => {
    // Arrange
    const page = 1;
    const pageSize = 10;
    const sortOrder = 'asc';

    const mockResponse: ApiResponse<Interviewer[]> = {
      success: true,
      data: [
        {
          employeeId: 1,
          firstName: 'test',
          lastName: '',
          email: '',
          jobRoleId: 0,
          interviewRoundId: 0,
          isRecruiter: false,
          isAdmin: false,
          changePassword: false,
          jobRole: {
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        {
          employeeId: 2,
          firstName: 'test',
          lastName: '',
          email: '',
          jobRoleId: 0,
          interviewRoundId: 0,
          isRecruiter: false,
          isAdmin: false,
          changePassword: false,
          jobRole: {
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        }
      ],
      message: ''
    };

    // Act
    service.getAllInterviewers(page, pageSize, sortOrder).subscribe((response) => {
      // Assert
      expect(response).toBeTruthy();
      expect(response.success).toBeTrue();
      expect(response.data.length).toBe(2); 
      expect(response.data[0].firstName).toBe('test'); 
    });
    const expectedUrl = `http://localhost:5263/api/Admin/GetAllEmployees?page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should fetch all interviewers with search parameter', () => {
    // Arrange
    const page = 1;
    const pageSize = 10;
    const sortOrder = 'asc';
    const search = 'test';

    const mockResponse: ApiResponse<Interviewer[]> = {
      success: true,
      data: [
        {
          employeeId: 1,
          firstName: 'test',
          lastName: '',
          email: '',
          jobRoleId: 0,
          interviewRoundId: 0,
          isRecruiter: false,
          isAdmin: false,
          changePassword: false,
          jobRole: {
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
      ],
      message: ''
    };

    // Act
    service.getAllInterviewers(page, pageSize, sortOrder, search).subscribe((response) => {
      // Assert
      expect(response).toBeTruthy();
      expect(response.success).toBeTrue();
      expect(response.data.length).toBe(1); 
      expect(response.data[0].firstName).toBe('test'); 
    });

    const expectedUrl = `http://localhost:5263/api/Admin/GetAllEmployees?search=${search}&page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should log error and return ApiResponse with failure message', () => {
    // Arrange
    const errorMessage = 'Failed to load contacts';

    spyOn(console, 'error');

    // Act
    service.getAllInterviewers(1, 10, 'asc').pipe(
      catchError((error: any) => {
        // Assert
        expect(console.error).toHaveBeenCalledWith('An error occurred:', error);
        return of({ success: false, message: 'Failed to fetch interviewers list.' } as ApiResponse<Interviewer[]>);
      })
    ).subscribe((response) => {
      // Assert
      expect(response.success).toBeFalse();
      expect(response.message).toBe('Failed to fetch interviewers list.');
    });
    const req = httpMock.expectOne('http://localhost:5263/api/Admin/GetAllEmployees?page=1&pageSize=10&sortOrder=asc');
    expect(req.request.method).toBe('GET');
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });

    expect(console.error).toHaveBeenCalled();
  });

  //else part
  it('should log error and return ApiResponse with failure message for search', () => {
    // Arrange
    const page = 1;
    const pageSize = 10;
    const sortOrder = 'asc';
    const search = 'Testtt'; 
    const errorMessage = 'Failed to load contacts';


    spyOn(console, 'error');

    // Act
    service.getAllInterviewers(page, pageSize, sortOrder, search).pipe(
      catchError((error: any) => {
        expect(console.error).toHaveBeenCalledWith('An error occurred:', error);
        return of({ success: false, message: 'Failed to fetch interviewers list.' } as ApiResponse<Interviewer[]>);
      })
    ).subscribe((response) => {
      // Assert
      expect(response.success).toBeFalse();
      expect(response.message).toBe('Failed to fetch interviewers list.');
    });

    const expectedUrl = `http://localhost:5263/api/Admin/GetAllEmployees?search=${search}&page=${page}&pageSize=${pageSize}&sortOrder=${sortOrder}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });

    expect(console.error).toHaveBeenCalled();
  });


  //getTotalInterviewersCount
  it('should fetch total interviewers count without search parameter', () => {
    // Arrange
    const mockResponse: ApiResponse<number> = {
      success: true,
      data: 10, 
      message: ''
    };

    // Act
    service.getTotalInterviewersCount().subscribe((response) => {
      // Assert
      expect(response).toBeTruthy();
      expect(response.success).toBeTrue();
      expect(response.data).toBe(10); 
    });

    const expectedUrl = `http://localhost:5263/api/Admin/GetTotalEmployeeCount`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should fetch total interviewers count with search parameter', () => {
    // Arrange
    const search = 'John';

    const mockResponse: ApiResponse<number> = {
      success: true,
      data: 5, 
      message: ''
    };

    // Act
    service.getTotalInterviewersCount(search).subscribe((response) => {
      // Assert
      expect(response).toBeTruthy();
      expect(response.success).toBeTrue();
      expect(response.data).toBe(5); 
    });

    const expectedUrl = `http://localhost:5263/api/Admin/GetTotalEmployeeCount?search=${search}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });


  it('should log error and return ApiResponse with failure message', () => {
    // Arrange
    const errorMessage = 'Failed to load interviewer total count';
   
    spyOn(console, 'error');

    // Act
    service.getTotalInterviewersCount().pipe(
      catchError((error: any) => {
        // Assert
        expect(console.error).toHaveBeenCalledWith('An error occurred:', error);
        return of({ success: false, message: 'Failed to fetch total interviewers count.' } as ApiResponse<number>);
      })
    ).subscribe((response) => {
      // Assert
      expect(response.success).toBeFalse();
      expect(response.message).toBe('Failed to fetch total interviewers count.');
    });

    // Simulate HTTP error
    const req = httpMock.expectOne('http://localhost:5263/api/Admin/GetTotalEmployeeCount');
    expect(req.request.method).toBe('GET');
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
    expect(console.error).toHaveBeenCalled();
  });

  //else part
  it('should log error and return ApiResponse with failure message for getTotalInterviewersCount with search', () => {
    // Arrange
    const search = 'Testt'; 
    const errorMessage = 'Failed to load interviewer total count';


    spyOn(console, 'error');

    // Act
    service.getTotalInterviewersCount(search).pipe(
      catchError((error: any) => {
        expect(console.error).toHaveBeenCalledWith('An error occurred:', error);
        return of({ success: false, message: 'Failed to fetch total interviewers count.' } as ApiResponse<number>);
      })
    ).subscribe((response) => {
      // Assert
      expect(response.success).toBeFalse();
      expect(response.message).toBe('Failed to fetch total interviewers count.');
    });

    const expectedUrl = `http://localhost:5263/api/Admin/GetTotalEmployeeCount?search=${search}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });

    expect(console.error).toHaveBeenCalled();
  });

 
 
  //addInterviewer
  it('should add employee successfully', () => {
    // Arrange
    const mockAddInterviewer: AddInterviewer = {
      firstName: 'test',
      lastName: 'test last',
      email: 'test@gmail.com',
      jobRoleId: 1,
      interviewRoundId: 1,
    };

    const mockResponse: ApiResponse<string> = {
      success: true,
      data: 'Interviewer added successfully',
      message: ''
    };

    // Act
    service.addInterviewer(mockAddInterviewer).subscribe((response) => {
      // Assert
      expect(response).toBeTruthy();
      expect(response.success).toBeTrue();
      expect(response.data).toBe('Interviewer added successfully');
    });

    const expectedUrl = `http://localhost:5263/api/Admin/AddEmployee`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(mockAddInterviewer); 
    req.flush(mockResponse);
  });

  it('should handle failed employee addition', () => {
    //Arrange
    const addInterviewer: AddInterviewer = {
      firstName: 'test',
      lastName: 'test last',
      email: 'test@gmail.com',
      jobRoleId: 1,
      interviewRoundId: 1,
    };
    const mockErrorResponse: ApiResponse<string> = {
      success: true,
      message: "Interviewer already exists.",
      data: ""
    };
    //Act
    service.addInterviewer(addInterviewer).subscribe(response => {
      //Assert
      expect(response).toBe(mockErrorResponse);
    });

    const req = httpMock.expectOne('http://localhost:5263/api/Admin/AddEmployee');
    expect(req.request.method).toBe('POST');
    req.flush(mockErrorResponse);
  });

 
  //getAllJobRoles
  it('should fetch all job roles successfully', () => {
    // Arrange
    const mockJobRoles: JobRoleInterviewer[] = [
      { jobRoleId: 1, jobRoleName: 'Interviewer' },
      { jobRoleId: 2, jobRoleName: 'Recruiter' }
    ];

    const mockResponse: ApiResponse<JobRoleInterviewer[]> = {
      success: true,
      data: mockJobRoles,
      message: ''
    };

    // Act
    service.getAllJobRoles().subscribe((response) => {
      // Assert
      expect(response).toBeTruthy();
      expect(response.success).toBeTrue();
      expect(response.data).toEqual(mockJobRoles);
    });
    const expectedUrl = `http://localhost:5263/api/Admin/GetAllJobRoles`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });





  //getAllInterviewRounds
  it('should log error and return ApiResponse with failure message', () => {
    // Arrange
    const mockHttpError = {
      statusText: "Internal Server Error",
      status: 500
    }
    spyOn(console, 'error');

    // Act
    service.getAllInterviewRounds().pipe(
      catchError((error: any) => {
        // Assert
        expect(console.error).toHaveBeenCalledWith('An error occurred:', error);
        return of({ success: false, message: 'Failed to fetch interview rounds.' } as ApiResponse<InterviewRoundInterviewer[]>);
      })
    ).subscribe((response) => {
      // Assert
      expect(response.success).toBeFalse();
      expect(response.message).toBe('Failed to fetch interview rounds.');
    });
    const req = httpMock.expectOne('http://localhost:5263/api/Admin/GetAllInterviewRounds');
    expect(req.request.method).toBe('GET');
    req.flush({}, mockHttpError); 
    expect(console.error).toHaveBeenCalled();
  });

  


  //updateEmployee
  it('should update employee successfully', () => {
    //Arrange
    const updatedInterviewer: UpdateInterviewer = {
      employeeId: 0,
      firstName: '',
      lastName: '',
      email: '',
      jobRoleId: 0,
      interviewRoundId: 0
    };
    const mockSuccessResponse: ApiResponse<string> = {
      success: true,
      data: '',
      message: 'Employee updated successfully.'
    };

    //Act
    service.updateInterviewer(updatedInterviewer).subscribe(
      response => {
        //Assert
        expect(response).toEqual(mockSuccessResponse);
      });
    const req = httpMock.expectOne('http://localhost:5263/api/Admin/EditEmployee/');
    expect(req.request.method).toBe('PUT');
    req.flush(mockSuccessResponse);

  });

  it('should handle failed category update', () => {
    //Arrange
    const updatedemployee: UpdateInterviewer = {
      employeeId: 0,
      firstName: '',
      lastName: '',
      email: '',
      jobRoleId: 0,
      interviewRoundId: 0
    };
    const mockErrorResponse: ApiResponse<string> = {
      success: true,
      message: "Employee already exists.",
      data: ''
    };
    //Act
    service.updateInterviewer(updatedemployee).subscribe(
      response => {
        //Assert
        expect(response).toEqual(mockErrorResponse);
      });
    const req = httpMock.expectOne('http://localhost:5263/api/Admin/EditEmployee/');
    expect(req.request.method).toBe('PUT');
    req.flush(mockErrorResponse);

  });




  //getEmployeeById
  it('should fetch an employee by ID successfully', () => {
    // Arrange
    const employeeId = 1;
    const mockEmployee: Interviewer = {
      employeeId: employeeId,
      firstName: '',
      lastName: '',
      email: '',
      jobRoleId: 0,
      interviewRoundId: 0,
      isRecruiter: false,
      isAdmin: false,
      changePassword: false,
      jobRole: {
        jobRoleId: 0,
        jobRoleName: ''
      },
      interviewRound: {
        interviewRoundId: 0,
        interviewRoundName: ''
      }
    };

    const mockResponse: ApiResponse<Interviewer> = {
      success: true,
      data: mockEmployee,
      message: ''
    };

    // Act
    service.getEmployeeById(employeeId).subscribe((response) => {
      // Assert
      expect(response.success).toBeTrue();
      expect(response.data).toEqual(mockEmployee);
    });
    const expectedUrl = `http://localhost:5263/api/Admin/GetEmployeeById/${employeeId}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should handle failed employee id retrival', () =>{
    //Arrange
    const employeeId=1;
    const mockErrorResponse:ApiResponse<Interviewer>={
      success:false,
      data:{} as Interviewer,
      message:'No record found!'
    };
    //Act
    service.getEmployeeById(employeeId).subscribe(response=>{
      //Assert
      expect(response).toEqual(mockErrorResponse);
      expect(response.message).toBe('No record found!');
      expect(response.success).toBeFalse();
    });

    const req=httpMock.expectOne('http://localhost:5263/api/Admin/GetEmployeeById/'+employeeId);
    expect(req.request.method).toBe('GET');
    req.flush(mockErrorResponse);

  });



  //deleteEmployeeById
  it('should delete employee successfully', () => {
    // Arrange
    const employeeId = 1;
    const mockSuccessResponse: ApiResponse<string> = {
      success: true,
      data: '',
      message: 'Employee deleted successfully.'
    };

    // Act
    service.deleteEmployeeById(employeeId).subscribe(response => {
      // Assert
      expect(response.success).toBeTrue();
      expect(response.message).toBe('Employee deleted successfully.');
      expect(response.data).toEqual(mockSuccessResponse.data);
    });
    const expectedUrl = `http://localhost:5263/api/Admin/RemoveEmployee?id=${employeeId}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockSuccessResponse);
  });

  it('should handle failed delete employee', () =>{
    //Arrange
    const employeeId=1;
    const mockSuccessResponse:ApiResponse<string>={
      success:false,
      data:'',
      message:'Something went wrong, please try after sometimes.'
    };
    //Act
    service.deleteEmployeeById(employeeId).subscribe(response=>{
      //Assert
      expect(response.success).toBeFalse();
      expect(response.message).toBe('Something went wrong, please try after sometimes.');
      expect(response.data).toEqual(mockSuccessResponse.data);
    });

    const req=httpMock.expectOne(`http://localhost:5263/api/Admin/RemoveEmployee?id=${employeeId}`);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockSuccessResponse);

  });





  //GetIsChangedPasswordById/
  it('should get change password successfully', () => {
    // Arrange
    const employeeId = 1;
    const mockSuccessResponse: ApiResponse<boolean> = {
      success: true,
      data: false,
      message: 'Change password successfully.'
    };

    // Act
    service.GetIsChangedPasswordById(employeeId).subscribe(response => {
      // Assert
      expect(response.success).toBeTrue();
      expect(response.message).toBe('Change password successfully.');
      expect(response.data).toEqual(mockSuccessResponse.data);
    });
    const expectedUrl = `http://localhost:5263/api/Admin/GetIsChangedPasswordById/${employeeId}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockSuccessResponse);
  });

  it('should handle failed change password', () =>{
    //Arrange
    const employeeId=1;
    const mockSuccessResponse:ApiResponse<boolean>={
      success:false,
      data:false,
      message:'Something went wrong, please try after sometimes.'
    };
    //Act
    service.GetIsChangedPasswordById(employeeId).subscribe(response=>{
      //Assert
      expect(response.success).toBeFalse();
      expect(response.message).toBe('Something went wrong, please try after sometimes.');
      expect(response.data).toEqual(mockSuccessResponse.data);
    });

    const req=httpMock.expectOne(`http://localhost:5263/api/Admin/GetIsChangedPasswordById/${employeeId}`);
    expect(req.request.method).toBe('GET');
    req.flush(mockSuccessResponse);

  });


});
