import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobrolebasedalldetailreportComponent } from './jobrolebasedalldetailreport.component';
import { ReportService } from 'src/app/services/report.service';
import { Router } from '@angular/router';
import { JobRole } from 'src/app/models/jobrole.model';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { InterviewerService } from 'src/app/services/interviewer.service';
import { of, throwError } from 'rxjs';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { DetailedReport } from 'src/app/models/alldetailreport.model';

describe('JobrolebasedalldetailreportComponent', () => {
  let component: JobrolebasedalldetailreportComponent;
  let fixture: ComponentFixture<JobrolebasedalldetailreportComponent>;
  let reportService: ReportService;
  let interviewerService:InterviewerService;
  let router: Router;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule,FormsModule],
      declarations: [JobrolebasedalldetailreportComponent]
    });
    fixture = TestBed.createComponent(JobrolebasedalldetailreportComponent);
    component = fixture.componentInstance;
    reportService = TestBed.inject(ReportService);
    interviewerService = TestBed.inject(InterviewerService);
    router = TestBed.inject(Router);
    
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch job role when component initialize', () => {
    // Test ngOnInit
    //Arrange
    const mockJobRoles : JobRole[]=[
      {
        jobRoleId:1,
        jobRoleName:'Developer'
      },
      {
        jobRoleId:2,
        jobRoleName:'Tester'
      },
      {
        jobRoleId:1,
        jobRoleName:'Business Analysis'
      }
    ]
    const mockApiRespone :ApiResponse<JobRole[]>={
      success:true,
      data:mockJobRoles,
      message:''
    };
    spyOn(interviewerService,'getAllJobRoles').and.returnValue(of(mockApiRespone));

    //Act
    component.ngOnInit();

    //Assert
    expect(component.jobrole).toEqual(mockJobRoles);
    expect(component.loading).toEqual(false);
  });

  it('should handle when success resposne is false while fetching job role when component initialize', () => {
    // Test ngOnInit
    //Arrange
    const mockApiRespone :ApiResponse<JobRole[]>={  
      success:false,
      data:[],
      message:'Something went wrong, please try after sometime'
    };
    spyOn(console,'error');
    spyOn(interviewerService,'getAllJobRoles').and.returnValue(of(mockApiRespone));

    //Act
    component.ngOnInit();

    //Assert
    expect(console.error).toHaveBeenCalledWith('Failed to fetch job role',mockApiRespone.message);
    expect(component.loading).toEqual(false);
  });

  it('should handle when error response while fetching job role when component initialize', () => {
    // Test ngOnInit
    //Arrange
    const errorMessage:String ='Error fetching job role';
    spyOn(console,'error');
    spyOn(interviewerService,'getAllJobRoles').and.returnValue(throwError(errorMessage));

    //Act
    component.ngOnInit();

    //Assert
    expect(console.error).toHaveBeenCalledWith('Error fetching job role',errorMessage);
    expect(component.loading).toEqual(false);
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
    spyOn(reportService,'getDetailedReportBasedOnJobRole').and.returnValue(of(mockApiRespone));

    //Act
    component.selectedJobRoleId=1;
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
    spyOn(reportService,'getDetailedReportBasedOnJobRole').and.returnValue(of(mockApiRespone));

    //Act
    component.selectedJobRoleId=1;
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
    spyOn(reportService,'getDetailedReportBasedOnJobRole').and.returnValue(throwError(errorMessage));

    //Act
    component.selectedJobRoleId=1;
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
    spyOn(reportService,'getDetailedReportCountBasedOnJobRole').and.returnValue(of(mockApiRespone));
    spyOn(component,'loadReportDetails');

    //Act
    component.selectedJobRoleId=1;
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
    spyOn(reportService,'getDetailedReportCountBasedOnJobRole').and.returnValue(of(mockApiRespone));
    spyOn(console,'error');

    //Act
    component.selectedJobRoleId=1;
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
    spyOn(reportService,'getDetailedReportCountBasedOnJobRole').and.returnValue(throwError(errorMessage));
    spyOn(console,'error');
    //Act
    component.selectedJobRoleId=1;
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
