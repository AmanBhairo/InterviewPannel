import { Component, OnInit } from '@angular/core';
import { InterviewRoundInterviewer } from 'src/app/models/interviewround.interviewer.model';
import { InterviewRound } from 'src/app/models/interviewround.model';
import { SlotsReport } from 'src/app/models/slotcountreport.model';
import { InterviewerService } from 'src/app/services/interviewer.service';
import { ReportService } from 'src/app/services/report.service';

@Component({
  selector: 'app-interviewround-report',
  templateUrl: './interviewround-report.component.html',
  styleUrls: ['./interviewround-report.component.css']
})
export class InterviewroundReportComponent implements OnInit{
  selectedInterviewRoundId: number =0;
  loading : boolean = false;

  slotReport:SlotsReport={
    availableSlot:null,
    bookedSlot:null
  }

  interviewRounds: InterviewRoundInterviewer[] = [];

  constructor(private reportService: ReportService,private interviewerService:InterviewerService) { }
  ngOnInit(): void {
    this.loadInterviewRounds();
  }

  loadInterviewRounds():void{
    this.loading = true;
    this.interviewerService.getAllInterviewRounds()
    .subscribe(response=> {
      if(response.success){
        this.interviewRounds = response.data;
      }
      else{
        console.error('Failed to detch interview rounds', response.message);
      }
      this.loading = false;
    },error =>{
      console.error('Error fetching interview rounds',error);
      this.loading = false;
    }
    )
  }

  fetchReport():void{
    this.reportService.getSlotsCountReportBasedOnInterviewRound(this.selectedInterviewRoundId).subscribe({
      next : (response)=>{
        if (response.success) {
          this.slotReport = response.data;
          
        }else{
          console.log('Failed to fetch Report',response.message);
          
        }
      },
      error:(err)=>{
        alert(err.error.message);
      },
      complete:()=>{
        console.log('Completed');
      }
    });
  }

  OnInterviewRoundChange():void{
    this.slotReport.availableSlot=null;
    this.slotReport.bookedSlot=null;
    this.fetchReport()
  }

  
}
