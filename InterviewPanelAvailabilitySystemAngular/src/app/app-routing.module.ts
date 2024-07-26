import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { PrivacyComponent } from './components/privacy/privacy.component';
import { SigninComponent } from './components/auth/signin/signin.component';
import { ChangepasswordComponent } from './components/auth/changepassword/changepassword.component';
import { InterviewerListComponent } from './components/interviewer/interviewer-list/interviewer-list.component';
import { AddInterviewerComponent } from './components/interviewer/add-interviewer/add-interviewer.component';
import { JobroleReportComponent } from './components/reports/jobrole-report/jobrole-report.component';
import { InterviewroundReportComponent } from './components/reports/interviewround-report/interviewround-report.component';
import { ReportdeciderComponent } from './components/reports/reportdecider/reportdecider.component';
import { UpdateInterviewerComponent } from './components/interviewer/update-interviewer/update-interviewer.component';
import { adminGuard } from './guards/admin.guard';
import { recruiterGuard } from './guards/recruiter.guard';
import { interviewerGuard } from './guards/interviewer.guard';
import { DaterangealldetailReportComponent } from './components/reports/daterangealldetail-report/daterangealldetail-report.component';
import { InterviewroundbasedallreportComponent } from './components/reports/interviewroundbasedallreport/interviewroundbasedallreport.component';
import { JobrolebasedalldetailreportComponent } from './components/reports/jobrolebasedalldetailreport/jobrolebasedalldetailreport.component';
import { RecruiterPanelByAllComponent } from './components/recruiter-panel-by-all/recruiter-panel-by-all.component';
import { InterviewerPanelComponent } from './components/interviewer-panel/interviewer-panel.component';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'privacy', component: PrivacyComponent },
  { path: 'signin', component: SigninComponent },
  { path: 'changepassword', component: ChangepasswordComponent, canActivate:[interviewerGuard] },
  { path: 'changepassword1', component: ChangepasswordComponent, canActivate:[recruiterGuard] },
  { path: 'interviewers-list', component: InterviewerListComponent, canActivate:[adminGuard] },
  { path: 'add-interviewer', component:AddInterviewerComponent, canActivate:[adminGuard]},
  { path: 'jobrolebasedreport', component: JobroleReportComponent, canActivate:[adminGuard] },
  { path: 'interviewroundbasedreport', component: InterviewroundReportComponent, canActivate:[interviewerGuard]},
    { path: 'reportDecider/:reportDeciderVal', component: ReportdeciderComponent, canActivate:[adminGuard]},
  { path: 'interviewer-panel', component: InterviewerPanelComponent, canActivate:[interviewerGuard]},
  { path: 'recruiterPanelbyall', component: RecruiterPanelByAllComponent, canActivate:[recruiterGuard]},


  { path: 'daterangebaseddetailedreport', component: DaterangealldetailReportComponent},

  { path: 'update-interviewer/:id', component: UpdateInterviewerComponent, canActivate:[adminGuard]},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
