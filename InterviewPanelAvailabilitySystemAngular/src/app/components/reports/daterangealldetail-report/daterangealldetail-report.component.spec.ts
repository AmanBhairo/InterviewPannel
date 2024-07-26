import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DaterangealldetailReportComponent } from './daterangealldetail-report.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { InterviewerService } from 'src/app/services/interviewer.service';
import { ReportService } from 'src/app/services/report.service';
import { of, throwError } from 'rxjs';
import { DetailedReport } from 'src/app/models/alldetailreport.model';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';

describe('DaterangealldetailReportComponent', () => {
  let component: DaterangealldetailReportComponent;
  let fixture: ComponentFixture<DaterangealldetailReportComponent>;
  let reportService: ReportService;
  let interviewerService:InterviewerService;
  let router: Router;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule,FormsModule],
      declarations: [DaterangealldetailReportComponent]
    });
    fixture = TestBed.createComponent(DaterangealldetailReportComponent);
    component = fixture.componentInstance;
    reportService = TestBed.inject(ReportService);
    interviewerService = TestBed.inject(InterviewerService);
    router = TestBed.inject(Router);

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch report data  ', () => {
    //Arrange
    const mockReportDetails : DetailedReport[]=[
      {
        employeeId:1,
        firstName:'firstName1',
        lastName:'lastName1',
        email:'user1@gmail.com',
        timeslotId:1,
        timeSlotName:'10:00AM-11:00AM',
        slotDate:'2024-07-16'
      },
      {
        employeeId:2,
        firstName:'firstName2',
        lastName:'lastName2',
        email:'user2@gmail.com',
        timeslotId:2,
        timeSlotName:'10:00AM-11:00AM',
        slotDate:'2024-08-16'
      }
    ]
    const mockApiRespone :ApiResponse<DetailedReport[]>={
      success:true,
      data:mockReportDetails,
      message:''
    };
    spyOn(reportService,'getDetailedReportBasedOnDateRange').and.returnValue(of(mockApiRespone));

    //Act
    component.startDate='07-15-2024';
    component.endDate='07-17-2024';
    component.booked=false;
    component.pageNumber=1;
    component.pageSize=6;
    component.loadReportDetails();

    //Assert
    expect(component.reportDetails).toEqual(mockReportDetails);
    expect(component.loading).toEqual(false);
  });

  it('should handle when success response is false while fetching report data  ', () => {
    //Arrange
    
    const mockApiRespone :ApiResponse<DetailedReport[]>={
      success:false,
      data:[],
      message:'No record found'
    };
    spyOn(console,'error');
    spyOn(reportService,'getDetailedReportBasedOnDateRange').and.returnValue(of(mockApiRespone));

    //Act
    component.startDate='07-15-2024';
    component.endDate='07-17-2024';
    component.booked=false;
    component.pageNumber=1;
    component.pageSize=6;
    component.loadReportDetails();

    //Assert
    expect(console.error).toHaveBeenCalledOnceWith('No record available',mockApiRespone.message);
    expect(component.loading).toEqual(false);
  });

  it('should handle error response while fetching report data  ', () => {
    //Arrange
    
    const errorMessage:string='No record found';
    spyOn(console,'error');
    spyOn(reportService,'getDetailedReportBasedOnDateRange').and.returnValue(throwError(errorMessage));

    //Act
    component.startDate='07-15-2024';
    component.endDate='07-17-2024';
    component.booked=false;
    component.pageNumber=1;
    component.pageSize=6;
    component.loadReportDetails();

    //Assert
    expect(component.reportDetails).toEqual([])
    expect(console.error).toHaveBeenCalledOnceWith('Error fetching reports',errorMessage);
    expect(component.loading).toEqual(false);
  });

  it('should fetch ReportsDetailCount  ', () => {
    //Arrange
    const mockReportDetailsCount : number = 4;
    const mockApiRespone :ApiResponse<number>={
      success:true,
      data:mockReportDetailsCount,
      message:''
    };
    spyOn(reportService,'getDetailedReportCountBasedOnDateRange').and.returnValue(of(mockApiRespone));
    spyOn(component,'loadReportDetails');

    //Act
    component.startDate='07-15-2024';
    component.endDate='07-17-2024';
    component.booked=false;
    component.pageNumber=1;
    component.pageSize=6;
    component.getReportsDetailCount();

    //Assert
    expect(component.totalItems).toEqual(mockReportDetailsCount);
    expect(component.loading).toEqual(false);
    expect(component.loadReportDetails).toHaveBeenCalled();
  });

  it('should handle false success response while fetching ReportsDetailCount  ', () => {
    //Arrange
    const mockApiRespone :ApiResponse<number>={
      success:false,
      data:0,
      message:'Failed to fetch count'
    };
    spyOn(reportService,'getDetailedReportCountBasedOnDateRange').and.returnValue(of(mockApiRespone));
    spyOn(console,'error');

    //Act
    component.startDate='07-15-2024';
    component.endDate='07-17-2024';
    component.booked=false;
    component.pageNumber=1;
    component.pageSize=6;
    component.getReportsDetailCount();

    //Assert
    expect(console.error).toHaveBeenCalledOnceWith('Failed to fetch reports count',mockApiRespone.message)
    expect(component.loading).toEqual(false);
  });

  it('should handle error response while fetching ReportsDetailCount  ', () => {
    //Arrange
    const errorMessage :string='Failed to fetch count';
    spyOn(reportService,'getDetailedReportCountBasedOnDateRange').and.returnValue(throwError(errorMessage));
    spyOn(console,'error');
    //Act
    component.startDate='07-15-2024';
    component.endDate='07-17-2024';
    component.booked=false;
    component.pageNumber=1;
    component.pageSize=6;
    component.getReportsDetailCount();

    //Assert
    expect(console.error).toHaveBeenCalledOnceWith('Error fetching reports count.',errorMessage)
    expect(component.loading).toEqual(false);
  });

  it('should call changePageSize',()=>{
    //Arrange
    spyOn(component,'loadReportDetails');

    //Act
    const pageSize =4;
    component.totalItems=20;
    component.changePageSize(pageSize);

    //Assert
    expect(component.pageSize).toEqual(pageSize);
    expect(component.totalPages).toEqual(5);
    expect(component.loadReportDetails).toHaveBeenCalled();
  });

  it('should call changePage',()=>{
    //Arrange
    spyOn(component,'loadReportDetails');
    spyOn(component,'getReportsDetailCount');

    //Act
    const pageNumber =3;
    component.changePage(pageNumber);

    //Assert
    expect(component.pageNumber).toEqual(pageNumber);
    expect(component.getReportsDetailCount).toHaveBeenCalled();
    expect(component.loadReportDetails).toHaveBeenCalled();

  });

  it('should call OnJobRoleChange',()=>{
    //Arrange
    spyOn(component,'getReportsDetailCount');

    //Act
    component.OnJobRoleChange();

    //Assert
    expect(component.getReportsDetailCount).toHaveBeenCalled();
  });

  it('should call slotdecider',()=>{
    //Arrange
    spyOn(component,'getReportsDetailCount');

    //Act
    const slot:boolean = false;
    component.slotdecider(slot);

    //Assert
    expect(component.booked).toEqual(false);
    expect(component.getReportsDetailCount).toHaveBeenCalled();
  });

  it('should call OnStartDateChange',()=>{
    //Act
    component.OnStartDateChange();

    //Assert
    expect(component.endDate).toEqual('');
    expect(component.reportDetails).toEqual([]);
    expect(component.totalItems).toEqual(0);
    expect(component.minEndDate).toEqual(component.startDate);

  });

  it('should call OnEndDateChange',()=>{
    //Act
    component.startDate='07-17-2024';
    component.endDate='07-17-2024';
    spyOn(component,'getReportsDetailCount');
    component.OnEndDateChange();
    
    //Assert
    expect(component.getReportsDetailCount).toHaveBeenCalled();

  });

  it('should call slotdecider and change page number to 1 if booked value is changed',()=>{
    //Arrange
    spyOn(component,'getReportsDetailCount');
    component.booked = true

    //Act
    const slot:boolean = false;
    component.slotdecider(slot);

    //Assert
    expect(component.booked).toEqual(false);
    expect(component.getReportsDetailCount).toHaveBeenCalled();
    expect(component.pageNumber).toEqual(1);
  });
});
