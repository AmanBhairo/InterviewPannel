import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { AddInterviewer } from 'src/app/models/add-interviewer.model';
import { InterviewRoundInterviewer } from 'src/app/models/interviewround.interviewer.model';
import { JobRoleInterviewer } from 'src/app/models/jobrole.interviewer.model';
import { InterviewerService } from 'src/app/services/interviewer.service';

@Component({
  selector: 'app-add-interviewer',
  templateUrl: './add-interviewer.component.html',
  styleUrls: ['./add-interviewer.component.css'],
})
export class AddInterviewerComponent implements OnInit{
  interviewRounds: InterviewRoundInterviewer[] = [];
  jobRoles: JobRoleInterviewer[] = [];
  newInterviewer: AddInterviewer = {
    firstName: '',
    lastName: '',
    email: '',
    jobRoleId: 0,
    interviewRoundId: 0,
  };
  loading: boolean = false;
  constructor(
    private interviewerService: InterviewerService,
    private route: ActivatedRoute,
    private router: Router
  ) {}
  ngOnInit(): void {
    this.loadJobRoles();
    this.loadInterviewRounds();
  }
  loadJobRoles(): void {
    this.loading = true;
    this.interviewerService.getAllJobRoles().subscribe({
      next: (response: ApiResponse<JobRoleInterviewer[]>) => {
        if (response.success) {
          this.jobRoles = response.data;
        } else {
          console.error('Failed to fetch job roles', response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching job roles: ', error);
        this.loading = false;
      },
    });
  }
  loadInterviewRounds(): void {
    this.loading = true;
    this.interviewerService.getAllInterviewRounds().subscribe({
      next: (response: ApiResponse<InterviewRoundInterviewer[]>) => {
        if (response.success) {
          this.interviewRounds = response.data;
        } else {
          console.error('Failed to fetch interview rounds', response.message);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching interview rounds: ', error);
        this.loading = false;
      },
    });
  }
  onSubmit(addInterviewerForm: NgForm): void {
    if (addInterviewerForm.valid) {
      this.loading = true;
      this.interviewerService.addInterviewer(this.newInterviewer).subscribe({
        next: (response) => {
          if (response.success) {
            this.router.navigate(['/interviewers-list']);
            // addInterviewerForm.resetForm();
          } else {
            alert(response.message);
          }
          this.loading = false;
        },
        error: (err) => {
          this.loading = false;
          alert(err.error.message);
        },
        complete: () => {
          this.loading = false;
          console.log('completed');
        },
      });
    }
  }
}
