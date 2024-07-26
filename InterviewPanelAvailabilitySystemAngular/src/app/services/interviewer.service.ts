import { AddInterviewer } from './../models/add-interviewer.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Interviewer } from '../models/interviewer.model';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Observable, catchError, of } from 'rxjs';
import { JobRoleInterviewer } from '../models/jobrole.interviewer.model';
import { InterviewRoundInterviewer } from '../models/interviewround.interviewer.model';
import { UpdateInterviewer } from '../models/updateInterviewer.model';

@Injectable({
  providedIn: 'root'
})
export class InterviewerService {
  private apiUrl = "http://localhost:5263/api/Admin/";
  constructor(private http : HttpClient) { }
  getAllInterviewers(page: number, pageSize: number, sortOrder: string, search?: string): Observable<ApiResponse<Interviewer[]>> {
    if (search == null) {
      return this.http.get<ApiResponse<Interviewer[]>>(this.apiUrl + "GetAllEmployees?page=" + page + "&pageSize=" + pageSize + "&sortOrder=" + sortOrder).pipe(
        catchError((error: any) => {
          console.error('An error occurred:', error);
          return of({ success: false, message: 'Failed to fetch interviewers list.' } as ApiResponse<Interviewer[]>);
        })
      );
    }
    else {
      return this.http.get<ApiResponse<Interviewer[]>>(this.apiUrl + "GetAllEmployees?search=" + search + "&page=" + page + "&pageSize=" + pageSize + "&sortOrder=" + sortOrder).pipe(
        catchError((error: any) => {
          console.error('An error occurred:', error);
          return of({ success: false, message: 'Failed to fetch interviewers list.' } as ApiResponse<Interviewer[]>);
        })
      );

    }
  }
  getTotalInterviewersCount(search?: string): Observable<ApiResponse<number>> {
    if (search == null) {
      return this.http.get<ApiResponse<number>>(this.apiUrl + "GetTotalEmployeeCount").pipe(
        catchError((error: any) => {
          console.error('An error occurred:', error);
          return of({ success: false, message: 'Failed to fetch total interviewers count.' } as ApiResponse<number>);
        })
      );
    }
    else {
      return this.http.get<ApiResponse<number>>(this.apiUrl + "GetTotalEmployeeCount?search=" + search).pipe(
        catchError((error: any) => {
          console.error('An error occurred:', error);
          return of({ success: false, message: 'Failed to fetch total interviewers count.' } as ApiResponse<number>);
        })
      );

    }
  }
  addInterviewer(addInterviewer : AddInterviewer): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(this.apiUrl+'AddEmployee', addInterviewer).pipe(
      catchError((error: any) => {
        console.error('An error occurred:', error);
        return of({ success: false, message: 'Failed to fetch add interviewer.' } as ApiResponse<string>);
      })
    );
  }
  getAllJobRoles():Observable<ApiResponse<JobRoleInterviewer[]>>{
    return this.http.get<ApiResponse<JobRoleInterviewer[]>>(this.apiUrl+'GetAllJobRoles').pipe(
      catchError((error: any) => {
        console.error('An error occurred:', error);
        return of({ success: false, message: 'Failed to fetch job roles.' } as ApiResponse<JobRoleInterviewer[]>);
      })
    );
  }
  getAllInterviewRounds():Observable<ApiResponse<InterviewRoundInterviewer[]>>{
    return this.http.get<ApiResponse<InterviewRoundInterviewer[]>>(this.apiUrl+'GetAllInterviewRounds').pipe(
      catchError((error: any) => {
        console.error('An error occurred:', error);
        return of({ success: false, message: 'Failed to fetch interview rounds.' } as ApiResponse<InterviewRoundInterviewer[]>);
      })
    );
  }

  updateInterviewer(updateInterviewer : UpdateInterviewer): Observable<ApiResponse<string>> {
    return this.http.put<ApiResponse<string>>(this.apiUrl+'EditEmployee/', updateInterviewer).pipe(
      catchError((error: any) => {
        console.error('An error occurred:', error);
        return of({ success: false, message: 'Failed to update employee.' } as ApiResponse<string>);
      })
    );
  }
  
  getEmployeeById(employeeId: number):Observable<ApiResponse<Interviewer>>{
    return this.http.get<ApiResponse<Interviewer>>(this.apiUrl+"GetEmployeeById/"+employeeId).pipe(
      catchError((error: any) => {
        console.error('An error occurred:', error);
        return of({ success: false, message: 'Failed to fetch employee.' } as ApiResponse<Interviewer>);
      })
    );
  }

  deleteEmployeeById(employeeId: number | undefined): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(this.apiUrl+'RemoveEmployee?id='+employeeId).pipe(
      catchError((error: any) => {
        console.error('An error occurred:', error);
        return of({ success: false, message: 'Failed to delete employee.' } as ApiResponse<string>);
      })
    );
  }
  GetIsChangedPasswordById(employeeId: number): Observable<ApiResponse<boolean>> {
      return this.http.get<ApiResponse<boolean>>(this.apiUrl + "GetIsChangedPasswordById/" + employeeId).pipe(
        catchError((error: any) => {
          console.error('An error occurred:', error);
          return of({ success: false, message: 'Failed to check is passsword changed.' } as ApiResponse<boolean>);
        })
      );
  }

}
