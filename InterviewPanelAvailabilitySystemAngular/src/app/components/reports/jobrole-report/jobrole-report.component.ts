import { Component, OnInit } from '@angular/core';
import { JobRole } from 'src/app/models/jobrole.model';
import { SlotsReport } from 'src/app/models/slotcountreport.model';
import { InterviewerService } from 'src/app/services/interviewer.service';
import { ReportService } from 'src/app/services/report.service';

@Component({
  selector: 'app-jobrole-report',
  templateUrl: './jobrole-report.component.html',
  styleUrls: ['./jobrole-report.component.css']
})
export class JobroleReportComponent implements OnInit{
  selectedJobRoleId: number =0;
  loading : boolean = false;
  slotReport:SlotsReport={
    availableSlot:null,
    bookedSlot:null
  }

  jobrole: JobRole[] = [];

  constructor(private reportService: ReportService,private interviewerService:InterviewerService) { }
  ngOnInit(): void {
    this.loadJobRole();
  }

  loadJobRole():void{
    this.loading = true;
    this.interviewerService.getAllJobRoles()
    .subscribe(response=> {
      if(response.success){
        this.jobrole = response.data;
      }
      else{
        console.error('Failed to fetch job role', response.message);
      }
      this.loading = false;
    },error =>{
      console.error('Error fetching job role',error);
      this.loading = false;
    }
    )
  }

  fetchReport():void{
    this.reportService.getSlotsCountReportBasedOnJobRole(this.selectedJobRoleId).subscribe({
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

  // getAllJobRole():void{
  //   this.reportService.getSlotsCountReportBasedOnJobRole(this.selectedJobRoleId).subscribe({
  //     next : (response)=>{
  //       if (response.success) {
  //         this.slotReport = response.data;
          
  //       }else{
  //         console.log('Failed to fetch Report',response.message);
          
  //       }
  //     },
  //     error:(err)=>{
  //       alert(err.error.message);
  //     },
  //     complete:()=>{
  //       console.log('Completed');
  //     }
  //   });
  // }

  OnJobRoleChange():void{
    this.slotReport.availableSlot=null;
    this.slotReport.bookedSlot=null;
    this.fetchReport()
  }
}
