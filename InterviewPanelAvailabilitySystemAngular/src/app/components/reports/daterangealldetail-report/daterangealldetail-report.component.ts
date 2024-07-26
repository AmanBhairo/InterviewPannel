import { Component, OnInit } from '@angular/core';
import { DetailedReport } from 'src/app/models/alldetailreport.model';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { JobRole } from 'src/app/models/jobrole.model';
import { Timeslot } from 'src/app/models/timeslot.model';
import { InterviewerPanelService } from 'src/app/services/interviewer-panel.service';
import { InterviewerService } from 'src/app/services/interviewer.service';
import { ReportService } from 'src/app/services/report.service';

@Component({
  selector: 'app-daterangealldetail-report',
  templateUrl: './daterangealldetail-report.component.html',
  styleUrls: ['./daterangealldetail-report.component.css']
})
export class DaterangealldetailReportComponent implements OnInit {

  booked:boolean = false;
  pageNumber:number = 1;
  pageSize:number = 6;
  loading : boolean = false;
  imageSrc:string= 'assets/loader.gif';
  reportDetails : DetailedReport[]|undefined;
  totalItems: number = 0;
  totalPages: number = 0;
  startDate:string='';
  endDate:string='';
  minEndDate=''

  constructor(private reportService: ReportService,private interviewPannelService:InterviewerPanelService,private interviewerService:InterviewerService) {}

  ngOnInit(): void {
    
  }


  
  loadReportDetails():void{
    this.loading = true;
    this.reportService.getDetailedReportBasedOnDateRange(this.startDate,this.endDate,this.booked,this.pageNumber,this.pageSize)
    .subscribe(response=> {
      if(response.success){
        this.reportDetails = response.data;
      }
      else{
        console.error('No record available', response.message);
      }
      this.loading = false;
    },error =>{
      this.reportDetails = [];
      console.error('Error fetching reports',error);
      this.loading = false;
    }
    )
  }


  changePageSize(pageSize: number): void {
    this.pageSize = pageSize;
    this.pageNumber = 1; 
    this.totalPages = Math.ceil(this.totalItems / this.pageSize);
    this.loadReportDetails();
  }

  changePage(pageNumber: number): void {
    this.pageNumber = pageNumber;
    this.getReportsDetailCount()
    this.loadReportDetails();
  }

  OnJobRoleChange():void{
    this.getReportsDetailCount()
    // this.loadReportDetails()
  }

  getReportsDetailCount():void{
    this.reportService.getDetailedReportCountBasedOnDateRange(this.startDate,this.endDate,this.booked).subscribe({
      next: (response: ApiResponse<number>) => {
        if (response.success) {
          console.log(response.data);
          this.totalItems = response.data;
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);
          this.loadReportDetails();
        } else {
          console.error('Failed to fetch reports count', response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching reports count.', error);
        this.loading = false;
      }
    });

  }


  slotdecider(slot:boolean):void{
    if(this.booked != slot){
      this.pageNumber =1;
    }
    this.booked = slot;
    this.getReportsDetailCount();
  }

  OnStartDateChange():void{
    this.endDate='';
    this.reportDetails=[];
    this.totalItems=0;
    this.pageNumber=1;
    this.minEndDate = this.startDate;
  }

  OnEndDateChange():void{
    this.reportDetails=[];
    this.pageNumber=1;
    if(this.startDate!='' && this.endDate!=''){
      this.getReportsDetailCount();
    }
  }

  
}
