import { TestBed } from '@angular/core/testing';

import { ReportService } from './report.service';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ApiResponse } from '../models/ApiResponse{T}';
import { DetailedReport } from '../models/alldetailreport.model';
import { SlotsReport } from '../models/slotcountreport.model';

describe('ReportService', () => {
  let service: ReportService;
  let httpMock : HttpTestingController;
  const mockApiResponse : ApiResponse<DetailedReport[]> ={
    success : true,
    data:[
      {
        employeeId:1,
        timeslotId:1,
        firstName:'firstName1',
        lastName:'lastName1',
        email:'user1@gmail.com',
        slotDate:'07/17/2024',
        timeSlotName:'10:00AM-11:00AM'
      },
      {
        employeeId:2,
        timeslotId:2,
        firstName:'firstName2',
        lastName:'lastName2',
        email:'user2@gmail.com',
        slotDate:'07/18/2024',
        timeSlotName:'11:00AM-12:00PM'
      }
    ],
    message:''
  }

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, HttpClientTestingModule],
      providers:[ReportService]
    });
    service = TestBed.inject(ReportService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(()=>{
    httpMock.verify();
  });

  it('should get detailed report based on Job role',()=>{
    //Arrange
    const jobroleId:number = 1;
    const booked:boolean = false;
    const page:number = 1;
    const pageSize:number = 6;
    const apiUrl = 'http://localhost:5263/api/Report/';

    //Act
    service.getDetailedReportBasedOnJobRole(jobroleId,booked,page,pageSize).subscribe((response)=>{
      //Assert
      expect(response.success).toBe(true);
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);
    });
      const req = httpMock.expectOne(apiUrl + "ReportDetails?jobRoleId=" + jobroleId + "&booked=" + booked + "&page=" + page + "&pageSize=" + pageSize);
      expect(req.request.method).toBe('GET');
      req.flush(mockApiResponse);
  })
    


  //getDetailedReportBasedOnJobRole
  it('should get detailed report based on Job role',()=>{
    //Arrange
    const jobroleId:number = 1;
    const booked:boolean = false;
    const page:number = 1;
    const pageSize:number = 6;
    const apiUrl = 'http://localhost:5263/api/Report/';
 
    //Act
    service.getDetailedReportBasedOnJobRole(jobroleId,booked,page,pageSize).subscribe((response)=>{
      //Assert
      expect(response.success).toBe(true);
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);
    });
 
    const req = httpMock.expectOne(apiUrl+"ReportDetails?jobRoleId="+jobroleId+"&booked="+booked+"&page="+page+"&pageSize="+pageSize);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  })
 
  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should handle HTTP error gracefully', () => {
    //Arrange
    const jobroleId:number = 1;
    const booked:boolean = false;
    const page:number = 1;
    const pageSize:number = 6;
    const apiUrl = 'http://localhost:5263/api/Report/';
    const errorMessage = 'Failed to load job roles';
    //Act
    service.getDetailedReportBasedOnJobRole(jobroleId,booked,page,pageSize).subscribe(
      () => fail('expected an error, not job roles'),
      (error) => {
        //Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    );
    
    const req = httpMock.expectOne(apiUrl+"ReportDetails?jobRoleId="+jobroleId+"&booked="+booked+"&page="+page+"&pageSize="+pageSize);
    expect(req.request.method).toBe('GET');
    //Respond with error
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
  });



  //getSlotsCountReportBasedOnJobRole
  it('should fetch job role by id successfully', () =>{
    //Arrange
    const jobroleId=1;
    const mockSuccessResponse:ApiResponse<SlotsReport>={
      success:true,
      data:{
        availableSlot: null,
        bookedSlot: null
      },
      message:''
    };
    //Act
    service.getSlotsCountReportBasedOnJobRole(jobroleId).subscribe(response=>{
      //Assert
      expect(response.success).toBeTrue();
      expect(response.message).toBe('');
      expect(response.data).toEqual(mockSuccessResponse.data);
    });

    const req=httpMock.expectOne('http://localhost:5263/api/Report/SlotsReport?jobRoleId='+jobroleId);
    expect(req.request.method).toBe('GET');
    req.flush(mockSuccessResponse);

  });

  it('should handle failed job role retrival', () =>{
    //Arrange
    const jobroleId=1;
    const mockErrorResponse:ApiResponse<SlotsReport>={
      success:false,
      data:{} as SlotsReport,
      message:'No record found!'
    };
    //Act
    service.getSlotsCountReportBasedOnJobRole(jobroleId).subscribe(response=>{
      //Assert
      expect(response).toEqual(mockErrorResponse);
      expect(response.message).toBe('No record found!');
      expect(response.success).toBeFalse();
    });

    const req=httpMock.expectOne('http://localhost:5263/api/Report/SlotsReport?jobRoleId='+jobroleId);
    expect(req.request.method).toBe('GET');
    req.flush(mockErrorResponse);

  });

  it('should handle HTTP error', () =>{
    //Arrange
    const jobroleId=1;
    const mockHttpError={
      status:500,
      statusText:'Internal Server Error'
    };
    //Act
    service.getSlotsCountReportBasedOnJobRole(jobroleId).subscribe({
      next: () => fail('should have failed with 500 error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual('Internal Server Error')
      }
    });

    const req=httpMock.expectOne('http://localhost:5263/api/Report/SlotsReport?jobRoleId='+jobroleId);
    expect(req.request.method).toBe('GET');
    req.flush({}, mockHttpError);   

  });


  //getSlotsCountReportBasedOnInterviewRound
  it('should fetch interview round by id successfully', () =>{
    //Arrange
    const interViewRoundId=1;
    const mockSuccessResponse:ApiResponse<SlotsReport>={
      success:true,
      data:{
        availableSlot: null,
        bookedSlot: null
      },
      message:''
    };
    //Act
    service.getSlotsCountReportBasedOnInterviewRound(interViewRoundId).subscribe(response=>{
      //Assert
      expect(response.success).toBeTrue();
      expect(response.message).toBe('');
      expect(response.data).toEqual(mockSuccessResponse.data);
    });

    const req=httpMock.expectOne('http://localhost:5263/api/Report/SlotsReport?interViewRoundId='+interViewRoundId);
    expect(req.request.method).toBe('GET');
    req.flush(mockSuccessResponse);

  });

  it('should handle failed interview round retrival', () =>{
    //Arrange
    const interViewRoundId=1;
    const mockErrorResponse:ApiResponse<SlotsReport>={
      success:false,
      data:{} as SlotsReport,
      message:'No record found!'
    };
    //Act
    service.getSlotsCountReportBasedOnInterviewRound(interViewRoundId).subscribe(response=>{
      //Assert
      expect(response).toEqual(mockErrorResponse);
      expect(response.message).toBe('No record found!');
      expect(response.success).toBeFalse();
    });

    const req=httpMock.expectOne('http://localhost:5263/api/Report/SlotsReport?interViewRoundId='+interViewRoundId);
    expect(req.request.method).toBe('GET');
    req.flush(mockErrorResponse);

  });

  it('should handle HTTP error', () =>{
    //Arrange
    const interViewRoundId=1;
    const mockHttpError={
      status:500,
      statusText:'Internal Server Error'
    };
    //Act
    service.getSlotsCountReportBasedOnInterviewRound(interViewRoundId).subscribe({
      next: () => fail('should have failed with 500 error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual('Internal Server Error')
      }
    });

    const req=httpMock.expectOne('http://localhost:5263/api/Report/SlotsReport?interViewRoundId='+interViewRoundId);
    expect(req.request.method).toBe('GET');
    req.flush({}, mockHttpError);   

  });


  //getSlotsCountReportBasedOnDateRange
  it('should fetch slots count based on date range successfully', () =>{
    //Arrange
    const startDate = '';
    const endDate = '';
    const mockSuccessResponse:ApiResponse<SlotsReport>={
      success:true,
      data:{
        availableSlot: null,
        bookedSlot: null
      },
      message:''
    };
    //Act
    service.getSlotsCountReportBasedOnDateRange(startDate, endDate).subscribe(response=>{
      //Assert
      expect(response.success).toBeTrue();
      expect(response.message).toBe('');
      expect(response.data).toEqual(mockSuccessResponse.data);
    });

    const req=httpMock.expectOne('http://localhost:5263/api/Report/SlotsReport?startDate=&endDate='+startDate, endDate);
    expect(req.request.method).toBe('GET');
    req.flush(mockSuccessResponse);

  });

  it('should handle failed slots count retrival', () =>{
    //Arrange
    const startDate = '';
    const endDate = '';
    const mockErrorResponse:ApiResponse<SlotsReport>={
      success:false,
      data:{} as SlotsReport,
      message:'No record found!'
    };
    //Act
    service.getSlotsCountReportBasedOnDateRange(startDate, endDate).subscribe(response=>{
      //Assert
      expect(response).toEqual(mockErrorResponse);
      expect(response.message).toBe('No record found!');
      expect(response.success).toBeFalse();
    });

    const req=httpMock.expectOne('http://localhost:5263/api/Report/SlotsReport?startDate=&endDate='+startDate, endDate);
    expect(req.request.method).toBe('GET');
    req.flush(mockErrorResponse);

  });

  it('should handle HTTP error', () =>{
    //Arrange
    const startDate = '';
    const endDate = '';
    const mockHttpError={
      status:500,
      statusText:'Internal Server Error'
    };
    //Act
    service.getSlotsCountReportBasedOnDateRange(startDate, endDate).subscribe({
      next: () => fail('should have failed with 500 error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual('Internal Server Error')
      }
    });

    const req=httpMock.expectOne('http://localhost:5263/api/Report/SlotsReport?startDate=&endDate='+startDate, endDate);
    expect(req.request.method).toBe('GET');
    req.flush({}, mockHttpError);   

  });


  //getDetailedReportCountBasedOnJobRole
  it('should fetch detailed report based on job role successfully', () => {
    // Arrange
    const jobroleId = 1;
    const booked = false;
    const mockApiResponse = { data: 2 };
  
    // Act
    service.getDetailedReportCountBasedOnJobRole(jobroleId, booked).subscribe((response) => {
      // Assert
      expect(response.data).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);
    });
  
    // Expect a single HTTP GET request and respond with mockApiResponse
    const req = httpMock.expectOne(`http://localhost:5263/api/Report/TotalReportDetailCount?jobRoleId=${jobroleId}&booked=${booked}`);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });
  

  it('should handle an empty job role list', () => {
    //Arrange
    const jobroleId = 1;
    const booked = false;
    const mockApiResponse = { data: 0 }; 
    const emptyResponse: ApiResponse<number> = {
      success: true,
      data: 0,
      message: ''
    }
    //Act
    service.getDetailedReportCountBasedOnJobRole(jobroleId, booked).subscribe((response) => {
      //Assert
      expect(response.data).toBe(0);
      expect(response.data).toEqual(mockApiResponse.data);
    });
    const req = httpMock.expectOne(`http://localhost:5263/api/Report/TotalReportDetailCount?jobRoleId=${jobroleId}&booked=${booked}`);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);
  });

  it('should handle HTTP error gracefully', () => {
    //Arrange
    const jobroleId = 1;
    const booked = false;
    const errorMessage = 'Failed to load contacts';
    //Act
    service.getDetailedReportCountBasedOnJobRole(jobroleId, booked).subscribe(
      () => fail('expected an error, not contacts'),
      (error) => {
        //Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    );

    const req = httpMock.expectOne(`http://localhost:5263/api/Report/TotalReportDetailCount?jobRoleId=${jobroleId}&booked=${booked}`);
    expect(req.request.method).toBe('GET');
    //Respond with error
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
  });


  //getDetailedReportBasedOnInterviewRound
  it('should fetch interview rounds successfully', () =>{
    //Arrange
    const interViewRoundId = 1;
    const booked = false;
    const page = 1;
    const pageSize = 10;
    const mockSuccessResponse:ApiResponse<DetailedReport[]>={
      success:true,
      data:[
        {employeeId: 0,
        timeslotId: 0,
        firstName: '',
        lastName: '',
        email: '',
        slotDate: '',
        timeSlotName: ''}
    ],
      message:''
    };
    //Act
    service.getDetailedReportBasedOnInterviewRound(interViewRoundId, booked, page, pageSize).subscribe(response=>{
      //Assert
      expect(response.success).toBeTrue();
      expect(response.message).toBe('');
      expect(response.data).toEqual(mockSuccessResponse.data);
    });

    const req=httpMock.expectOne(`http://localhost:5263/api/Report/ReportDetails?interViewRoundId=${interViewRoundId}&booked=${booked}&page=${page}&pageSize=${pageSize}`);
    expect(req.request.method).toBe('GET');
    req.flush(mockSuccessResponse);

  });

  it('should handle failed interview rounds retrival', () =>{
    //Arrange
    const interViewRoundId = 1;
    const booked = false;
    const page = 1;
    const pageSize = 10;
    const mockErrorResponse:ApiResponse<DetailedReport[]>={
      success:false,
      data:{} as DetailedReport[],
      message:'No record found!'
    };
    //Act
    service.getDetailedReportBasedOnInterviewRound(interViewRoundId, booked, page, pageSize).subscribe(response=>{
      //Assert
      expect(response).toEqual(mockErrorResponse);
      expect(response.message).toBe('No record found!');
      expect(response.success).toBeFalse();
    });

    const req=httpMock.expectOne(`http://localhost:5263/api/Report/ReportDetails?interViewRoundId=${interViewRoundId}&booked=${booked}&page=${page}&pageSize=${pageSize}`);
    expect(req.request.method).toBe('GET');
    req.flush(mockErrorResponse);

  });

  it('should handle HTTP error', () =>{
    //Arrange
    const interViewRoundId = 1;
    const booked = false;
    const page = 1;
    const pageSize = 10;
    const mockHttpError={
      status:500,
      statusText:'Internal Server Error'
    };
    //Act
    service.getDetailedReportBasedOnInterviewRound(interViewRoundId, booked, page, pageSize).subscribe({
      next: () => fail('should have failed with 500 error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual('Internal Server Error')
      }
    });

    const req=httpMock.expectOne(`http://localhost:5263/api/Report/ReportDetails?interViewRoundId=${interViewRoundId}&booked=${booked}&page=${page}&pageSize=${pageSize}`);
    expect(req.request.method).toBe('GET');
    req.flush({}, mockHttpError);   

  });



  //getDetailedReportCountBasedOnInterviewRound
  it('should fetch detailed report based on interview round successfully', () => {
    // Arrange
    const interViewRoundId = 1;
    const booked = false;
    const mockApiResponse = { data: 2 };
  
    // Act
    service.getDetailedReportCountBasedOnInterviewRound(interViewRoundId, booked).subscribe((response) => {
      // Assert
      expect(response.data).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);
    });
  
    // Expect a single HTTP GET request and respond with mockApiResponse
    const req = httpMock.expectOne(`http://localhost:5263/api/Report/TotalReportDetailCount?interViewRoundId=${interViewRoundId}&booked=${booked}`);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });
  

  it('should handle an empty interview round list', () => {
    //Arrange
    const interViewRoundId = 1;
    const booked = false;
    const mockApiResponse = { data: 0 }; 
    const emptyResponse: ApiResponse<number> = {
      success: true,
      data: 0,
      message: ''
    }
    //Act
    service.getDetailedReportCountBasedOnInterviewRound(interViewRoundId, booked).subscribe((response) => {
      //Assert
      expect(response.data).toBe(0);
      expect(response.data).toEqual(mockApiResponse.data);
    });
    const req = httpMock.expectOne(`http://localhost:5263/api/Report/TotalReportDetailCount?interViewRoundId=${interViewRoundId}&booked=${booked}`);
    expect(req.request.method).toBe('GET');
    req.flush(emptyResponse);
  });

  it('should handle HTTP error gracefully', () => {
    //Arrange
    const interViewRoundId = 1;
    const booked = false;
    const errorMessage = 'Failed to load contacts';
    //Act
    service.getDetailedReportCountBasedOnInterviewRound(interViewRoundId, booked).subscribe(
      () => fail('expected an error, not contacts'),
      (error) => {
        //Assert
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    );

    const req = httpMock.expectOne(`http://localhost:5263/api/Report/TotalReportDetailCount?interViewRoundId=${interViewRoundId}&booked=${booked}`);
    expect(req.request.method).toBe('GET');
    //Respond with error
    req.flush(errorMessage, { status: 500, statusText: 'Internal Server Error' });
  });



  //getDetailedReportBasedOnDateRange
  it('should fetch detailed report based on date range successfully', () => {
    // Arrange
    const startDate = '';
    const endDate = '';
    const booked = false;
    const page = 1;
    const pageSize = 10;

    const mockResponse: ApiResponse<DetailedReport[]> = {
      success: true,
      data: [
        {
          employeeId: 1,
          timeslotId: 1,
          firstName: '',
          lastName: '',
          email: '',
          slotDate: '',
          timeSlotName: ''
        }
      ],
      message: ''
    };

    // Act
    service.getDetailedReportBasedOnDateRange(startDate, endDate, booked, page, pageSize).subscribe((response) => {
      // Assert
      expect(response).toBeTruthy();
      expect(response.success).toBeTrue();
      expect(response.data.length).toBe(1);
    });

    const expectedUrl = `http://localhost:5263/api/Report/ReportDetails?startDate=${startDate}&endDate=${endDate}&booked=${booked}&page=${page}&pageSize=${pageSize}`;
    const req = httpMock.expectOne(expectedUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });
  

  //getDetailedReportCountBasedOnDateRange
  it('should fetch slots count based on date range successfully', () =>{
    //Arrange
    const startDate = '';
    const endDate = '';
    const booked = false;
    const mockApiResponse = { data: 2 };
    //Act
    service.getDetailedReportCountBasedOnDateRange(startDate, endDate, booked).subscribe(response=>{
      //Assert
      expect(response.data).toEqual(mockApiResponse.data);
    });

    const req=httpMock.expectOne(`http://localhost:5263/api/Report/TotalReportDetailCount?startDate=${startDate}&endDate=${endDate}&booked=${booked}`);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);

  });

  

  it('should handle HTTP error', () =>{
    //Arrange
    const startDate = '';
    const endDate = '';
    const booked = false;
    const mockHttpError={
      status:500,
      statusText:'Internal Server Error'
    };
    //Act
    service.getDetailedReportCountBasedOnDateRange(startDate, endDate, booked).subscribe({
      next: () => fail('should have failed with 500 error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual('Internal Server Error')
      }
    });

    const req=httpMock.expectOne(`http://localhost:5263/api/Report/TotalReportDetailCount?startDate=${startDate}&endDate=${endDate}&booked=${booked}`);
    expect(req.request.method).toBe('GET');
    req.flush({}, mockHttpError);   

  });
});
