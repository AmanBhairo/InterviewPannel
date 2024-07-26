import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DaterangeReportComponent } from './daterange-report.component';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule, NgModel } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NgModule } from '@angular/core';
import { NgbDateStruct, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { InterviewerService } from 'src/app/services/interviewer.service';
import { ReportService } from 'src/app/services/report.service';
import { of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { SlotsReport } from 'src/app/models/slotcountreport.model';

describe('DaterangeReportComponent', () => {
  let component: DaterangeReportComponent;
  let fixture: ComponentFixture<DaterangeReportComponent>;
  let reportService: ReportService;
  let interviewerService:InterviewerService;
  let router: Router;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, HttpClientTestingModule,NgbModule],
      declarations: [DaterangeReportComponent]
    });
    fixture = TestBed.createComponent(DaterangeReportComponent);
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
    spyOn(reportService,'getSlotsCountReportBasedOnDateRange').and.returnValue(of(mockApiRespone));

    //Act
    component.startDate={ 
      "year": 2024, 
      "month": 7, 
      "day": 15 
     };
     component.endDate={ 
      "year": 2024, 
      "month": 7, 
      "day": 17 
     };
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
    spyOn(reportService,'getSlotsCountReportBasedOnDateRange').and.returnValue(of(mockApiRespone));

    //Act
    component.startDate={ 
      "year": 2024, 
      "month": 7, 
      "day": 15 
     };
     component.endDate={ 
      "year": 2024, 
      "month": 7, 
      "day": 17 
     };
    component.fetchReport();

    //Assert
    expect(console.log).toHaveBeenCalledWith('Failed to fetch Report',mockApiRespone.message);
    expect(console.log).toHaveBeenCalledWith('Completed');
  });

  it('should handle error response while fetching report data', () => {
    //Arrange
    const errorMessage:string='Failed to fetch report';
    spyOn(console,'log');
    spyOn(reportService,'getSlotsCountReportBasedOnDateRange').and.returnValue(throwError({ error: { message: errorMessage }}));
    spyOn(window,'alert');

    //Act
    component.startDate={ 
      "year": 2024, 
      "month": 7, 
      "day": 15 
     };
     component.endDate={ 
      "year": 2024, 
      "month": 7, 
      "day": 17 
     };
    component.fetchReport();

    //Assert
    expect(alert).toHaveBeenCalledWith(errorMessage);
  });

  it('should call onStartDateChange',()=>{
    //Arrange
    spyOn(component,'fetchReport');
    //Act
    const date:NgbDateStruct={
      "year": 2024, 
       "month": 7, 
       "day": 15  
     }
    component.onStartDateChange(date);

    //Assert
    expect(component.startDate).toEqual(date);
    expect(component.endDate).toEqual(null);
    expect(component.fetchReport).toHaveBeenCalled();
  });

  it('should call onEndDateChange',()=>{
    //Arrange
    spyOn(component,'fetchReport');
    //Act
    const date:NgbDateStruct={
      "year": 2024, 
       "month": 7, 
       "day": 15  
     }
    component.onEndDateChange(date);

    //Assert
    expect(component.endDate).toEqual(date);
    expect(component.fetchReport).toHaveBeenCalled();
  });

  it('should call OnDateChange',()=>{
    //Arrange
    spyOn(component,'fetchReport');
    //Act
    component.startDate={
      "year": 2024, 
       "month": 7, 
       "day": 15  
    }

    component.endDate={
      "year": 2024, 
       "month": 7, 
       "day": 18  
    }

    component.OnDateChange();

    //Assert
    expect(component.fetchReport).toHaveBeenCalled();
  });
});
