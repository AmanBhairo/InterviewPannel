import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './services/auth.service';
import { InterviewerService } from './services/interviewer.service';
import { Interviewer } from './models/interviewer.model';
import { JobRoleInterviewer } from './models/jobrole.interviewer.model';
import { Subscription } from 'rxjs';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'InterviewPanelAvailabilitySystem';
  isAuthenticated :boolean = false;
  username: string | null | undefined;
  userId : number | undefined;
  isRecruiter: boolean = false;
  Name: string = '';
  private userSubscription: Subscription | undefined


  constructor(
    private authService: AuthService,
    private cdr: ChangeDetectorRef,
    private router: Router,
    private interviewService: InterviewerService
  ) {}
  ngOnInit(): void {
    this.authService.isAuthenticated().subscribe((authState: boolean) => {
      this.isAuthenticated = authState;
      this.cdr.detectChanges(); 
    });
    this.userSubscription = this.authService.getUsername().subscribe((username: string | null | undefined) => {
      this.username = username;
      if (this.username) {
        this.getUser();
      }
      this.cdr.detectChanges(); 
    });
    // this.authService.getUsername().subscribe((username: string | null | undefined) => {
    //   this.username = username;
    //   this.cdr.detectChanges(); 
    // });
   this.userSubscription = this.authService.getUserId().subscribe((userId: string | null | undefined) => {

      this.userId = Number(userId);
      if(this.userId)
      {
        this.getUser();
      }
    });

  }
  signOut() {
    this.authService.signOut();
    this.isRecruiter = false;
    this.router.navigate(['/home']);
  }
  ngOnDestroy(): void {
    // Unsubscribe to prevent memory leaks  
    if (this.userSubscription) {
      this.userSubscription.unsubscribe();
    }
  }
 

  getUser()
  {
    const employeeId = Number(this.userId);
    console.log(this.userId);
    this.interviewService.getEmployeeById(employeeId).subscribe({
      next: (response) => {
        if (response.success) {
          this.Name = response.data.firstName;
          if(response.data.isRecruiter)
          {
            this.isRecruiter = true
          }
          if(!response.data.isRecruiter && !response.data.isAdmin)
          {
            this.isRecruiter = false;
          }
          
        } else {
          console.error('Failed to fetch contact', response.message);
        }
      },
      error: (error) => {
        console.error('Failed to fetch contact', error);
      },
    });
  }
  
 

  reportDecider(reportDeciderVal:string){
    this.router.navigate(['/reportDecider/', reportDeciderVal]);
  }
}
