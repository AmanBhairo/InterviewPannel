import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobroleReportComponent } from './jobrole-report.component';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { InterviewerService } from 'src/app/services/interviewer.service';
import { ReportService } from 'src/app/services/report.service';
import { of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { JobRole } from 'src/app/models/jobrole.model';
import { SlotsReport } from 'src/app/models/slotcountreport.model';

describe('JobroleReportComponent', () => {
  let component: JobroleReportComponent;
  let fixture: ComponentFixture<JobroleReportComponent>;
  let reportService: ReportService;
  let interviewerService:InterviewerService;
  let router: Router;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, HttpClientTestingModule],
      declarations: [JobroleReportComponent]
    });
    fixture = TestBed.createComponent(JobroleReportComponent);
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
    const mockReportDetails : SlotsReport=
      {
        availableSlot:5,
        bookedSlot:10
      };
    const mockApiRespone :ApiResponse<SlotsReport >={
      success:true,
      data:mockReportDetails,
      message:''
    };
    spyOn(reportService,'getSlotsCountReportBasedOnJobRole').and.returnValue(of(mockApiRespone));

    //Act
    component.selectedJobRoleId=1;
    component.fetchReport();

    //Assert
    expect(component.slotReport).toEqual(mockReportDetails);
  });

  it('should handle false success response while fetching report data  ', () => {
    //Arrange
    const mockReportDetails : SlotsReport=
      {
        availableSlot:0,
        bookedSlot:0
      };
    const mockApiRespone :ApiResponse<SlotsReport >={
      success:false,
      data:mockReportDetails,
      message:'Failed to fetch report'
    };
    spyOn(console,'log');
    spyOn(reportService,'getSlotsCountReportBasedOnJobRole').and.returnValue(of(mockApiRespone));

    //Act
    component.selectedJobRoleId=1;
    component.fetchReport();

    //Assert
    expect(console.log).toHaveBeenCalledWith('Failed to fetch Report',mockApiRespone.message);
  });

  it('should handle error response while fetching report data  ', () => {
    //Arrange
    const mockReportDetails : SlotsReport=
      {
        availableSlot:0,
        bookedSlot:0
      };
    const errorMessage:string='Failed to fetch report';
    spyOn(window,'alert');
    spyOn(reportService,'getSlotsCountReportBasedOnJobRole').and.returnValue(throwError({ error: { message: errorMessage }}));

    //Act
    component.selectedJobRoleId=1;
    component.fetchReport();

    //Assert
    expect(alert).toHaveBeenCalledWith(errorMessage);
  });

  it('should call OnJobRoleChange',()=>{
    //Arrange
    spyOn(component,'fetchReport')
    //Act
    component.OnJobRoleChange();
    //Assert
    expect(component.slotReport.availableSlot).toEqual(null);
    expect(component.slotReport.bookedSlot).toEqual(null);
    expect(component.fetchReport).toHaveBeenCalled();
  });
});
