import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}';
import { SlotsReport } from '../models/slotcountreport.model';
import { DetailedReport } from '../models/alldetailreport.model';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private apiUrl ='http://localhost:5263/api/Report/';
  constructor(private http:HttpClient) { }

  getSlotsCountReportBasedOnJobRole(jobroleId:number):Observable<ApiResponse<SlotsReport>>{
    return this.http.get<ApiResponse<SlotsReport>>(this.apiUrl+"SlotsReport?jobRoleId="+jobroleId);
  }

  getSlotsCountReportBasedOnInterviewRound(interViewRoundId:number):Observable<ApiResponse<SlotsReport>>{
    return this.http.get<ApiResponse<SlotsReport>>(this.apiUrl+"SlotsReport?interViewRoundId="+interViewRoundId);
  }

  getSlotsCountReportBasedOnDateRange(startDate:string,endDate:string):Observable<ApiResponse<SlotsReport>>{
    return this.http.get<ApiResponse<SlotsReport>>(this.apiUrl+"SlotsReport?startDate="+startDate+"&endDate="+endDate);
  }

  getDetailedReportBasedOnJobRole(jobroleId:number,booked:boolean,page:number,pageSize:number):Observable<ApiResponse<DetailedReport[]>>{
    return this.http.get<ApiResponse<DetailedReport[]>>(this.apiUrl+"ReportDetails?jobRoleId="+jobroleId+"&booked="+booked+"&page="+page+"&pageSize="+pageSize);
  }

  getDetailedReportCountBasedOnJobRole(jobroleId:number,booked:boolean):Observable<ApiResponse<number>>{
    return this.http.get<ApiResponse<number>>(this.apiUrl+"TotalReportDetailCount?jobRoleId="+jobroleId+"&booked="+booked);
  }

  getDetailedReportBasedOnInterviewRound(interViewRoundId:number,booked:boolean,page:number,pageSize:number):Observable<ApiResponse<DetailedReport[]>>{
    return this.http.get<ApiResponse<DetailedReport[]>>(this.apiUrl+"ReportDetails?interViewRoundId="+interViewRoundId+"&booked="+booked+"&page="+page+"&pageSize="+pageSize);
  }

  getDetailedReportCountBasedOnInterviewRound(interViewRoundId:number,booked:boolean):Observable<ApiResponse<number>>{
    return this.http.get<ApiResponse<number>>(this.apiUrl+"TotalReportDetailCount?interViewRoundId="+interViewRoundId+"&booked="+booked);
  }

  getDetailedReportBasedOnDateRange(startDate:string,endDate:string,booked:boolean,page:number,pageSize:number):Observable<ApiResponse<DetailedReport[]>>{
    return this.http.get<ApiResponse<DetailedReport[]>>(this.apiUrl+"ReportDetails?startDate="+startDate+"&endDate="+endDate+"&booked="+booked+"&page="+page+"&pageSize="+pageSize);
  }

  getDetailedReportCountBasedOnDateRange(startDate:string,endDate:string,booked:boolean):Observable<ApiResponse<number>>{
    return this.http.get<ApiResponse<number>>(this.apiUrl+"TotalReportDetailCount?startDate="+startDate+"&endDate="+endDate+"&booked="+booked);
  }
}
