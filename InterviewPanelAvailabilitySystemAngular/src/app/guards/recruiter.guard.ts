import { Interviewer } from './../models/interviewer.model';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { map } from 'rxjs';
import { inject } from '@angular/core';
import { InterviewerService } from '../services/interviewer.service';
import { ApiResponse } from '../models/ApiResponse{T}';

let employeeIntId: number = 0;
export const recruiterGuard: CanActivateFn = (route, state) => {
  const interviewerService = inject(InterviewerService);
  const authService = inject(AuthService);
  const router = inject(Router);
  
  authService.getUserId().subscribe((userId: string | null | undefined) => {
    employeeIntId = Number(userId);
  });
  return interviewerService.getEmployeeById(employeeIntId).pipe(
    map((response: ApiResponse<Interviewer> | undefined) => {
      if (response && response?.data.isRecruiter == true) {
        return true;
      } else {
        router.navigate(['/home']); // Redirect to a different route if not admin
        return false;
      }
    }),
  );
};
