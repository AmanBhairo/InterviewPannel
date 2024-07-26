import { ChangeDetectorRef, Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { InterviewerService } from 'src/app/services/interviewer.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  username: string | undefined | null;
  isAuthenticated :boolean = false;
  Name: string = ''
  userId!: number ;

  constructor(private authService: AuthService,private cdr: ChangeDetectorRef, private interviewService: InterviewerService) { }

  ngOnInit() {
    this.authService.getUsername().subscribe((username: string | null | undefined) => {
      this.username = username;
    });

    this.authService.isAuthenticated().subscribe((authState: boolean) => {
      this.isAuthenticated = authState;
      this.cdr.detectChanges(); 
    });
    this.getUser();
  }

  getUser()
  {
    this.authService.getUserId().subscribe((userId: string | null | undefined) => {
      this.userId = Number(userId);
      console.log(this.userId);
    });
    this.interviewService.getEmployeeById(this.userId).subscribe({
      next: (response) => {
        if (response.success) {
          this.Name = response.data.firstName;
          
        } else {
          console.error('Failed to fetch', response.message);
        }
      },
      error: (error) => {
        console.error('Failed to fetch', error);
      },
    });
  }
}
