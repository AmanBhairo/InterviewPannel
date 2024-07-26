import { Component, OnInit } from '@angular/core';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { InterviewRoundInterviewer } from 'src/app/models/interviewround.interviewer.model';
import { InterviewSlots } from 'src/app/models/interviewSlots.model';
import { JobRole } from 'src/app/models/jobrole.model';
import { InterviewerService } from 'src/app/services/interviewer.service';
import { RecruiterService } from 'src/app/services/recruiter.service';
 
@Component({
  selector: 'app-recruiter-panel-by-all',
  templateUrl: './recruiter-panel-by-all.component.html',
  styleUrls: ['./recruiter-panel-by-all.component.css']
})
export class RecruiterPanelByAllComponent implements OnInit{
  searchQuery: string = '';
  pageNumber: number = 1;
  pageSize: number = 2;
  loading: boolean = false;
  jobRole: JobRole = { jobRoleId: 0, jobRoleName: '' };
  interviewRoundInterviewer: InterviewRoundInterviewer = { interviewRoundId: 0, interviewRoundName: '' };
  jobRoles: JobRole[] = [];
  interviewRoundInterviewers: InterviewRoundInterviewer[] = [];
  recruiterDetailsPagination: InterviewSlots[] = [];
  interviewSlots: InterviewSlots[] = [];
  sort: string = "asc";
  totalItems: number = 0;
  totalPages: number = 0;
  constructor(
    private interviewerService: InterviewerService,
    private recruiterService: RecruiterService
  ) { }
 
  ngOnInit(): void {
    this.loadJobRoles();
    this.loadInterviewRound();
    this.loadInterviewerCount();
  }
 
  loadJobRoles(): void {
    this.interviewerService.getAllJobRoles().subscribe({
      next: (response: ApiResponse<JobRole[]>) => {
        if (response.success) {
          this.jobRoles = response.data;
        } else {
          console.error('Failed to fetch jobroles', response.message);
        }
      },
      error: (error) => {
        console.error('Error fetching jobroles:', error);
      }
    });
  }
 
  loadInterviewRound(): void {
    this.interviewerService.getAllInterviewRounds().subscribe({
      next: (response: ApiResponse<InterviewRoundInterviewer[]>) => {
        if (response.success) {
          this.interviewRoundInterviewers = response.data;
        } else {
          console.error('Failed to fetch interviewRound', response.message);
        }
      },
      error: (error) => {
        console.error('Error fetching interviewRound:', error);
      }
    });
  }
 
  getAllInterviwersWithPaginationByAll(searchQuery: string,jobRoleId:number,interviewRoundId:number): void {
    // this.recruiterDetailsPagination = []
    // this.jobRole.jobRoleId = 0;
    // this.interviewRoundInterviewer.interviewRoundId = 0;
 
    this.loading = true;
    this.recruiterService.getAllInterviwersWithPaginationByAll(this.pageNumber, this.pageSize, this.sort, searchQuery,jobRoleId,interviewRoundId).subscribe({
      next: (response: ApiResponse<InterviewSlots[]>) => {
        if (response.success) {
          this.recruiterDetailsPagination = response.data;
          console.log(response.data);
         
        } else {
          console.error('Failed to fetch recruiters with pagination', response.message);
          this.recruiterDetailsPagination = [];
 
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching recruiters with pagination:', error);
        this.loading = false;
      }
    });
  }
 
  onClickUpdate(slotId: number) {
    this.recruiterService.updateInterviewSlot(slotId)
      .subscribe({
        next: (response: ApiResponse<InterviewSlots[]>) => {
          if (response.success) {
            this.interviewSlots = response.data;
            // console.log(this.interviewSlots);
            this.ngOnInit();
            // if (this.jobRole.jobRoleId) {
            //   this.loadJobRoles(); // Fetch job roles again
            //   this.onSelectJobRole(this.jobRole.jobRoleId);
            // }
            // else if (this.interviewRoundInterviewer.interviewRoundId) {
            //   this.loadInterviewRound(); // Fetch interview rounds again
            //   this.onSelectInterviewRound(this.interviewRoundInterviewer.interviewRoundId);
            // }
            // else
            // {
            //   this.loadInterviewerCount();
            // }
           
          } else {
            console.error('Failed to fetch count', response.message);
            this.recruiterDetailsPagination = [];
          }
        },
        error: (error => {
          console.error('Failed to fetch count', error);
          this.recruiterDetailsPagination = []
        })
      });
 
  }
 
  onSearch(): void {
        this.recruiterDetailsPagination = []
    this.pageNumber = 1;
    this.loadInterviewerCount();
  }
 
  clearSearch(): void {
      this.recruiterDetailsPagination = []
 
    this.searchQuery = '';
    this.pageNumber = 1;
    this.loadInterviewerCount();
  }
  changePageSizePagination(pageSize: number): void {
    if (this.recruiterDetailsPagination.length > 0) {
      this.pageSize = pageSize;
      this.pageNumber = 1;
      this.totalPages = Math.ceil(this.totalItems / this.pageSize);
      // this.getAllInterviwerWithLetter(this.searchQuery);
      this.loadInterviewerCount();
 
    }
  }
 
  // sortAsc() {
  //   this.sort = 'asc'
  //   this.pageNumber = 1;
  //   this.loadInterviewerCount();
  // }
 
  // sortDesc() {
  //   this.sort = 'desc'
  //   this.pageNumber = 1;
  //   this.loadInterviewerCount();
  //   console.log(this.sort)
  // }
 
  onClickSort(): void {
    this.loading = true;
    if (this.sort == 'asc') {
      this.sort = 'desc';
    }
    else if (this.sort == 'desc') {
      this.sort = 'asc';
    }
    this.loadInterviewerCount();
  }
 
  changePagePagination(pageNumber: number): void {
    console.log(this.totalPages);
    console.log(this.totalItems);
    this.pageNumber = pageNumber;
    this.loadInterviewerCount();
    // this.getAllInterviwerWithLetter(this.searchQuery);
   
  }
 
  loadInterviewerCount(): void {
    this.recruiterService.getTotalInterviewSlotsByAll(this.searchQuery,this.jobRole.jobRoleId,this.interviewRoundInterviewer.interviewRoundId).subscribe({
      next: (response: ApiResponse<number>) => {
        if (response.success) {
          // console.log(response.data);
          this.totalItems = response.data;
          this.totalPages = Math.ceil(this.totalItems / this.pageSize);
          this.getAllInterviwersWithPaginationByAll(this.searchQuery,this.jobRole.jobRoleId,this.interviewRoundInterviewer.interviewRoundId);
        } else {
          console.error('Failed to fetch count', response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching count.', error);
        this.loading = false;
      }
    });
 
  }
 
  onSelectJobRole(jobRoleId: number): void {
        this.recruiterDetailsPagination = []
    // this.searchQuery = "";
    this.pageNumber = 1;
    this.jobRole.jobRoleId = jobRoleId;
    this.loadInterviewerCount();
  }
 
  onSelectInterviewRound(interviewRoundId: number): void {
    // this.searchQuery = "";
    // console.log(interviewRoundId)
    this.pageNumber = 1;
    this.recruiterDetailsPagination = [];
    // this.jobRole.jobRoleId = 0;
    this.interviewRoundInterviewer.interviewRoundId = interviewRoundId;
    this.loadInterviewerCount();
  }
  onShowAll()
  {
    this.jobRole.jobRoleId = 0;
    this.interviewRoundInterviewer.interviewRoundId = 0;
    this.searchQuery = '';
    this.loadInterviewerCount();
  }
 
}
 