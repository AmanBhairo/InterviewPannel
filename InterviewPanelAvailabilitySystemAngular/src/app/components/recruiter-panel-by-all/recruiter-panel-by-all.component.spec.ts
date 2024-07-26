import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecruiterPanelByAllComponent } from './recruiter-panel-by-all.component';
import { InterviewerService } from 'src/app/services/interviewer.service';
import { RecruiterService } from 'src/app/services/recruiter.service';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule, NgForm } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
// import { RecruiterPanelComponent } from '../recruiter-panel/recruiter-panel.component';
import { JobRole } from 'src/app/models/jobrole.model';
import { InterviewRoundInterviewer } from 'src/app/models/interviewround.interviewer.model';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { of, throwError } from 'rxjs';
import { InterviewSlots } from 'src/app/models/interviewSlots.model';

describe('RecruiterPanelByAllComponent', () => {
  let component: RecruiterPanelByAllComponent;
  let fixture: ComponentFixture<RecruiterPanelByAllComponent>;
  let interviewerServiceSpy: jasmine.SpyObj<InterviewerService>;
  let recruiterServiceSpy: jasmine.SpyObj<RecruiterService>;
  let routerSpy: Router
  beforeEach(() => {
    interviewerServiceSpy = jasmine.createSpyObj('InterviewerService', ['getAllJobRoles', 'getAllInterviewRounds', 'getTotalInterviewersCount']);
    recruiterServiceSpy = jasmine.createSpyObj('RecruiterService', ['getAllInterviwersWithPagination', 'updateInterviewSlot', 'getAllInterviewerCount', 'getTotalInterviewersByInterviewerRound', 'getTotalInterviewersByJobRole', 'getInterviewersByInterviewRound', 'getTotalInterviewSlotsByAll', 'getAllInterviwersWithPaginationByAll']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, HttpClientTestingModule],
      declarations: [RecruiterPanelByAllComponent],
      providers: [{ provide: InterviewerService, useValue: interviewerServiceSpy },
      { provide: RecruiterService, useValue: recruiterServiceSpy },
      ]

    });
    fixture = TestBed.createComponent(RecruiterPanelByAllComponent);
    component = fixture.componentInstance;
    // fixture.detectChanges();
    routerSpy = TestBed.inject(Router)
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should load interview round on init', () => {
    // Arrange
    const mockInterviwers: InterviewRoundInterviewer[] = [
      { interviewRoundId: 1, interviewRoundName: 'Category 1' }
    ];
    const mockJobroles: JobRole[] = [
      { jobRoleId: 1, jobRoleName: 'Category 1' }
    ];

    const mockResponse1: ApiResponse<InterviewRoundInterviewer[]> = { success: false, data: mockInterviwers, message: 'Failed to fetch countries' };
    const mockResponse2: ApiResponse<JobRole[]> = { success: false, data: mockJobroles, message: 'Failed to fetch countries' };
    const mockResponse3: ApiResponse<number> = { success: false, data: 1, message: 'Failed to fetch countries' };
    interviewerServiceSpy.getAllInterviewRounds.and.returnValue(of(mockResponse1));
    interviewerServiceSpy.getAllJobRoles.and.returnValue(of(mockResponse2));
    interviewerServiceSpy.getTotalInterviewersCount.and.returnValue(of(mockResponse3));
    spyOn(component, 'loadInterviewRound');
    spyOn(component, 'loadInterviewerCount');
    spyOn(component, 'loadJobRoles');
    // Act
    component.ngOnInit();
    // fixture.detectChanges();// ngOnInit is called here

    // Assert
    expect(component.loadInterviewRound).toHaveBeenCalled();
    expect(component.loadInterviewerCount).toHaveBeenCalled();
    expect(component.loadJobRoles).toHaveBeenCalled();
  });
  it('should load job roles', () => {
    // Arrange
    const mockStates: JobRole[] = [
      { jobRoleId: 1, jobRoleName: 'Category 1' },
      { jobRoleId: 2, jobRoleName: 'Category 1' },
    ];
    const mockResponse: ApiResponse<JobRole[]> = { success: true, data: mockStates, message: '' };
    interviewerServiceSpy.getAllJobRoles.and.returnValue(of(mockResponse));

    // Act
    component.loadJobRoles()
    // Assert
    expect(interviewerServiceSpy.getAllJobRoles).toHaveBeenCalled();
    expect(component.jobRoles).toEqual(mockStates);
  });

  it('should not load job role when response is false', () => {
    // Arrange

    const mockResponse: ApiResponse<JobRole[]> = { success: false, data: [], message: 'Failed to fetch jobroles' };
    interviewerServiceSpy.getAllJobRoles.and.returnValue(of(mockResponse));
    spyOn(console, 'error');
    // Act
    component.loadJobRoles() // ngOnInit is called here

    // Assert
    expect(interviewerServiceSpy.getAllJobRoles).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch jobroles', 'Failed to fetch jobroles');
  });

  it('should handle error during country loading HTTP Error', () => {
    // Arrange
    const mockError = { message: 'Network error' };
    interviewerServiceSpy.getAllJobRoles.and.returnValue(throwError(() => mockError));
    spyOn(console, 'error');

    // Act
    component.loadJobRoles() // ngOnInit is called here

    // Assert
    expect(interviewerServiceSpy.getAllJobRoles).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error fetching jobroles:', mockError);
  });
  it('should load interview round', () => {
    // Arrange
    const mockStates: InterviewRoundInterviewer[] = [
      { interviewRoundId: 1, interviewRoundName: 'Category 1' },
      { interviewRoundId: 2, interviewRoundName: 'Category 1' },
    ];
    const mockResponse: ApiResponse<InterviewRoundInterviewer[]> = { success: true, data: mockStates, message: '' };
    interviewerServiceSpy.getAllInterviewRounds.and.returnValue(of(mockResponse));

    // Act
    component.loadInterviewRound()
    // Assert
    expect(interviewerServiceSpy.getAllInterviewRounds).toHaveBeenCalled();
    expect(component.interviewRoundInterviewers).toEqual(mockStates);
  });

  it('should not load interview round when response is false', () => {
    // Arrange

    const mockResponse: ApiResponse<InterviewRoundInterviewer[]> = { success: false, data: [], message: 'Failed to fetch interviewRound' };
    interviewerServiceSpy.getAllInterviewRounds.and.returnValue(of(mockResponse));
    spyOn(console, 'error');
    // Act
    component.loadInterviewRound() // ngOnInit is called here

    // Assert
    expect(interviewerServiceSpy.getAllInterviewRounds).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch interviewRound', 'Failed to fetch interviewRound');
  });

  it('should handle error during country loading HTTP Error', () => {
    // Arrange
    const mockError = { message: 'Network error' };
    interviewerServiceSpy.getAllInterviewRounds.and.returnValue(throwError(() => mockError));
    spyOn(console, 'error');

    // Act
    component.loadInterviewRound() // ngOnInit is called here

    // Assert
    expect(interviewerServiceSpy.getAllInterviewRounds).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error fetching interviewRound:', mockError);
  });
  //onPageChange
  it('should change the current page and load interviewers', () => {
    // Arrange
    spyOn(component, 'loadInterviewerCount');
    const newPage = 3;
    component.searchQuery = 'test';

    // Act
    component.changePagePagination(newPage);

    // Assert
    expect(component.pageNumber).toBe(newPage);
    expect(component.loadInterviewerCount).toHaveBeenCalledWith();
  });



  //onPageSizeChange

  it('should reset current page, load interviewers, and total count', () => {
    // Arrange
    component.pageNumber = 1;
    component.pageSize = 1;
    component.recruiterDetailsPagination.length = 1;

    spyOn(component, 'loadInterviewerCount');

    // Act
    component.changePageSizePagination(2);

    // Assert
    expect(component.pageNumber).toBe(1);
    expect(component.loadInterviewerCount).toHaveBeenCalled();
  });

  it('should toggle desc sort order and reload data', () => {
    // Arrange
    spyOn(component, 'loadInterviewerCount');
    component.pageNumber = 1;

    // Act
    component.onClickSort();

    // Assert
    expect(component.sort).toBe('desc');
    expect(component.pageNumber).toBe(1);
    expect(component.loadInterviewerCount).toHaveBeenCalledWith();
  });

  it('should toggle asc sort order and reload data', () => {
    // Arrange
    spyOn(component, 'loadInterviewerCount');
    component.sort = 'desc'
    // Act
    component.onClickSort();

    // Assert
    expect(component.sort).toBe('asc');
    expect(component.pageNumber).toBe(1);
    expect(component.loadInterviewerCount).toHaveBeenCalledWith();
  });
  it('should load count on select job role', () => {
    // Arrange
    spyOn(component, 'loadInterviewerCount');
    component.recruiterDetailsPagination = [];
    component.pageNumber = 1;
    const jobRoleId = 1;
    component.jobRole.jobRoleId = 1;
    // Act
    component.onSelectJobRole(jobRoleId);

    // Assert
    expect(component.pageNumber).toBe(1);
    expect(component.loadInterviewerCount).toHaveBeenCalledWith();
  });
  it('should load count on select interview round', () => {
    // Arrange
    spyOn(component, 'loadInterviewerCount');
    component.recruiterDetailsPagination = [];
    component.pageNumber = 1;
    const interviewRoundId = 1;
    component.interviewRoundInterviewer.interviewRoundId = 1;
    // Act
    component.onSelectInterviewRound(interviewRoundId);

    // Assert
    expect(component.pageNumber).toBe(1);
    expect(component.loadInterviewerCount).toHaveBeenCalledWith();
  });

  it('should search on onSearch', () => {
    // Arrange
    spyOn(component, 'loadInterviewerCount');
    component.recruiterDetailsPagination = [];
    component.pageNumber = 1;
    // Act
    component.onSearch();

    // Assert
    expect(component.pageNumber).toBe(1);
    expect(component.loadInterviewerCount).toHaveBeenCalledWith();
  });
  it('should clear search on clearSearch', () => {
    // Arrange
    spyOn(component, 'loadInterviewerCount');
    component.recruiterDetailsPagination = [];
    component.pageNumber = 1;
    component.searchQuery = ''
    // Act
    component.clearSearch();

    // Assert
    expect(component.pageNumber).toBe(1);
    expect(component.searchQuery).toBe('');
    expect(component.loadInterviewerCount).toHaveBeenCalledWith();
  });
  it('should fail to count total interviewer', () => {
    //Arrange
    const mockResponse: ApiResponse<number> = { success: false, data: 0, message: 'Failed to fetch contacts count' };
    recruiterServiceSpy.getTotalInterviewSlotsByAll.and.returnValue(of(mockResponse));

    spyOn(console, 'error')
    //Act
    component.loadInterviewerCount();
    //Assert
    expect(recruiterServiceSpy.getTotalInterviewSlotsByAll).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch count', 'Failed to fetch contacts count');
  });

  it('should handle Http error response', () => {
    //Arrange
    const mockError = { message: 'Network Error' };
    recruiterServiceSpy.getTotalInterviewSlotsByAll.and.returnValue(throwError(mockError));
    spyOn(console, 'error')
    //Act
    component.loadInterviewerCount();
    //Assert
    expect(recruiterServiceSpy.getTotalInterviewSlotsByAll).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error fetching count.', mockError);
  });
  it('should get all inrerviewer with pagination', () => {
    //Arrange
    const mockResponse: ApiResponse<number> = { success: true, data: 2, message: '' };
    const mockResponseForSlots: ApiResponse<InterviewSlots[]> = { success: true, data: [], message: '' };
    component.totalItems = mockResponse.data;
    component.totalPages = Math.ceil(1 / 2);
    recruiterServiceSpy.getTotalInterviewSlotsByAll.and.returnValue(of(mockResponse));

    recruiterServiceSpy.getAllInterviwersWithPaginationByAll.and.returnValue(of(mockResponseForSlots));

    //Act

    component.loadInterviewerCount();
    //Assert
    expect(recruiterServiceSpy.getAllInterviwersWithPaginationByAll).toHaveBeenCalled();
  });
  it('should handle error fetching recruiters with pagination', () => {
    // Arrange
    const mockErrorResponse = { success: false, message: 'Failed to fetch recruiters' };
    recruiterServiceSpy.getAllInterviwersWithPaginationByAll.and.returnValue(throwError(mockErrorResponse));

    // Act
    component.getAllInterviwersWithPaginationByAll('testQuery', 1, 2);

    // Assert
    expect(recruiterServiceSpy.getAllInterviwersWithPaginationByAll).toHaveBeenCalled();
    expect(component.loading).toBeFalse();
    expect(component.recruiterDetailsPagination).toEqual([]);
  });
  it('should get all inrerviewer with pagination', () => {
    //Arrange
   const mockResponseForSlots: ApiResponse<InterviewSlots[]> = { success: false, data: [], message: '' };
    component.recruiterDetailsPagination=[];
    
    recruiterServiceSpy.getAllInterviwersWithPaginationByAll.and.returnValue(of(mockResponseForSlots));
    spyOn(console,'error');
    //Act

    component.getAllInterviwersWithPaginationByAll('a',1,1);
    //Assert
    expect(recruiterServiceSpy.getAllInterviwersWithPaginationByAll).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch recruiters with pagination',mockResponseForSlots.message);
  });
   //onShowAll
   it('should reset current page, load interviewers, and total count', () => {
    // Arrange
    spyOn(component, 'loadInterviewerCount'); 
    component.pageNumber = 2; 
    component.searchQuery = 'test'; 

    // Act
    component.onShowAll();

    // Assert
    expect(component.pageNumber).toBe(2); 
    expect(component.loadInterviewerCount).toHaveBeenCalledWith();
  });

  it('should reset current page and call methods without search query', () => {
    // Arrange
    spyOn(component, 'loadInterviewerCount'); 
    component.pageNumber = 3; 
    component.searchQuery = ''; 

    // Act
    component.onShowAll();

    // Assert
    expect(component.pageNumber).toBe(3); 
    expect(component.loadInterviewerCount).toHaveBeenCalled(); 
  });

//   it('should alert error message on unsuccessful product updation', () => {
//    // Arrange
//    const mockResponse: ApiResponse<InterviewSlots[]> = { success: false, data: [], message: '' };
//    const slotId = 1;
//    recruiterServiceSpy.updateInterviewSlot.and.returnValue(of(mockResponse));
// component.interviewSlots=mockResponse.data;
// spyOn(component,"ngOnInit")
// spyOn(component,'loadJobRoles');
// spyOn(component,'loadInterviewRound');
// spyOn(component,'loadInterviewerCount');
//    // Act
//    component.onClickUpdate(slotId);
//    // Assert
//    expect(recruiterServiceSpy.updateInterviewSlot).toHaveBeenCalledWith(slotId);
//    expect(component.interviewSlots).toEqual(mockResponse.data);
//   });
//   it('should update interview slot successfully', () => {
//     // Arrange
//     const mockResponse: ApiResponse<InterviewSlots[]> = { success: true, data: [], message: '' };
//     const slotId = 1;
//     recruiterServiceSpy.updateInterviewSlot.and.returnValue(of(mockResponse));
// component.interviewSlots=mockResponse.data;
// component.ngOnInit();
// // spyOn(component,'ngOnInit')
// spyOn(component,'loadJobRoles');
// spyOn(component,'loadInterviewRound');
// spyOn(component,'loadInterviewerCount');
//     // Act
//     component.onClickUpdate(slotId);
//     // Assert
//     expect(recruiterServiceSpy.updateInterviewSlot).toHaveBeenCalledWith(slotId);
//     expect(component.interviewSlots).toEqual(mockResponse.data);
//    });

  it('should handle error updating interview slot', () => {
    // Arrange
    const mockErrorResponse = { success: false, message: 'Failed to update slot' };
    const slotId = 1;
    recruiterServiceSpy.updateInterviewSlot.and.returnValue(throwError(mockErrorResponse));
spyOn(console,'error');
component.recruiterDetailsPagination=[]
    // Act
    component.onClickUpdate(slotId);
   
    // Assert
    expect(recruiterServiceSpy.updateInterviewSlot).toHaveBeenCalledWith(slotId);
    expect(console.error).toHaveBeenCalledWith('Failed to fetch count', mockErrorResponse);
    expect(component.recruiterDetailsPagination).toEqual([]);
    // Add more specific assertions as needed
  });
});

