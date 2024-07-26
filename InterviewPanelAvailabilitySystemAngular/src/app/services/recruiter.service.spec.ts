import { TestBed } from '@angular/core/testing';

import { RecruiterService } from './recruiter.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { RecruiterDetails } from '../models/recruiterDetails.model';
import { ApiResponse } from '../models/ApiResponse{T}';
import { InterviewSlots } from '../models/interviewSlots.model';

describe('RecruiterService', () => {
  let service: RecruiterService;
  let httpMock : HttpTestingController;
  
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, HttpClientTestingModule],
      providers:[RecruiterService]
    });
    service = TestBed.inject(RecruiterService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(()=>{
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });


  //getInterviewersByJobRole

  it('should fetch interviewers by job role successfully', () => {
    // Arrange
    const jobRoleId = 1;
    const pageNumber = 1;
    const pageSize = 4;
    const mockInterviewers: RecruiterDetails[] = [
      {
        slotId: 0,
        slotDate: '',
        timeslotId: 0,
        isBooked: false,
        jobRoleId: 0,
        jobRoleName: '',
        timeslotName: '',
        interviewRoundId: 0,
        interviewRoundName: '',
        employeeId: 0,
        firstName: '',
        lastName: '',
        email: '',
        employee:{
          employeeId:0,
          firstName:'',
          lastName:'',
          email:'',
          changePassword:false,
          interviewRoundId:0,
          isAdmin:false,
          isRecruiter:false,
          jobRoleId:0,
          interviewRound:{
            interviewRoundId:0,
            interviewRoundName:''
          },
          jobRole:{
            jobRoleId:0,
            jobRoleName:''
          }
        }
      },
      {
        slotId: 0,
        employeeId: 0,
        slotDate: '',
        timeslotId: 0,
        isBooked: false,
        firstName: '',
        lastName: '',
        email: '',
        jobRoleId: 0,
        jobRoleName: '',
        timeslotName: '',
        interviewRoundId: 0,
        interviewRoundName: '',
        employee:{
          employeeId:0,
          firstName:'',
          lastName:'',
          email:'',
          changePassword:false,
          interviewRoundId:0,
          isAdmin:false,
          isRecruiter:false,
          jobRoleId:0,
          interviewRound:{
            interviewRoundId:0,
            interviewRoundName:''
          },
          jobRole:{
            jobRoleId:0,
            jobRoleName:''
          }
        }
      }
    ];
    const mockApiResponse: ApiResponse<RecruiterDetails[]> = {
      success: true,
      data: mockInterviewers,
      message: ''
    };
  
    // Act
    service.getInterviewersByJobRole(jobRoleId, pageNumber, pageSize).subscribe(response => {
      // Assert
      expect(response.success).toBeTrue();
      expect(response.data).toEqual(mockApiResponse.data);
      expect(response.message).toBe('');
    });
  
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetInterviewersByJobRole/${jobRoleId}?page=${pageNumber}&pageSize=${pageSize}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should handle empty interviewers list', () => {
    // Arrange
    const jobRoleId = 1;
    const pageNumber = 1;
    const pageSize = 4;
    const mockApiResponse: ApiResponse<RecruiterDetails[]> = {
      success: true,
      data: [],
      message: ''
    };
  
    // Act
    service.getInterviewersByJobRole(jobRoleId,pageNumber,pageSize).subscribe(response => {
      // Assert
      expect(response.success).toBeTrue();
      expect(response.data).toEqual(mockApiResponse.data);
      expect(response.message).toBe('');
    });
  
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetInterviewersByJobRole/${jobRoleId}?page=${pageNumber}&pageSize=${pageSize}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });
  
  it('should handle server error', () => {
    // Arrange
    const jobRoleId = 1;
    const pageNumber = 1;
    const pageSize = 4;
    const mockErrorResponse = { status: 500, statusText: 'Internal Server Error' };
  
    // Act
    service.getInterviewersByJobRole(jobRoleId,pageNumber,pageSize).subscribe({
      error: (err) => {
        // Assert
        expect(err.status).toBe(mockErrorResponse.status);
        expect(err.statusText).toBe(mockErrorResponse.statusText);
      }
    });
  
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetInterviewersByJobRole/${jobRoleId}?page=${pageNumber}&pageSize=${pageSize}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(null, mockErrorResponse);
  });
  


  // //updateInterviewSlot
  it('should update interview slot successfully', () => {
    // Arrange
    const slotId = 1;
    const mockUpdatedSlots: InterviewSlots[] = [
      {
        slotId: 1, employeeId: 1, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      },
      {
        slotId: 2, employeeId: 2, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      }
    ];
    const mockApiResponse: ApiResponse<InterviewSlots[]> = {
      success: true,
      data: mockUpdatedSlots,
      message: 'Interview slot updated successfully.'
    };
  
    // Act
    service.updateInterviewSlot(slotId).subscribe(response => {
      // Assert
      expect(response.success).toBeTrue();
      expect(response.data).toEqual(mockApiResponse.data);
      expect(response.message).toBe('Interview slot updated successfully.');
    });
  
    const expectedUrl = `http://localhost:5263/api/Recruiter/UpdateInterviewSlot`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body.slotId).toBe(slotId);
    req.flush(mockApiResponse);
  });
  

  it('should handle server error during interview slot update', () => {
    // Arrange
    const slotId = 1;
    const mockErrorResponse = { status: 500, statusText: 'Internal Server Error' };
  
    // Act
    service.updateInterviewSlot(slotId).subscribe({
      error: (err) => {
        // Assert
        expect(err.status).toBe(mockErrorResponse.status);
        expect(err.statusText).toBe(mockErrorResponse.statusText);
      }
    });
  
    const expectedUrl = `http://localhost:5263/api/Recruiter/UpdateInterviewSlot`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body.slotId).toBe(slotId);
    req.flush(null, mockErrorResponse);
  });
  

  it('should handle no interview slot found for update', () => {
    // Arrange
    const slotId = 999; 
    const mockErrorResponse: ApiResponse<InterviewSlots[]> = {
      success: false,
      data: [],
      message: 'No interview slot found for update.'
    };
  
    // Act
    service.updateInterviewSlot(slotId).subscribe(response => {
      // Assert
      expect(response.success).toBeFalse();
      expect(response.data).toEqual(mockErrorResponse.data);
      expect(response.message).toBe('No interview slot found for update.');
    });
  
    const expectedUrl = `http://localhost:5263/api/Recruiter/UpdateInterviewSlot`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body.slotId).toBe(slotId);
    req.flush(mockErrorResponse);
  });
  

  // //getInterviewersByInterviewRound
  it('should fetch interviewers by interview round successfully', () => {
    // Arrange
    const interviewRoundId = 1;
    const pageNumber = 1;
    const pageSize = 4;
    const mockInterviewers: RecruiterDetails[] = [
      {
        employeeId: 1,
        slotId: 0,
        slotDate: '',
        timeslotId: 0,
        isBooked: false,
        firstName: '',
        lastName: '',
        email: '',
        jobRoleId: 0,
        jobRoleName: '',
        timeslotName: '',
        interviewRoundId: 0,
        interviewRoundName: '',
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        }
      },
      {
        employeeId: 2,
        slotId: 0,
        slotDate: '',
        timeslotId: 0,
        isBooked: false,
        firstName: '',
        lastName: '',
        email: '',
        jobRoleId: 0,
        jobRoleName: '',
        timeslotName: '',
        interviewRoundId: 0,
        interviewRoundName: '',
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        }
      }
    ];
    const mockApiResponse: ApiResponse<RecruiterDetails[]> = {
      success: true,
      data: mockInterviewers,
      message: 'Interviewers fetched successfully.'
    };
  
    // Act
    service.getInterviewersByInterviewRound(interviewRoundId,pageNumber,pageSize).subscribe(response => {
      // Assert
      expect(response.success).toBeTrue();
      expect(response.data).toEqual(mockApiResponse.data);
      expect(response.message).toBe('Interviewers fetched successfully.');
    });
  
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetInterviewersByInterviewRound/${interviewRoundId}?page=${pageNumber}&pageSize=${pageSize}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });
  
  it('should handle server error when fetching interviewers', () => {
    // Arrange
    const interviewRoundId = 1;
    const pageNumber = 1;
    const pageSize = 4;
    const mockErrorResponse = { status: 500, statusText: 'Internal Server Error' };
  
    // Act
    service.getInterviewersByInterviewRound(interviewRoundId,pageNumber,pageSize).subscribe({
      error: (err) => {
        // Assert
        expect(err.status).toBe(mockErrorResponse.status);
        expect(err.statusText).toBe(mockErrorResponse.statusText);
      }
    });
  
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetInterviewersByInterviewRound/${interviewRoundId}?page=${pageNumber}&pageSize=${pageSize}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(null, mockErrorResponse);
  });

  it('should fetch all interviewers with pagination and no search query', () => {
    // Arrange
    const pageNumber = 1;
    const pageSize = 10;
    const sort = 'asc';
    const mockInterviewers: InterviewSlots[] = [
      {
        slotId: 1, employeeId: 1, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      },
      {
        slotId: 2, employeeId: 2, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      }
    ];
    const mockApiResponse: ApiResponse<InterviewSlots[]> = {
      success: true,
      data: mockInterviewers,
      message: ''
    };

    // Act
    service.getAllInterviwersWithPagination(pageNumber, pageSize, sort).subscribe(response => {
      // Assert
      expect(response.success).toBeTrue();
      expect(response.data).toEqual(mockApiResponse.data);
      expect(response.message).toBe('');
    });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetPaginatedInterviwer?page=${pageNumber}&pageSize=${pageSize}&sortOrder=${sort}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch all interviewers with pagination and search query', () => {
    // Arrange
    const pageNumber = 1;
    const pageSize = 10;
    const sort = 'asc';
    const searchQuery = 'John';
    const mockInterviewers: InterviewSlots[] = [
      {
        slotId: 1, employeeId: 1, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      },
      {
        slotId: 2, employeeId: 2, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      }
    ];
    const mockApiResponse: ApiResponse<InterviewSlots[]> = {
      success: true,
      data: mockInterviewers,
      message: ''
    };

    // Act
    service.getAllInterviwersWithPagination(pageNumber, pageSize, sort, searchQuery).subscribe(response => {
      // Assert
      expect(response.success).toBeTrue();
      expect(response.data).toEqual(mockApiResponse.data);
      expect(response.message).toBe('');
    });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetPaginatedInterviwer?page=${pageNumber}&pageSize=${pageSize}&searchQuery=${searchQuery}&sortOrder=${sort}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch total interviewer count without search query', () => {
    // Arrange
    const mockCount = 10;
    const mockApiResponse: ApiResponse<number> = {
      success: true,
      data: mockCount,
      message: ''
    };

    // Act
    service.getAllInterviewerCount().subscribe(response => {
      // Assert
      expect(response.success).toBeTrue();
      expect(response.data).toEqual(mockCount);
      expect(response.message).toBe('');
    });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetTotalInterviewSlots`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch total interviewer count with search query', () => {
    // Arrange
    const searchQuery = 'John';
    const mockCount = 5;
    const mockApiResponse: ApiResponse<number> = {
      success: true,
      data: mockCount,
      message: ''
    };

    // Act
    service.getAllInterviewerCount(searchQuery).subscribe(response => {
      // Assert
      expect(response.success).toBeTrue();
      expect(response.data).toEqual(mockCount);
      expect(response.message).toBe('');
    });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetTotalInterviewSlots?searchQuery=${searchQuery}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch total interviewers by job role successfully', () => {
    // Arrange
    const jobRoleId = 1;
    const mockCount = 5;
    const mockApiResponse: ApiResponse<number> = {
      success: true,
      data: mockCount,
      message: ''
    };

    // Act
    service.getTotalInterviewersByJobRole(jobRoleId).subscribe(response => {
      // Assert
      expect(response.success).toBeTrue();
      expect(response.data).toEqual(mockCount);
      expect(response.message).toBe('');
    });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetTotalInterviewersByJobRole?jobRoleId=${jobRoleId}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch total interviewers by interviewer round successfully', () => {
    // Arrange
    const interviewRoundId = 1;
    const mockCount = 5;
    const mockApiResponse: ApiResponse<number> = {
      success: true,
      data: mockCount,
      message: ''
    };

    // Act
    service.getTotalInterviewersByInterviewerRound(interviewRoundId).subscribe(response => {
      // Assert
      expect(response.success).toBeTrue();
      expect(response.data).toEqual(mockCount);
      expect(response.message).toBe('');
    });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetTotalInterviewersByInterviewRound?interviewRoundId=${interviewRoundId}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch total interview slots when jobRoleId is 0, roundId is 0, and searchQuery is null', () => {
    // Arrange
    const jobRoleId = 0;
    const roundId = 0;
    const searchQuery = '';

    const mockTotalInterviewSlots = 25;
    const mockApiResponse: ApiResponse<number> = {
      success: true,
      data: mockTotalInterviewSlots,
      message: ''
    };

    // Act
    service.getTotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId)
      .subscribe(response => {
        // Assert
        expect(response.success).toBeTrue();
        expect(response.data).toEqual(mockApiResponse.data);
        expect(response.message).toBe('');
      });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetTotalInterviewSlotsByAll`;
    const req = httpMock.expectOne(expectedUrl);

    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch interviewers with pagination when jobRoleId and roundId are both 0 and searchQuery is null', () => {
    // Arrange
    const pageNumber = 1;
    const pageSize = 20;
    const sort = 'asc';
    const jobRoleId = 0;
    const roundId = 0;
    const searchQuery = '';

    const mockInterviewers: InterviewSlots[] = [
      {
        slotId: 1, employeeId: 1, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      },
      {
        slotId: 2, employeeId: 2, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      }
    ]; 
    const mockApiResponse: ApiResponse<InterviewSlots[]> = {
      success: true,
      data: mockInterviewers,
      message: ''
    };

    // Act
    service.getAllInterviwersWithPaginationByAll(pageNumber, pageSize, sort, searchQuery, jobRoleId, roundId)
      .subscribe(response => {
        // Assert
        expect(response.success).toBeTrue();
        expect(response.data).toEqual(mockApiResponse.data);
        expect(response.message).toBe('');
      });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetPaginatedInterviwerByAll?page=${pageNumber}&pageSize=${pageSize}&sortOrder=${sort}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch interviewers with pagination when jobRoleId and roundId are both 0 and searchQuery is not null', () => {
    // Arrange
    const pageNumber = 1;
    const pageSize = 20;
    const sort = 'asc';
    const jobRoleId = 0;
    const roundId = 0;
    const searchQuery = 'John';

    const mockInterviewers: InterviewSlots[] = [
      {
        slotId: 1, employeeId: 1, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      },
      {
        slotId: 2, employeeId: 2, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      }
    ]; 
    const mockApiResponse: ApiResponse<InterviewSlots[]> = {
      success: true,
      data: mockInterviewers,
      message: ''
    };

    // Act
    service.getAllInterviwersWithPaginationByAll(pageNumber, pageSize, sort, searchQuery, jobRoleId, roundId)
      .subscribe(response => {
        // Assert
        expect(response.success).toBeTrue();
        expect(response.data).toEqual(mockApiResponse.data);
        expect(response.message).toBe('');
      });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetPaginatedInterviwerByAll?page=${pageNumber}&pageSize=${pageSize}&searchQuery=${searchQuery}&sortOrder=${sort}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch interviewers with pagination when jobRoleId is not 0, roundId is not 0, and searchQuery is null', () => {
    // Arrange
    const pageNumber = 1;
    const pageSize = 20;
    const sort = 'asc';
    const jobRoleId = 1;
    const roundId = 1;
    const searchQuery = '';

    const mockInterviewers: InterviewSlots[] = [
      {
        slotId: 1, employeeId: 1, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      },
      {
        slotId: 2, employeeId: 2, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      }
    ]; 
    const mockApiResponse: ApiResponse<InterviewSlots[]> = {
      success: true,
      data: mockInterviewers,
      message: ''
    };

    // Act
    service.getAllInterviwersWithPaginationByAll(pageNumber, pageSize, sort, searchQuery, jobRoleId, roundId)
      .subscribe(response => {
        // Assert
        expect(response.success).toBeTrue();
        expect(response.data).toEqual(mockApiResponse.data);
        expect(response.message).toBe('');
      });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetPaginatedInterviwerByAll?page=${pageNumber}&pageSize=${pageSize}&sortOrder=${sort}&jobRoleId=${jobRoleId}&roundId=${roundId}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch interviewers with pagination when jobRoleId is not 0, roundId is not 0, and searchQuery is not null', () => {
    // Arrange
    const pageNumber = 1;
    const pageSize = 20;
    const sort = 'asc';
    const jobRoleId = 1;
    const roundId = 1;
    const searchQuery = 'Jane';

    const mockInterviewers: InterviewSlots[] = [
      {
        slotId: 1, employeeId: 1, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      },
      {
        slotId: 2, employeeId: 2, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      }
    ]; 
    const mockApiResponse: ApiResponse<InterviewSlots[]> = {
      success: true,
      data: mockInterviewers,
      message: ''
    };

    // Act
    service.getAllInterviwersWithPaginationByAll(pageNumber, pageSize, sort, searchQuery, jobRoleId, roundId)
      .subscribe(response => {
        // Assert
        expect(response.success).toBeTrue();
        expect(response.data).toEqual(mockApiResponse.data);
        expect(response.message).toBe('');
      });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetPaginatedInterviwerByAll?page=${pageNumber}&pageSize=${pageSize}&searchQuery=${searchQuery}&sortOrder=${sort}&jobRoleId=${jobRoleId}&roundId=${roundId}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch interviewers with pagination when jobRoleId is 0, roundId is not 0, and searchQuery is null', () => {
    // Arrange
    const pageNumber = 1;
    const pageSize = 20;
    const sort = 'asc';
    const jobRoleId = 0;
    const roundId = 1;
    const searchQuery = '';

    const mockInterviewers: InterviewSlots[] = [
      {
        slotId: 1, employeeId: 1, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      },
      {
        slotId: 2, employeeId: 2, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      }
    ]; 
    const mockApiResponse: ApiResponse<InterviewSlots[]> = {
      success: true,
      data: mockInterviewers,
      message: ''
    };

    // Act
    service.getAllInterviwersWithPaginationByAll(pageNumber, pageSize, sort, searchQuery, jobRoleId, roundId)
      .subscribe(response => {
        // Assert
        expect(response.success).toBeTrue();
        expect(response.data).toEqual(mockApiResponse.data);
        expect(response.message).toBe('');
      });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetPaginatedInterviwerByAll?page=${pageNumber}&pageSize=${pageSize}&sortOrder=${sort}&roundId=${roundId}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch interviewers with pagination when jobRoleId is 0, roundId is not 0, and searchQuery is not null', () => {
    // Arrange
    const pageNumber = 1;
    const pageSize = 20;
    const sort = 'asc';
    const jobRoleId = 0;
    const roundId = 1;
    const searchQuery = 'Doe';

    const mockInterviewers: InterviewSlots[] = [
      {
        slotId: 1, employeeId: 1, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      },
      {
        slotId: 2, employeeId: 2, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      }
    ];
    const mockApiResponse: ApiResponse<InterviewSlots[]> = {
      success: true,
      data: mockInterviewers,
      message: ''
    };

    // Act
    service.getAllInterviwersWithPaginationByAll(pageNumber, pageSize, sort, searchQuery, jobRoleId, roundId)
      .subscribe(response => {
        // Assert
        expect(response.success).toBeTrue();
        expect(response.data).toEqual(mockApiResponse.data);
        expect(response.message).toBe('');
      });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetPaginatedInterviwerByAll?page=${pageNumber}&pageSize=${pageSize}&searchQuery=${searchQuery}&sortOrder=${sort}&roundId=${roundId}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch interviewers with pagination when jobRoleId is not 0, roundId is 0, and searchQuery is null', () => {
    // Arrange
    const pageNumber = 1;
    const pageSize = 20;
    const sort = 'asc';
    const jobRoleId = 1;
    const roundId = 0;
    const searchQuery = '';

    const mockInterviewers: InterviewSlots[] = [
      {
        slotId: 1, employeeId: 1, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      },
      {
        slotId: 2, employeeId: 2, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      }
    ]; 
    const mockApiResponse: ApiResponse<InterviewSlots[]> = {
      success: true,
      data: mockInterviewers,
      message: ''
    };

    // Act
    service.getAllInterviwersWithPaginationByAll(pageNumber, pageSize, sort, searchQuery, jobRoleId, roundId)
      .subscribe(response => {
        // Assert
        expect(response.success).toBeTrue();
        expect(response.data).toEqual(mockApiResponse.data);
        expect(response.message).toBe('');
      });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetPaginatedInterviwerByAll?page=${pageNumber}&pageSize=${pageSize}&sortOrder=${sort}&jobRoleId=${jobRoleId}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch interviewers with pagination when jobRoleId is not 0, roundId is 0, and searchQuery is not null', () => {
    // Arrange
    const pageNumber = 1;
    const pageSize = 20;
    const sort = 'asc';
    const jobRoleId = 1;
    const roundId = 0;
    const searchQuery = 'Smith';

    const mockInterviewers: InterviewSlots[] = [
      {
        slotId: 1, employeeId: 1, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      },
      {
        slotId: 2, employeeId: 2, slotDate: '2024-07-18',
        timeslotId: 0,
        isBooked: false,
        employee: {
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
            jobRoleId: 0,
            jobRoleName: ''
          },
          interviewRound: {
            interviewRoundId: 0,
            interviewRoundName: ''
          }
        },
        timeslot: {
          timeslotId: 0,
          timeslotName: ''
        }
      }
    ]; 
    const mockApiResponse: ApiResponse<InterviewSlots[]> = {
      success: true,
      data: mockInterviewers,
      message: ''
    };

    // Act
    service.getAllInterviwersWithPaginationByAll(pageNumber, pageSize, sort, searchQuery, jobRoleId, roundId)
      .subscribe(response => {
        // Assert
        expect(response.success).toBeTrue();
        expect(response.data).toEqual(mockApiResponse.data);
        expect(response.message).toBe('');
      });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetPaginatedInterviwerByAll?page=${pageNumber}&pageSize=${pageSize}&searchQuery=${searchQuery}&sortOrder=${sort}&jobRoleId=${jobRoleId}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch total interview slots when jobRoleId is 0, roundId is not 0, and searchQuery is not null or empty', () => {
    // Arrange
    const jobRoleId = 0; 
    const roundId = 2; 
    const searchQuery = 'Testt'; 

    const mockTotalInterviewSlots = 30;
    const mockApiResponse: ApiResponse<number> = {
      success: true,
      data: mockTotalInterviewSlots,
      message: ''
    };

    // Act
    service.getTotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId)
      .subscribe(response => {
        // Assert
        expect(response.success).toBeTrue();
        expect(response.data).toEqual(mockApiResponse.data);
        expect(response.message).toBe('');
      });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetTotalInterviewSlotsByAll?searchQuery=${searchQuery}&roundId=${roundId}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch total interview slots when jobRoleId is not null, roundId is not null, and searchQuery is null', () => {
    // Arrange
    const jobRoleId = 1;
    const roundId = 2;
    const searchQuery = '';

    const mockTotalInterviewSlots = 20; 
    const mockApiResponse: ApiResponse<number> = {
      success: true,
      data: mockTotalInterviewSlots,
      message: ''
    };

    // Act
    service.getTotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId)
      .subscribe(response => {
        // Assert
        expect(response.success).toBeTrue();
        expect(response.data).toEqual(mockApiResponse.data);
        expect(response.message).toBe('');
      });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetTotalInterviewSlotsByAll?jobRoleId=${jobRoleId}&roundId=${roundId}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch total interview slots when jobRoleId is not null, roundId is not null, and searchQuery is not null', () => {
    // Arrange
    const jobRoleId = 1;
    const roundId = 2;
    const searchQuery = 'Doe';

    const mockTotalInterviewSlots = 25; 
    const mockApiResponse: ApiResponse<number> = {
      success: true,
      data: mockTotalInterviewSlots,
      message: ''
    };

    // Act
    service.getTotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId)
      .subscribe(response => {
        // Assert
        expect(response.success).toBeTrue();
        expect(response.data).toEqual(mockApiResponse.data);
        expect(response.message).toBe('');
      });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetTotalInterviewSlotsByAll?searchQuery=${searchQuery}&jobRoleId=${jobRoleId}&roundId=${roundId}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch total interview slots when jobRoleId is 0, roundId is not 0, and searchQuery is null', () => {
    // Arrange
    const jobRoleId = 0; 
    const roundId = 2; 
    const searchQuery = ''; 

    const mockTotalInterviewSlots = 25;
    const mockApiResponse: ApiResponse<number> = {
      success: true,
      data: mockTotalInterviewSlots,
      message: ''
    };

    // Act
    service.getTotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId)
      .subscribe(response => {
        // Assert
        expect(response.success).toBeTrue();
        expect(response.data).toEqual(mockApiResponse.data);
        expect(response.message).toBe('');
      });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetTotalInterviewSlotsByAll?roundId=${roundId}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch total interview slots when jobRoleId is not null, roundId is null, and searchQuery is null', () => {
    // Arrange
    const jobRoleId = 1;
    const roundId = null;
    const searchQuery = '';
  
    const mockTotalInterviewSlots = 40; 
    const mockApiResponse: ApiResponse<number> = {
      success: true,
      data: mockTotalInterviewSlots,
      message: ''
    };
  
    // Act
    service.getTotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId)
      .subscribe(response => {
        // Assert
        expect(response.success).toBeTrue();
        expect(response.data).toEqual(mockApiResponse.data);
        expect(response.message).toBe('');
      });
  
    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetTotalInterviewSlotsByAll`;
    const req = httpMock.expectOne(req => req.url.startsWith(expectedUrl));
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch total interview slots when jobRoleId is not 0, roundId is 0, and searchQuery is empty string', () => {
    // Arrange
    const jobRoleId = 1; 
    const roundId = 0; 
    const searchQuery = ''; 

    const mockTotalInterviewSlots = 25;
    const mockApiResponse: ApiResponse<number> = {
      success: true,
      data: mockTotalInterviewSlots,
      message: ''
    };

    // Act
    service.getTotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId)
      .subscribe(response => {
        // Assert
        expect(response.success).toBeTrue();
        expect(response.data).toEqual(mockApiResponse.data);
        expect(response.message).toBe('');
      });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetTotalInterviewSlotsByAll?jobRoleId=${jobRoleId}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });
  
  it('should fetch total interview slots when jobRoleId is not 0, roundId is 0, and searchQuery is not null or empty', () => {
    // Arrange
    const jobRoleId = 1; 
    const roundId = 0; 
    const searchQuery = 'Testt'; 

    const mockTotalInterviewSlots = 25;
    const mockApiResponse: ApiResponse<number> = {
      success: true,
      data: mockTotalInterviewSlots,
      message: ''
    };

    // Act
    service.getTotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId)
      .subscribe(response => {
        // Assert
        expect(response.success).toBeTrue();
        expect(response.data).toEqual(mockApiResponse.data);
        expect(response.message).toBe('');
      });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetTotalInterviewSlotsByAll?searchQuery=${searchQuery}&jobRoleId=${jobRoleId}`;
    const req = httpMock.expectOne(expectedUrl);

    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should fetch total interview slots when jobRoleId is 0, roundId is 0, and searchQuery is not null or empty', () => {
    // Arrange
    const jobRoleId = 0;
    const roundId = 0;
    const searchQuery = 'Testtt';

    const mockTotalInterviewSlots = 25;
    const mockApiResponse: ApiResponse<number> = {
      success: true,
      data: mockTotalInterviewSlots,
      message: ''
    };

    // Act
    service.getTotalInterviewSlotsByAll(searchQuery, jobRoleId, roundId)
      .subscribe(response => {
        // Assert
        expect(response.success).toBeTrue();
        expect(response.data).toEqual(mockApiResponse.data);
        expect(response.message).toBe('');
      });

    // Assert
    const expectedUrl = `http://localhost:5263/api/Recruiter/GetTotalInterviewSlotsByAll?searchQuery=${searchQuery}`;
    const req = httpMock.expectOne(expectedUrl);

    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });
});
