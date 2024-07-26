import { Router } from '@angular/router';
import { InterviewerService } from 'src/app/services/interviewer.service';
import { Interviewer } from './../../../models/interviewer.model';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-interviewer-list',
  templateUrl: './interviewer-list.component.html',
  styleUrls: ['./interviewer-list.component.css'],
})
export class InterviewerListComponent implements OnInit{
  interviewers: Interviewer[] | undefined;
  username: string | null | undefined;
  totalUsers!: number;
  pageSize = 2;
  currentPage = 1;
  loading: boolean = false;
  // isAuthenticated: boolean = false;
  totalPages: number[] = [];
  sortOrder: string = 'asc';
  searchQuery: string = '';
  employeeId: number | undefined;
  constructor(
    private interviewerService: InterviewerService,
    private cdr: ChangeDetectorRef,
    private route: Router
  ) { }
  ngOnInit(): void {
    this.loadInterviewers();
    this.totalInterviewerCount();
  }

  totalInterviewerCount(search?: string) {
    this.interviewerService.getTotalInterviewersCount(search).subscribe({
      next: (response: ApiResponse<number>) => {
        if (response.success) {
          this.totalUsers = response.data;
          console.log(this.totalUsers);
          this.calculateTotalPages();
        } else {
          console.error('Failed to fetch interviewers count', response.message);
        }
      },
      error: (error) => {
        console.error('Failed to fetch interviewers count', error);
        this.loading = false;
      },
    });
  }
  loadInterviewers(search?: string) {
    this.loading = true;
    this.interviewerService.getAllInterviewers(this.currentPage, this.pageSize, this.sortOrder, search).subscribe({
      next: (response: ApiResponse<Interviewer[]>) => {
        if (response.success) {
          this.interviewers = response.data;
          console.log(response.data);
        }
        else {  
          console.error('Failed to fetch interviewers', response.message);
        }
        this.loading = false;

      },
      error: (error => {
        console.error('Failed to fetch interviewers', error);
        this.loading = false;
      })
    });
  }
  calculateTotalPages() {
    this.totalPages = [];
    const pages = Math.ceil(this.totalUsers / this.pageSize);
    for (let i = 1; i <= pages; i++) {
      this.totalPages.push(i);
    }
  }
  onPageChange(page: number) {
    this.currentPage = page;
    this.loadInterviewers(this.searchQuery);
  }

  onPageSizeChange() {
    this.currentPage = 1; // Reset to first page when page size changes
    this.loadInterviewers(this.searchQuery);
    this.totalInterviewerCount(this.searchQuery);
  }

  onShowAll() {
    this.currentPage = 1;
    this.totalInterviewerCount(this.searchQuery);
    this.loadInterviewers(this.searchQuery);
  }
  onClickSort(): void {
    this.loading = true;
    if (this.sortOrder == 'asc') {
      this.sortOrder = 'desc';
    }
    else if (this.sortOrder == 'desc') {
      this.sortOrder = 'asc';
    }
    this.currentPage = 1;
    this.totalInterviewerCount(this.searchQuery);
    this.loadInterviewers(this.searchQuery);
  }
  searchUsers() {
    if (this.searchQuery && this.searchQuery.length > 2) {
      this.currentPage = 1;
      this.loadInterviewers(this.searchQuery);
      this.totalInterviewerCount(this.searchQuery);
    }
    else {
      this.currentPage = 1;
      this.loadInterviewers();
      this.totalInterviewerCount();
    }
  }
  clearSearch() {
    this.currentPage = 1;
    this.searchQuery = '';
    this.loadInterviewers(this.searchQuery);
    this.totalInterviewerCount(this.searchQuery);
  }

  //update
  employeeUpdate(id:number): void{
    this.route.navigate(['update-interviewer/', id])
  }

  //delete
  confirmDelete(id: number): void {
    if(confirm('Are you sure ?')) {
      this.employeeId = id;
      this.deleteEmployee();
    }
  }

  deleteEmployee(): void{
    this.interviewerService.deleteEmployeeById(this.employeeId).subscribe({
      next: (response) => {
        if(response.success){
          this.totalUsers--;
          this.calculateTotalPages()
          if (this.currentPage > this.totalPages.length) {
            this.currentPage = this.totalPages.length;
          }
          this.loadInterviewers();
          // this.ngOnInit();
        }
        else{
          alert(response.message);
        }
      },
      error: (err)=> {
        alert(err.error.message);
      },
      complete:  ()=>{
        console.log('Completed');
      }
    });
  }
}
