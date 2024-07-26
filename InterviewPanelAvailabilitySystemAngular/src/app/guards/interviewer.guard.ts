import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { map } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}';
import { Interviewer } from '../models/interviewer.model';
import { AuthService } from '../services/auth.service';
import { InterviewerService } from '../services/interviewer.service';

let employeeIntId: number = 0;
export const interviewerGuard: CanActivateFn = (route, state) => {
  const interviewerService = inject(InterviewerService);
  const authService = inject(AuthService);
  const router = inject(Router);
  
  authService.getUserId().subscribe((userId: string | null | undefined) => {
    employeeIntId = Number(userId);
  });
  return interviewerService.getEmployeeById(employeeIntId).pipe(
    map((response: ApiResponse<Interviewer> | undefined) => {
      if (response && response?.data.isRecruiter == false && response?.data.isAdmin == false ) {
        return true;
      } else {
        router.navigate(['/home']); // Redirect to a different route if not admin
        return false;
      }
    }),
  );
};
