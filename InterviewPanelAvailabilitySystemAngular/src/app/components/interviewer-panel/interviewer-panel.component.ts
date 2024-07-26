import { DatePipe } from "@angular/common";
import { ChangeDetectorRef, Component } from "@angular/core";
import { Router } from "@angular/router";
import { AddInterviewSlot } from "src/app/models/add-interview-slot.model";
import { ApiResponse } from "src/app/models/ApiResponse{T}";
import { InterviewSlots } from "src/app/models/interviewSlots.model";
import { Timeslot } from "src/app/models/timeslot.model";
import { AuthService } from "src/app/services/auth.service";
import { InterviewerPanelService } from "src/app/services/interviewer-panel.service";


@Component({
  selector: 'app-interviewer-panel',
  templateUrl: './interviewer-panel.component.html',
  styleUrls: ['./interviewer-panel.component.css']
})
export class InterviewerPanelComponent {
  selectedDate: string | null | undefined
  timeslots : Timeslot[] = [];
  employeeId: number = 0;
  existingTimeslots: InterviewSlots[] =[];
  isChecked: boolean = false;
  loading: boolean = false;

  constructor(
    private interviewerService: InterviewerPanelService,
    private cdr: ChangeDetectorRef,
    private route: Router,
    private authService: AuthService,
    private datePipe: DatePipe

  ) { }

  ngOnInit()
  {
    this.authService.getUserId().subscribe((userId: string | null | undefined) => {
      this.employeeId = Number(userId);
      console.log(this.employeeId);
    });
  }
  selectDate(): void {
    this.cdr.detectChanges();
     console.log(this.selectedDate);
     if(this.selectedDate)
     {
     this.loadTimeslots();
     }

 
  }

  onDateChange(newDate: string) {
    if (!newDate) {
      this.selectedDate = null;
      this.timeslots = [];
    }
  }
  loadTimeslots() {
    this.interviewerService.getAllTimeslots().subscribe({
      next: (response: ApiResponse<Timeslot[]>) => {
        if (response.success) {
          this.timeslots = response.data;
          this.loadInterviewSlots(); // Load interview slots after timeslots are loaded
        } else {
          console.error('Failed to fetch timeslots', response.message);
        }
        this.loading = false;

      },
      error: (error) => {
        console.error('Error fetching timeslots:', error);
        this.loading = false;

      }
    });
  }

  loadInterviewSlots() {
    this.interviewerService.getAllInterviewslotsByEmployeeId(this.employeeId).subscribe({
      next: (response: ApiResponse<InterviewSlots[]>) => {
        if (response.success) {
          this.existingTimeslots = response.data;
          console.log(this.existingTimeslots);
          const formattedSelectedDate = this.datePipe.transform(this.selectedDate, 'yyyy-MM-dd');
          this.existingTimeslots = response.data.filter(slot => {
            const formattedSlotDate = this.datePipe.transform(slot.slotDate, 'yyyy-MM-dd');
            return formattedSlotDate === formattedSelectedDate;
          });
          console.log(this.existingTimeslots);
          this.loading = false;

        } 
         else {
          console.error('Failed to fetch inte4rview slots', response.message);
        }
        this.loading = false;

      },
      error: (error) => {
        console.error('Error fetching interview slots:', error);
        this.loading = false;

      }
    });
  }

  isTimeslotBooked(timeslotId: number): boolean {
    return this.existingTimeslots.some(slot => slot.timeslotId === timeslotId);
  }

  addInterviewSlot(timeslotId: number) {
        const addInterviewSlot: AddInterviewSlot = {
          employeeId: this.employeeId,
          slotDate: this.selectedDate,
          timeslotId: timeslotId,
          isBooked: false
        };

        this.interviewerService.addInterviewSlot(addInterviewSlot).subscribe({
          next:(response) => {
            console.log(response) 
            this.loadTimeslots();
            this.loadInterviewSlots();
            this.loading = false;

          },
          error: (err) =>{
            alert(err.error.message)
            console.log(err.error);
            this.loading = false;

          }
        
        })
      }

      handleCheckboxClick(timeslotId: number): void {
        if (this.isTimeslotBooked(timeslotId)) {
          this.deleteInterviewSlot(timeslotId);
        } else {
          this.addInterviewSlot(timeslotId);
        }
      }

      deleteInterviewSlot(timeslotId: number)
      {
        
        this.interviewerService.deleteInterviewSlot(this.employeeId,this.selectedDate,timeslotId).subscribe(() => {
         console.log(this.existingTimeslots);
         this.loadInterviewSlots();

        });
      }
      
      maxDate(): string {
        const today = new Date();
        const maxDate = new Date(today.getTime() + 10 * 24 * 60 * 60 * 1000); // Add 10 days in milliseconds
        const dd = String(maxDate.getDate()).padStart(2, '0');
        const mm = String(maxDate.getMonth() + 1).padStart(2, '0');
        const yyyy = maxDate.getFullYear();
    
        return `${yyyy}-${mm}-${dd}`;
    }
    
      minDate(): string {
        const today = new Date();
        const dd = String(today.getDate()+1).padStart(2, '0');
        const mm = String(today.getMonth() + 1).padStart(2, '0'); 
        const yyyy = today.getFullYear();
     
        return `${yyyy}-${mm}-${dd}`;
      }
      

}


