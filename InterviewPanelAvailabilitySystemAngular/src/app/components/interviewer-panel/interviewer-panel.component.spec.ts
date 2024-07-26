import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { AuthService } from 'src/app/services/auth.service';
import { InterviewerPanelService } from 'src/app/services/interviewer-panel.service';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Timeslot } from 'src/app/models/timeslot.model';
import { InterviewSlots } from 'src/app/models/interviewSlots.model';
import { InterviewerPanelComponent } from './interviewer-panel.component';

describe('InterviewerPanelComponent', () => {
  let component: InterviewerPanelComponent;
  let fixture: ComponentFixture<InterviewerPanelComponent>;
  let authService: AuthService;
  let interviewerPannelService:InterviewerPanelService;
  let router: Router;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, HttpClientTestingModule],
      declarations: [InterviewerPanelComponent],
      providers:[DatePipe]
    });
    fixture = TestBed.createComponent(InterviewerPanelComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthService);
    interviewerPannelService = TestBed.inject(InterviewerPanelService);
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set employeeId when getUserId returns a value', () => {
    //Arrange
    const mockUserId = '123';
    spyOn(authService,'getUserId').and.returnValue(of(mockUserId));
    
    //Act
    component.ngOnInit();
    
    //Assert
    expect(component.employeeId).toEqual(Number(mockUserId));
  });

  it('should should call selectDate', () => {
    //Arrange
    spyOn(component,'loadTimeslots');
    
    //Act
    component.selectedDate='07-17-2024';
    component.selectDate();
    
    //Assert
    expect(component.loadTimeslots).toHaveBeenCalled();
  });

  it('should should call onDateChange and set selectedDate and data to null', () => {
    //Arrange
    const newDate:string='';
    
    //Act
    component.onDateChange(newDate);
    
    //Assert
    expect(component.selectedDate).toEqual(null);
    expect(component.timeslots).toEqual([]);
  });

  it('should should call loadTimeslots and fetch time slots', () => {
    //Arrange
    const mockTimeSlot:Timeslot[]=[
      {
        timeslotId:2,
        timeslotName:'10:00AM-11:00AM'
      },
      {
        timeslotId:2,
        timeslotName:'11:00AM-12:00PM'
      }
    ]

    const mockApiRespone:ApiResponse<Timeslot[]>={
      data:mockTimeSlot,
      success:true,
      message:''
    }
    spyOn(interviewerPannelService,'getAllTimeslots').and.returnValue(of(mockApiRespone));  
    spyOn(component,'loadInterviewSlots');
    
    //Act
    component.loadTimeslots();
    
    //Assert
    expect(component.timeslots).toEqual(mockTimeSlot);
    expect(component.loadInterviewSlots).toHaveBeenCalled();
  });

  it('should should handle false success response while fetching loadTimeslots', () => {
    //Arrange
    
    const mockApiRespone:ApiResponse<Timeslot[]>={
      data:[],
      success:false,
      message:'Failed to fetch time slots'
    }
    spyOn(interviewerPannelService,'getAllTimeslots').and.returnValue(of(mockApiRespone)); 
    spyOn(console,'error'); 
    spyOn(component,'loadInterviewSlots');
    
    //Act
    component.loadTimeslots();
    
    //Assert
    expect(console.error).toHaveBeenCalledWith('Failed to fetch timeslots',mockApiRespone.message);
  });

  it('should should handle error response while fetching loadTimeslots', () => {
    //Arrange
    
    const errorMessage:string='Failed to fetch time slots';
    spyOn(interviewerPannelService,'getAllTimeslots').and.returnValue(throwError(errorMessage)); 
    spyOn(console,'error'); 
    spyOn(component,'loadInterviewSlots');
    
    //Act
    component.loadTimeslots();
    
    //Assert
    expect(console.error).toHaveBeenCalledWith('Error fetching timeslots:',errorMessage);
  });

  it('should should call loadInterviewSlots and fetch interview slots', () => {
    //Arrange
    const mockInterviewSlots:InterviewSlots[]=[
      {
        employeeId:1,
        isBooked:true,
        slotDate:'07-17-2024',
        slotId:1,
        timeslotId:1,
        employee:{
          firstName:'firstName',
          lastName:'lastName',
          changePassword:true,
          email:'user@gmail.com',
          employeeId:1,
          interviewRound:{
            interviewRoundId:1,
            interviewRoundName:'Technical'
          },
          interviewRoundId:1,
          isAdmin:true,
          isRecruiter:false,
          jobRoleId:1,
          jobRole:{
            jobRoleId:1,
            jobRoleName:'Developer'
          }
        },
        timeslot:{
          timeslotId:1,
          timeslotName:'10:00AM-11:00AM',
        }
      }
    ]
        

    const mockApiRespone:ApiResponse<InterviewSlots[]>={
      data:mockInterviewSlots,
      success:true,
      message:''
    }
    
    spyOn(interviewerPannelService,'getAllInterviewslotsByEmployeeId').and.returnValue(of(mockApiRespone));  
    
    //Act
    component.employeeId=1;
    component.loadInterviewSlots();
    
    //Assert
    // expect(component.existingTimeslots).toEqual(mockInterviewSlots);
    expect(interviewerPannelService.getAllInterviewslotsByEmployeeId).toHaveBeenCalledWith(1);
  });

  it('should should handle false success response while fetching loadInterviewSlots', () => {
    //Arrange
    
    const mockApiRespone:ApiResponse<InterviewSlots[]>={
      data:[],
      success:false,
      message:'Failed to fetch interview slots'
    }
    spyOn(console,'error');
    spyOn(interviewerPannelService,'getAllInterviewslotsByEmployeeId').and.returnValue(of(mockApiRespone));  
    
    //Act
    component.employeeId=1;
    component.loadInterviewSlots();
    
    //Assert
    expect(interviewerPannelService.getAllInterviewslotsByEmployeeId).toHaveBeenCalledWith(1);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch inte4rview slots',mockApiRespone.message);
  });

  it('should should handle error response while fetching loadInterviewSlots', () => {
    //Arrange
    
    const errorMessage:string='Failed to fetch interview slots';
    spyOn(console,'error');
    spyOn(interviewerPannelService,'getAllInterviewslotsByEmployeeId').and.returnValue(throwError(errorMessage));  
    
    //Act
    component.employeeId=1;
    component.loadInterviewSlots();
    
    //Assert
    expect(interviewerPannelService.getAllInterviewslotsByEmployeeId).toHaveBeenCalledWith(1);
    expect(console.error).toHaveBeenCalledWith('Error fetching interview slots:',errorMessage);
  });

  it('should call isTimeslotBooked',()=>{
    //Arrange
    component.existingTimeslots=[{
      employeeId:1,
      isBooked:true,
      slotDate:'07-17-2024',
      slotId:1,
      timeslotId:1,
      employee:{
        firstName:'firstName',
        lastName:'lastName',
        changePassword:true,
        email:'user@gmail.com',
        employeeId:1,
        interviewRound:{
          interviewRoundId:1,
          interviewRoundName:'Technical'
        },
        interviewRoundId:1,
        isAdmin:true,
        isRecruiter:false,
        jobRoleId:1,
        jobRole:{
          jobRoleId:1,
          jobRoleName:'Developer'
        }
      },
      timeslot:{
        timeslotId:1,
        timeslotName:'10:00AM-11:00AM',
      }
    }]

    //Act
    const timeslotId:number=1;
    const result = component.isTimeslotBooked(timeslotId);
    

    //Assert
    expect(result).toBe(true);
  });

  it('should call addInterviewSlot and successfulyy add interviewslot',()=>{
    //Arrange
    component.employeeId =1;
    component.selectedDate ='07-17-2024';

    const mockApiRespone:ApiResponse<string>={
      data:'',
      success:true,
      message:''
    }
    spyOn(interviewerPannelService,'addInterviewSlot').and.returnValue(of(mockApiRespone));
    spyOn(component,'loadTimeslots');
    spyOn(console,'log');

    //Act
    const timeslotId:number=1;
    component.addInterviewSlot(timeslotId);

    //Assert
    expect(console.log).toHaveBeenCalledWith(mockApiRespone);
  });

  it('should call addInterviewSlot and handle error response',()=>{
    //Arrange
    component.employeeId =1;
    component.selectedDate ='07-17-2024';

    const errorMessage:string='Failed to add interview slot';
    spyOn(interviewerPannelService,'addInterviewSlot').and.returnValue(throwError({error:{message:errorMessage}}));
    spyOn(console,'log');
    spyOn(window,'alert');

    //Act
    const timeslotId:number=1;
    component.addInterviewSlot(timeslotId);

    //Assert
    expect(window.alert).toHaveBeenCalledWith(errorMessage);
    
  });

  it('should call deleteInterviewSlot if timeslot is already booked', () => {
    // Arrange
    const timeslotIdToCheck = 1;
    spyOn(component, 'isTimeslotBooked').and.returnValue(true);
    spyOn(component, 'deleteInterviewSlot');

    // Act
    component.handleCheckboxClick(timeslotIdToCheck);

    // Assert
    expect(component.isTimeslotBooked).toHaveBeenCalledWith(timeslotIdToCheck);
    expect(component.deleteInterviewSlot).toHaveBeenCalledWith(timeslotIdToCheck);
  });

  it('should call addInterviewSlot if timeslot is not booked', () => {
    // Arrange
    const timeslotIdToCheck = 2;
    spyOn(component, 'isTimeslotBooked').and.returnValue(false);
    spyOn(component, 'addInterviewSlot');

    // Act
    component.handleCheckboxClick(timeslotIdToCheck);

    // Assert
    expect(component.isTimeslotBooked).toHaveBeenCalledWith(timeslotIdToCheck);
    expect(component.addInterviewSlot).toHaveBeenCalledWith(timeslotIdToCheck);
  });

  it('should delete interview slot and update existingTimeslots', () => {
    // Arrange
    const timeslotIdToDelete = 1;
    const mockApiResponse:ApiResponse<string>={
      data:'',
      success:true,
      message:''
    }; 
    spyOn(interviewerPannelService,'deleteInterviewSlot').and.returnValue(of(mockApiResponse));
    spyOn(console, 'log');

    // Act
    component.deleteInterviewSlot(timeslotIdToDelete);

    // Assert
    expect(interviewerPannelService.deleteInterviewSlot).toHaveBeenCalledWith(component.employeeId, component.selectedDate, timeslotIdToDelete);
    expect(console.log).toHaveBeenCalledOnceWith([]);
  });
  
});
