import { Component } from '@angular/core';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { SlotsReport } from 'src/app/models/slotcountreport.model';
import { ReportService } from 'src/app/services/report.service';

@Component({
  selector: 'app-daterange-report',
  templateUrl: './daterange-report.component.html',
  styleUrls: ['./daterange-report.component.css']
})
export class DaterangeReportComponent {
  startDate: NgbDateStruct  ;
  endDate: NgbDateStruct | null = null; 
  slotReport:SlotsReport={
    availableSlot:null, 
    bookedSlot:null
  }

  minStartDate: NgbDateStruct; // Minimum start date

  constructor(private reportService: ReportService) {
    const currentDate = new Date();
    this.minStartDate = {
      year: 2000,
      month: 1,
      day: 1
    };

    this.startDate = {
      year: 2000,
      month: 1,
      day: 1
    };
   }

  fetchReport(): void {
    if (this.startDate && this.endDate) {
      const formattedStartDate = this.formatDate(this.startDate);
      const formattedEndDate = this.formatDate(this.endDate);
      
      this.reportService.getSlotsCountReportBasedOnDateRange(formattedStartDate, formattedEndDate).subscribe({
        next: (response) => {
          if (response.success) {
            this.slotReport = response.data;
          } else {
            console.log('Failed to fetch Report', response.message);
          }
        },
        error: (err) => {
          alert(err.error.message);
        },
        complete: () => {
          console.log('Completed');
        }
      });
    }
  }

  onStartDateChange(date: NgbDateStruct): void {
    if (date) {
      this.startDate = date;
      // this.endDate = null; // Reset end date when start date changes
      this.slotReport ={
        availableSlot:null, 
        bookedSlot:null
      }
    }
    this.fetchReport();
  }

  onEndDateChange(date: NgbDateStruct): void {
    if (date) {
      this.endDate = date;
    }
    this.fetchReport();
  }

  formatDate(date: NgbDateStruct): string {
    const mm = date.month.toString().padStart(2, '0');
      const dd = date.day.toString().padStart(2, '0');
      const yyyy = date.year;

      return `${mm}-${dd}-${yyyy}`;
  }

  OnDateChange():void{
    if(this.startDate && this.endDate){
    this.fetchReport()
    }
  }

}
