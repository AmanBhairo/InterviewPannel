import { InterviewerService } from 'src/app/services/interviewer.service';
import { ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { LocalstorageService } from 'src/app/services/helpers/localstorage.service';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})
export class SigninComponent {
  username: string = '';
  password: string = '';
  loading : boolean = false;
  employeeIntId: number = 0;
  loginFirst: boolean = false;
  
  constructor(
    private authService: AuthService,
    private interviewerService: InterviewerService,
    private localStorageHelper: LocalstorageService,
    private router:Router,
    private cdr:ChangeDetectorRef ) {}
    
  login() {
    this.loading = true;
    this.authService.signIn(this.username, this.password).subscribe({
      next:(response) => {
        if(response.success) {
          this.cdr.detectChanges();
          this.getEmployeeId();
          this.IsEployeePasswordChangesIsTrue(this.employeeIntId);
        } else {
          alert(response.message);
        }
        this.loading = false;
      },
      error:(err) => {
        alert(err.error.message);
        this.loading = false;
      }
    })
  }
  getEmployeeId(){
    this.authService.getUserId().subscribe((userId: string | null | undefined) => {
      this.employeeIntId = Number(userId);
    });
  }
  IsEployeePasswordChangesIsTrue(employeeIntId:number):void{
    this.interviewerService.GetIsChangedPasswordById(employeeIntId).subscribe({
      next: (response) => {
        if(response.success){
          this.loginFirst = response.data;
          console.log(this.loginFirst);
          if(this.loginFirst == true){
            this.router.navigate(['/home']);
          }
          else{
            this.router.navigate(['/changepassword']);
          }
        }
        else{
          console.error('Failed to fetch is changed password:',response.message)
        }
      },
      error:(error) => {
        console.error('Error fetching is changed password:',error)
      }
    });
  }
}
