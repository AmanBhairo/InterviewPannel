import { TestBed } from '@angular/core/testing';

import { InterviewerPanelService } from './interviewer-panel.service';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Timeslot } from '../models/timeslot.model';
import { ApiResponse } from '../models/ApiResponse{T}';
import { InterviewSlots } from '../models/interviewSlots.model';
import { AddInterviewSlot } from '../models/add-interview-slot.model';

describe('InterviewerPanelService', () => {
  let service: InterviewerPanelService;
  let httpMock : HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, HttpClientTestingModule],
      providers:[InterviewerPanelService]
    });
    service = TestBed.inject(InterviewerPanelService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(()=>{
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  //getAllTimeslots
  it('should fetch all timeslots successfully', () => {
    // Arrange
    const mockTimeslots: Timeslot[] = [
      { timeslotId: 1, timeslotName: 'Timeslot 1' },
      { timeslotId: 2, timeslotName: 'Timeslot 2' }
    ];
    const mockApiResponse: ApiResponse<Timeslot[]> = {
      success: true,
      data: mockTimeslots,
      message: ''
    };

    // Act
    service.getAllTimeslots().subscribe(response => {
      // Assert
      expect(response.success).toBeTrue();
      expect(response.data.length).toBe(mockTimeslots.length);
      expect(response.data).toEqual(mockTimeslots);
    });
    const expectedUrl = `http://localhost:5263/api/Interviewer/GetAllTimeslots`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should handle getAllTimeSlots HTTP error gracefully', () => {
    //Arrange
   
    const apiUrl = `http://localhost:5263/api/Interviewer/GetAllTimeslots`;
    const errorMessage = 'Failed to load timeslots';
    //Act
    service.getAllTimeslots().subscribe(
      () => fail('expected an error, not timeslots'),
      (error) => {
        //Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    );

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
  });


  it('should handle error response when fetching timeslots', () => {
    // Arrange
    const mockErrorResponse = { status: 404, statusText: 'Not Found' };
  
    // Act
    service.getAllTimeslots().subscribe({
      error: (err) => {
        // Assert
        expect(err.status).toBe(404);
        expect(err.statusText).toBe('Not Found');
      }
    });
    const expectedUrl = `http://localhost:5263/api/Interviewer/GetAllTimeslots`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(null, mockErrorResponse);
  });


  //getAllInterviewslotsByEmployeeId
  it('should fetch interview slots successfully for a valid employee ID', () => {
    // Arrange
    const employeeId = 1;
    const mockInterviewSlots: InterviewSlots[] = [
      {
        slotId: 1,
        employeeId: 0,
        slotDate: '',
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
        slotId: 2,
        employeeId: 0,
        slotDate: '',
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
      data: mockInterviewSlots,
      message: ''
    };
  
    // Act
    service.getAllInterviewslotsByEmployeeId(employeeId).subscribe(response => {
      // Assert
      expect(response.success).toBeTrue();
      expect(response.data.length).toBe(mockInterviewSlots.length);
      expect(response.data).toEqual(mockInterviewSlots);
    });
  
    const expectedUrl = `http://localhost:5263/api/Interviewer/GetAllInterviewslots?employeeId=${employeeId}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });
  
  it('should handle error response when fetching interview slots', () => {
    // Arrange
    const employeeId = 1;
    const mockErrorResponse = { status: 404, statusText: 'Not Found' };
  
    // Act
    service.getAllInterviewslotsByEmployeeId(employeeId).subscribe({
      error: (err) => {
        // Assert
        expect(err.status).toBe(404);
        expect(err.statusText).toBe('Not Found');
      }
    });
    const expectedUrl = `http://localhost:5263/api/Interviewer/GetAllInterviewslots?employeeId=${employeeId}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(null, mockErrorResponse);
  });
  
  it('should handle getAllInterviewslotsByEmployeeId HTTP error gracefully', () => {
    //Arrange
    const employeeId=1;
    const apiUrl = `http://localhost:5263/api/Interviewer/GetAllInterviewslots?employeeId=`+employeeId;
    const errorMessage = 'Failed to load timeslots';
    //Act
    service.getAllInterviewslotsByEmployeeId(employeeId).subscribe(
      () => fail('expected an error, not timeslots'),
      (error) => {
        //Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    );

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
  });
  

  //addInterviewSlot
  it('should add interview slot successfully', () => {
    // Arrange
    const mockAddInterviewSlot: AddInterviewSlot = {
      employeeId: 1,
      slotDate: '2024-07-18',
      timeslotId: 0,
      isBooked: false
    };
    const mockApiResponse: ApiResponse<string> = {
      success: true,
      data: 'Interview slot added successfully.',
      message: ''
    };
  
    // Act
    service.addInterviewSlot(mockAddInterviewSlot).subscribe(response => {
      // Assert
      expect(response.success).toBeTrue();
      expect(response.data).toBe('Interview slot added successfully.');
      expect(response.message).toBe('');
    });
    const expectedUrl = `http://localhost:5263/api/Interviewer/AddInterviewSlot`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(mockAddInterviewSlot);
    req.flush(mockApiResponse);
  });
  
  it('should handle error response when adding interview slot', () => {
    // Arrange
    const mockAddInterviewSlot: AddInterviewSlot = {
      employeeId: 1,
      slotDate: undefined,
      timeslotId: 0,
      isBooked: false
    };
    const mockErrorResponse = { status: 500, statusText: 'Internal Server Error' };
  
    // Act
    service.addInterviewSlot(mockAddInterviewSlot).subscribe({
      error: (err) => {
        // Assert
        expect(err.status).toBe(500);
        expect(err.statusText).toBe('Internal Server Error');
      }
    });

    const expectedUrl = `http://localhost:5263/api/Interviewer/AddInterviewSlot`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('POST');
    req.flush(null, mockErrorResponse);
  });
  
 
  //deleteInterviewSlot
  it('should delete interview slot successfully', () => {
    // Arrange
    const id = 1;
    const slotDate = '2024-07-18'; 
    const timeslotId = 2;
    const mockApiResponse: ApiResponse<string> = {
      success: true,
      data: 'Interview slot deleted successfully.',
      message: ''
    };
  
    // Act
    service.deleteInterviewSlot(id, slotDate, timeslotId).subscribe(response => {
      // Assert
      expect(response.success).toBeTrue();
      expect(response.data).toBe('Interview slot deleted successfully.');
      expect(response.message).toBe('');
    });
  
    const expectedUrl = `http://localhost:5263/api/Interviewer/DeleteInterviewSlot${id}/?slotDate=${slotDate}&timeSlotId=${timeslotId}`;
    const req = httpMock.expectOne(req => req.url === expectedUrl && req.method === 'DELETE');
    req.flush(mockApiResponse);
  });
  

  it('should handle error response when deleting interview slot', () => {
    // Arrange
    const id = 1;
    const slotDate = '2024-07-18'; 
    const timeslotId = 2;
    const mockErrorResponse = { status: 500, statusText: 'Internal Server Error' };
  
    // Act
    service.deleteInterviewSlot(id, slotDate, timeslotId).subscribe({
      error: (err) => {
        // Assert
        expect(err.status).toBe(500);
        expect(err.statusText).toBe('Internal Server Error');
      }
    });
    const expectedUrl = `http://localhost:5263/api/Interviewer/DeleteInterviewSlot${id}/?slotDate=${slotDate}&timeSlotId=${timeslotId}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('DELETE');
    req.flush(null, mockErrorResponse);
  });
  
  
  
});
