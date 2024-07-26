import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { InterviewRoundInterviewer } from 'src/app/models/interviewround.interviewer.model';
import { JobRoleInterviewer } from 'src/app/models/jobrole.interviewer.model';
import { UpdateInterviewer } from 'src/app/models/updateInterviewer.model';
import { InterviewerService } from 'src/app/services/interviewer.service';

@Component({
  selector: 'app-update-interviewer',
  templateUrl: './update-interviewer.component.html',
  styleUrls: ['./update-interviewer.component.css']
})
export class UpdateInterviewerComponent implements OnInit{
  
  interviewRounds: InterviewRoundInterviewer[] = [];
  jobRoles: JobRoleInterviewer[] = [];
  formData!: FormData;
  employeeId!: number;
  updateInterviewer: UpdateInterviewer = {
    employeeId: 0,
    firstName: '',
    lastName: '',
    email: '',
    jobRoleId: 0,
    interviewRoundId: 0,
  };
  loading: boolean = false;
  contact: any;
  initialStateId: any;

  constructor(
    private interviewerService: InterviewerService,
    private route: ActivatedRoute,
    private router: Router
  ) {}
  
  ngOnInit(): void {
    this.route.params.subscribe((params) =>{
      this.loadJobRoles();
      this.loadInterviewRounds();
      this.employeeId = params['id'];
      this.loadEmployeeDetails(this.employeeId);
    });
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

  loadEmployeeDetails(emploueeId:number): void{
    this.interviewerService.getEmployeeById(emploueeId).subscribe({
      next:(response) =>{
        if(response.success){
          this.updateInterviewer.employeeId = response.data.employeeId;
          this.updateInterviewer.firstName = response.data.firstName;
          this.updateInterviewer.lastName = response.data.lastName;
          this.updateInterviewer.email = response.data.email;
          this.updateInterviewer.jobRoleId = response.data.jobRoleId;
          this.updateInterviewer.interviewRoundId = response.data.interviewRoundId;
          this.initialStateId = response.data.jobRoleId;
          
        }
        else{
          console.error('Failed to fetch employees: ',response.message);
        }
      },
      error:(err) =>{
        alert(err.error.message);
      },
      complete:()=>{
        console.log("completed");
      }
    })
  }
;

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
  
  onSubmit(updateInterviewerForm: NgForm) {
    if (updateInterviewerForm.valid) {
      this.loading = true;
      console.log('form submitted',updateInterviewerForm.value)
      console.log(this.employeeId)
      this.interviewerService.updateInterviewer(this.updateInterviewer).subscribe({
        next: (response) => {
          if (response.success) {
            this.router.navigate(['/interviewers-list']);
            // updateInterviewerForm.resetForm();
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
