import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Observable } from 'rxjs';
import { Timeslot } from '../models/timeslot.model';
import { InterviewSlots } from '../models/interviewSlots.model';
import { AddInterviewSlot } from '../models/add-interview-slot.model';

@Injectable({
  providedIn: 'root'
})
export class InterviewerPanelService {
  private apiUrl = "http://localhost:5263/api/Interviewer/";

  constructor(private http : HttpClient) { }

  getAllTimeslots():Observable<ApiResponse<Timeslot[]>>{
    return this.http.get<ApiResponse<Timeslot[]>>(this.apiUrl+'GetAllTimeslots');
  }
  getAllInterviewslotsByEmployeeId(employeeId: number | undefined):Observable<ApiResponse<InterviewSlots[]>>{
    return this.http.get<ApiResponse<InterviewSlots[]>>(this.apiUrl+'GetAllInterviewslots?employeeId='+employeeId);
  }

  addInterviewSlot(addInterviewSlot: AddInterviewSlot): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(`${this.apiUrl}AddInterviewSlot`, addInterviewSlot);
  }

  deleteInterviewSlot(id: number, slotDate: string | null| undefined,timeslotId: number): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}DeleteInterviewSlot${id}/?slotDate=${slotDate}&timeSlotId=${timeslotId}`)

  }
}
