import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HomeComponent } from './components/home/home.component';
import { PrivacyComponent } from './components/privacy/privacy.component';
import { SigninComponent } from './components/auth/signin/signin.component';
import { AuthService } from './services/auth.service';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { ChangepasswordComponent } from './components/auth/changepassword/changepassword.component';
import { InterviewerListComponent } from './components/interviewer/interviewer-list/interviewer-list.component';
import { AddInterviewerComponent } from './components/interviewer/add-interviewer/add-interviewer.component';
import { JobroleReportComponent } from './components/reports/jobrole-report/jobrole-report.component';
import { InterviewroundReportComponent } from './components/reports/interviewround-report/interviewround-report.component';
import { DaterangeReportComponent } from './components/reports/daterange-report/daterange-report.component';
import { ReportdeciderComponent } from './components/reports/reportdecider/reportdecider.component';
import { DatePipe } from '@angular/common';
import { DaterangealldetailReportComponent } from './components/reports/daterangealldetail-report/daterangealldetail-report.component';
import { InterviewroundbasedallreportComponent } from './components/reports/interviewroundbasedallreport/interviewroundbasedallreport.component';
import { JobrolebasedalldetailreportComponent } from './components/reports/jobrolebasedalldetailreport/jobrolebasedalldetailreport.component';
import { UpdateInterviewerComponent } from './components/interviewer/update-interviewer/update-interviewer.component';
import { RecruiterPanelByAllComponent } from './components/recruiter-panel-by-all/recruiter-panel-by-all.component';
import { InterviewerPanelComponent } from './components/interviewer-panel/interviewer-panel.component';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    PrivacyComponent,
    SigninComponent,
    ChangepasswordComponent,
    InterviewerListComponent,
    AddInterviewerComponent,
    JobroleReportComponent,
    InterviewroundReportComponent,
    DaterangeReportComponent,
    ReportdeciderComponent,
    InterviewerPanelComponent,
    UpdateInterviewerComponent,
    DaterangealldetailReportComponent,
    InterviewroundbasedallreportComponent,
    JobrolebasedalldetailreportComponent,
    RecruiterPanelByAllComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule
  ],
  providers: [
    AuthService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    DatePipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
