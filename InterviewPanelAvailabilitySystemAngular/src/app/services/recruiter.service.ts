import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}';
import { InterviewSlots } from '../models/interviewSlots.model';
import { RecruiterDetails } from '../models/recruiterDetails.model';
import { InterviewRoundInterviewer } from '../models/interviewround.interviewer.model';

@Injectable({
  providedIn: 'root'
})
export class RecruiterService {
  private apiUrl = 'http://localhost:5263/api/Recruiter';
  this: any;
  constructor(private http: HttpClient) { }
  getInterviewersByJobRole(jobRoleId: number, pageNumber: number, pageSize: number): Observable<ApiResponse<RecruiterDetails[]>> {
    return this.http.get<ApiResponse<RecruiterDetails[]>>(this.apiUrl + '/GetInterviewersByJobRole/' + jobRoleId + "?page=" + pageNumber + "&pageSize=" + pageSize);
  }
  updateInterviewSlot(slotId: number): Observable<ApiResponse<InterviewSlots[]>> {
    return this.http.put<ApiResponse<InterviewSlots[]>>(this.apiUrl + '/UpdateInterviewSlot', { slotId });
  }
  getInterviewersByInterviewRound(interviewRoundId: number, pageNumber: number, pageSize: number): Observable<ApiResponse<RecruiterDetails[]>> {
    return this.http.get<ApiResponse<RecruiterDetails[]>>(this.apiUrl + '/GetInterviewersByInterviewRound/' + interviewRoundId + "?page=" + pageNumber + "&pageSize=" + pageSize);
  }
  getAllInterviwersWithPagination(pageNumber: number, pageSize: number, sort: string, searchQuery?: string): Observable<ApiResponse<InterviewSlots[]>> {
    if (searchQuery == null || searchQuery == "") {
      return this.http.get<ApiResponse<InterviewSlots[]>>(this.apiUrl + '/GetPaginatedInterviwer?page=' + pageNumber + '&pageSize=' + pageSize + '&sortOrder=' + sort);
    }
    else {
      return this.http.get<ApiResponse<InterviewSlots[]>>(this.apiUrl + '/GetPaginatedInterviwer?page=' + pageNumber + '&pageSize=' + pageSize + '&searchQuery=' + searchQuery + '&sortOrder=' + sort);

    }
  }
  getAllInterviewerCount(searchQuery?: string): Observable<ApiResponse<number>> {
    if (searchQuery == null) {
      return this.http.get<ApiResponse<number>>(this.apiUrl + '/GetTotalInterviewSlots');
    }
    else {
      return this.http.get<ApiResponse<number>>(this.apiUrl + '/GetTotalInterviewSlots?searchQuery=' + searchQuery);
    }
  }
  getTotalInterviewersByJobRole(jobRoleId: number): Observable<ApiResponse<number>> {
    return this.http.get<ApiResponse<number>>(this.apiUrl + '/GetTotalInterviewersByJobRole?jobRoleId=' + jobRoleId);

  }
  getTotalInterviewersByInterviewerRound(interviewRoundId: number): Observable<ApiResponse<number>> {
    return this.http.get<ApiResponse<number>>(this.apiUrl + '/GetTotalInterviewersByInterviewRound?interviewRoundId=' + interviewRoundId);

  }

  getAllInterviwersWithPaginationByAll(pageNumber: number, pageSize: number = 20, sort: string, searchQuery?: string, jobRoleId?: number, roundId?: number): Observable<ApiResponse<InterviewSlots[]>> {
    if (jobRoleId == 0 && roundId == 0 && (searchQuery == null || searchQuery == "")) {
      return this.http.get<ApiResponse<InterviewSlots[]>>(this.apiUrl + '/GetPaginatedInterviwerByAll?page=' + pageNumber + '&pageSize=' + pageSize + '&sortOrder=' + sort);
    }
    else if (jobRoleId == 0 && roundId == 0 && (searchQuery != null && searchQuery != "")) {
      return this.http.get<ApiResponse<InterviewSlots[]>>(this.apiUrl + '/GetPaginatedInterviwerByAll?page=' + pageNumber + '&pageSize=' + pageSize +'&searchQuery='+ searchQuery+ '&sortOrder=' + sort);
    }
    else if (jobRoleId != 0 && roundId != 0 && (searchQuery == null || searchQuery == "")) {
      return this.http.get<ApiResponse<InterviewSlots[]>>(this.apiUrl + '/GetPaginatedInterviwerByAll?page=' + pageNumber + '&pageSize=' + pageSize + '&sortOrder=' + sort + "&jobRoleId=" +jobRoleId+"&roundId=" +roundId);
    }
    else if (jobRoleId != 0 && roundId != 0 && (searchQuery != null && searchQuery != "")) { 
      return this.http.get<ApiResponse<InterviewSlots[]>>(this.apiUrl + '/GetPaginatedInterviwerByAll?page=' + pageNumber + '&pageSize=' + pageSize + '&searchQuery=' + searchQuery + '&sortOrder=' + sort + "&jobRoleId=" +jobRoleId+"&roundId=" +roundId);
    }
    else if (jobRoleId == 0 && roundId != 0 && (searchQuery == null || searchQuery == "")) {
      return this.http.get<ApiResponse<InterviewSlots[]>>(this.apiUrl + '/GetPaginatedInterviwerByAll?page=' + pageNumber + '&pageSize=' + pageSize +  '&sortOrder=' + sort + "&roundId=" +roundId);
    }
    else if (jobRoleId == 0 && roundId != 0 && (searchQuery != null && searchQuery != "")) {
      return this.http.get<ApiResponse<InterviewSlots[]>>(this.apiUrl + '/GetPaginatedInterviwerByAll?page=' + pageNumber + '&pageSize=' + pageSize + '&searchQuery=' + searchQuery + '&sortOrder=' + sort +"&roundId=" +roundId);
    }
    else if (jobRoleId != 0 && roundId == 0 && (searchQuery == null || searchQuery == "")) {
      return this.http.get<ApiResponse<InterviewSlots[]>>(this.apiUrl + '/GetPaginatedInterviwerByAll?page=' + pageNumber + '&pageSize=' + pageSize + '&sortOrder=' + sort + "&jobRoleId=" +jobRoleId);

    }
    else {
      return this.http.get<ApiResponse<InterviewSlots[]>>(this.apiUrl + '/GetPaginatedInterviwerByAll?page=' + pageNumber + '&pageSize=' + pageSize + '&searchQuery=' + searchQuery + '&sortOrder=' + sort + "&jobRoleId=" +jobRoleId);

    }

  }
  getTotalInterviewSlotsByAll(searchQuery?: string, jobRoleId?: number|null, roundId?: number|null): Observable<ApiResponse<number>> {
    if (jobRoleId == 0 && roundId == 0 && (searchQuery == null || searchQuery == "")) {
      return this.http.get<ApiResponse<number>>(this.apiUrl + '/GetTotalInterviewSlotsByAll');
    }
    else if (jobRoleId == 0 && roundId == 0 && (searchQuery != null && searchQuery != "")) {
      return this.http.get<ApiResponse<number>>(this.apiUrl + '/GetTotalInterviewSlotsByAll?searchQuery='+ searchQuery);
    }
    else if (jobRoleId != 0 && roundId != 0 && (searchQuery == null || searchQuery == "")) {
      return this.http.get<ApiResponse<number>>(this.apiUrl + '/GetTotalInterviewSlotsByAll?jobRoleId=' +jobRoleId+"&roundId=" +roundId);
    }
    else if (jobRoleId != 0 && roundId != 0 && (searchQuery != null && searchQuery != "")) { 
      return this.http.get<ApiResponse<number>>(this.apiUrl + '/GetTotalInterviewSlotsByAll?searchQuery=' + searchQuery + "&jobRoleId=" +jobRoleId+"&roundId=" +roundId);
    }
    else if (jobRoleId == 0 && roundId != 0 && (searchQuery == null || searchQuery == "")) {
      return this.http.get<ApiResponse<number>>(this.apiUrl + '/GetTotalInterviewSlotsByAll?roundId=' +roundId);
    }
    else if (jobRoleId == 0 && roundId != 0 && (searchQuery != null && searchQuery != "")) {
      return this.http.get<ApiResponse<number>>(this.apiUrl + '/GetTotalInterviewSlotsByAll?searchQuery=' + searchQuery +"&roundId=" +roundId);
    }
    else if (jobRoleId != 0 && roundId == 0 && (searchQuery == null || searchQuery == "")) {
      return this.http.get<ApiResponse<number>>(this.apiUrl + '/GetTotalInterviewSlotsByAll?jobRoleId=' +jobRoleId);

    }
    else {
      return this.http.get<ApiResponse<number>>(this.apiUrl + '/GetTotalInterviewSlotsByAll?searchQuery=' + searchQuery + "&jobRoleId=" +jobRoleId);

    }

  }

}
