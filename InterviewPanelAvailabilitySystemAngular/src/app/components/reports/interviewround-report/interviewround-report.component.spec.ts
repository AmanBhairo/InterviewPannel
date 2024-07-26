import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InterviewroundReportComponent } from './interviewround-report.component';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { InterviewerService } from 'src/app/services/interviewer.service';
import { ReportService } from 'src/app/services/report.service';
import { of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { InterviewRoundInterviewer } from 'src/app/models/interviewround.interviewer.model';
import { SlotsReport } from 'src/app/models/slotcountreport.model';

describe('InterviewroundReportComponent', () => {
  let component: InterviewroundReportComponent;
  let fixture: ComponentFixture<InterviewroundReportComponent>;
  let reportService: ReportService;
  let interviewerService:InterviewerService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, HttpClientTestingModule],
      declarations: [InterviewroundReportComponent]
    });
    fixture = TestBed.createComponent(InterviewroundReportComponent);
    component = fixture.componentInstance;
    reportService = TestBed.inject(ReportService);
    interviewerService = TestBed.inject(InterviewerService);
    
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch interview round when component initialize', () => {
    // Test ngOnInit
    //Arrange
    const mockJobRoles : InterviewRoundInterviewer[]=[
      {
        interviewRoundId:1,
        interviewRoundName:'Technical'
      },
      {
        interviewRoundId:2,
        interviewRoundName:'Manager'
      }
    ]
    const mockApiRespone :ApiResponse<InterviewRoundInterviewer[]>={
      success:true,
      data:mockJobRoles,
      message:''
    };
    spyOn(interviewerService,'getAllInterviewRounds').and.returnValue(of(mockApiRespone));

    //Act
    component.ngOnInit();

    //Assert
    expect(component.interviewRounds).toEqual(mockJobRoles);
    expect(component.loading).toEqual(false);
  });

  it('should handle when success resposne is false while fetching interview round when component initialize', () => {
    // Test ngOnInit
    //Arrange
    const mockApiRespone :ApiResponse<InterviewRoundInterviewer[]>={  
      success:false,
      data:[],
      message:'Something went wrong, please try after sometime'
    };
    spyOn(console,'error');
    spyOn(interviewerService,'getAllInterviewRounds').and.returnValue(of(mockApiRespone));

    //Act
    component.ngOnInit();

    //Assert
    expect(console.error).toHaveBeenCalledWith('Failed to detch interview rounds',mockApiRespone.message);
    expect(component.loading).toEqual(false);
  });

  it('should handle when error response while fetching interview round when component initialize', () => {
    // Test ngOnInit
    //Arrange
    const errorMessage:String ='Error fetching interview round';
    spyOn(console,'error');
    spyOn(interviewerService,'getAllInterviewRounds').and.returnValue(throwError(errorMessage));

    //Act
    component.ngOnInit();

    //Assert
    expect(console.error).toHaveBeenCalledWith('Error fetching interview rounds',errorMessage);
    expect(component.loading).toEqual(false);
  });

  it('should fetch report data  ', () => {
    //Arrange
    const mockReportDetails : SlotsReport=
      {
        availableSlot:5,
        bookedSlot:10
      };
    const mockApiRespone :ApiResponse<SlotsReport>={
      success:true,
      data:mockReportDetails,
      message:''
    };
    spyOn(reportService,'getSlotsCountReportBasedOnInterviewRound').and.returnValue(of(mockApiRespone));

    //Act
    component.selectedInterviewRoundId=1;
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
    const mockApiRespone :ApiResponse<SlotsReport>={
      success:false,
      data:mockReportDetails,
      message:'Failed to fetch report'
    };
    spyOn(console,'log');
    spyOn(reportService,'getSlotsCountReportBasedOnInterviewRound').and.returnValue(of(mockApiRespone));

    //Act
    component.selectedInterviewRoundId=1;
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
    spyOn(reportService,'getSlotsCountReportBasedOnInterviewRound').and.returnValue(throwError({ error: { message: errorMessage }}));

    //Act
    component.selectedInterviewRoundId=1;
    component.fetchReport();

    //Assert
    expect(alert).toHaveBeenCalledWith(errorMessage);
  });

  it('should call OnJobRoleChange',()=>{
    //Arrange
    spyOn(component,'fetchReport')
    //Act
    component.OnInterviewRoundChange();
    //Assert
    expect(component.slotReport.availableSlot).toEqual(null);
    expect(component.slotReport.bookedSlot).toEqual(null);
    expect(component.fetchReport).toHaveBeenCalled();
  });
});
