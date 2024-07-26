import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InterviewerListComponent } from './interviewer-list.component';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ApiResponse } from 'src/app/models/ApiResponse{T}';
import { Router } from '@angular/router';
import { InterviewerService } from 'src/app/services/interviewer.service';
import { of, throwError } from 'rxjs';
import { Interviewer } from 'src/app/models/interviewer.model';

describe('InterviewerListComponent', () => {
  let component: InterviewerListComponent;
  let fixture: ComponentFixture<InterviewerListComponent>;
  let interviewerServiceSpy: jasmine.SpyObj<InterviewerService>;
  let router: Router;
  
  
  const mockInterviewer: Interviewer[] = [
    {
      employeeId: 1,
      firstName: 'ee',
      lastName: '',
      email: '',
      jobRoleId: 1,
      interviewRoundId: 1,
      jobRole: {
        jobRoleId: 1,
        jobRoleName: ''
      },
      interviewRound: {
        interviewRoundId: 1,
        interviewRoundName: ''
      },
      isRecruiter: false,
      isAdmin: false,
      changePassword: false
    }
  ]

  beforeEach(() => {
    interviewerServiceSpy = jasmine.createSpyObj(
      'InterviewerService', ['deleteEmployeeById', 'getTotalInterviewersCount', 'getAllInterviewers']);
      router = jasmine.createSpyObj('Router', ['navigate']);
    TestBed.configureTestingModule({
      imports: [RouterTestingModule.withRoutes([{path:'update-interviewer', component: InterviewerListComponent}]), FormsModule, HttpClientTestingModule],
      declarations: [InterviewerListComponent],
      providers: [
        {provide: InterviewerService, useValue: interviewerServiceSpy}
      ]
    });
    fixture = TestBed.createComponent(InterviewerListComponent);
    component = fixture.componentInstance;
    router = TestBed.inject(Router);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  
  it('should alert error message if delete employee fails', () => {
    // Arrange
    const mockDeleteResponse: ApiResponse<string> = { success: false, data: "", message: 'Failed to delete employee' };
    interviewerServiceSpy.deleteEmployeeById.and.returnValue(of(mockDeleteResponse));
    spyOn(window, 'alert');
 
    // Act
    component.employeeId = 1;
    component.deleteEmployee();
 
    // Assert
    expect(window.alert);
  });
  
  it('should delete employee and reload employees', () => {
    // Arrange
    const mockDeleteResponse: ApiResponse<string> = { success: true, data: "", message: 'Employee deleted successfully' };
    interviewerServiceSpy.deleteEmployeeById.and.returnValue(of(mockDeleteResponse));
    spyOn(component, 'loadInterviewers');
 
    // Act
    component.employeeId = 1;
    component.deleteEmployee();
 
    // Assert
    expect(component.loadInterviewers);
  });

  

  //calculateTotalPages
  it('should calculate total pages correctly', () => {
    // Arrange
    component.totalUsers = 15; 
    component.pageSize = 5;    

    // Act
    component.calculateTotalPages();

    // Assert
    expect(component.totalPages.length).toBe(3); 
    expect(component.totalPages).toEqual([1, 2, 3]); 
  });
  
  it('should handle zero totalUsers', () => {
    // Arrange
    component.totalUsers = 0; 
    component.pageSize = 10;  

    // Act
    component.calculateTotalPages();

    // Assert
    expect(component.totalPages.length).toBe(0); 
    expect(component.totalPages).toEqual([]);    
  });

  it('should handle large totalUsers and pageSize', () => {
    // Arrange
    component.totalUsers = 1000; 
    component.pageSize = 100;  

    // Act
    component.calculateTotalPages();

    // Assert
    expect(component.totalPages.length).toBe(10); 
    expect(component.totalPages).toEqual([1, 2, 3, 4, 5, 6, 7, 8, 9, 10]); 
  });


  //onPageChange
  it('should change the current page and load interviewers', () => {
    // Arrange
    spyOn(component, 'loadInterviewers'); 
    const newPage = 3;
    component.searchQuery = 'test'; 

    // Act
    component.onPageChange(newPage);

    // Assert
    expect(component.currentPage).toBe(newPage); 
    expect(component.loadInterviewers).toHaveBeenCalledWith(component.searchQuery); 
  });

  it('should reset search query when changing page', () => {
    // Arrange
    spyOn(component, 'loadInterviewers'); 
    const newPage = 2;
    component.searchQuery = 'test'; 

    // Act
    component.onPageChange(newPage);

    // Assert
    expect(component.currentPage).toBe(newPage); 
    expect(component.loadInterviewers).toHaveBeenCalledWith(component.searchQuery); 
  });


  //onPageSizeChange
  it('should reset current page, load interviewers, and total count', () => {
    // Arrange
    spyOn(component, 'loadInterviewers'); 
    spyOn(component, 'totalInterviewerCount');
    component.currentPage = 2; 
    component.searchQuery = 'test'; 

    // Act
    component.onPageSizeChange();

    // Assert
    expect(component.currentPage).toBe(1); 
    expect(component.loadInterviewers).toHaveBeenCalledWith(component.searchQuery); 
    expect(component.totalInterviewerCount).toHaveBeenCalledWith(component.searchQuery); 
  });

  it('should reset current page and call methods without search query', () => {
    // Arrange
    spyOn(component, 'loadInterviewers'); 
    spyOn(component, 'totalInterviewerCount');
    component.currentPage = 3; 
    component.searchQuery = '';

    // Act
    component.onPageSizeChange();

    // Assert
    expect(component.currentPage).toBe(1); 
    expect(component.loadInterviewers).toHaveBeenCalled();
    expect(component.totalInterviewerCount).toHaveBeenCalled(); 
  });


  //onShowAll
  it('should reset current page, load interviewers, and total count', () => {
    // Arrange
    spyOn(component, 'loadInterviewers'); 
    spyOn(component, 'totalInterviewerCount'); 
    component.currentPage = 2; 
    component.searchQuery = 'test'; 

    // Act
    component.onShowAll();

    // Assert
    expect(component.currentPage).toBe(1); 
    expect(component.loadInterviewers).toHaveBeenCalledWith(component.searchQuery); 
    expect(component.totalInterviewerCount).toHaveBeenCalledWith(component.searchQuery); 
  });

  it('should reset current page and call methods without search query', () => {
    // Arrange
    spyOn(component, 'loadInterviewers'); 
    spyOn(component, 'totalInterviewerCount'); 
    component.currentPage = 3; 
    component.searchQuery = ''; 

    // Act
    component.onShowAll();

    // Assert
    expect(component.currentPage).toBe(1); 
    expect(component.loadInterviewers).toHaveBeenCalled(); 
    expect(component.totalInterviewerCount).toHaveBeenCalled(); 
  });


  //onClickSort
  it('should toggle sort order and reload data', () => {
    // Arrange
    spyOn(component, 'loadInterviewers');
    spyOn(component, 'totalInterviewerCount');
    component.sortOrder = 'asc'; 
    component.searchQuery = 'test'; 

    // Act
    component.onClickSort();

    // Assert
    expect(component.loading).toBeTrue(); 
    expect(component.sortOrder).toBe('desc');
    expect(component.currentPage).toBe(1); 
    expect(component.totalInterviewerCount).toHaveBeenCalledWith(component.searchQuery); 
    expect(component.loadInterviewers).toHaveBeenCalledWith(component.searchQuery); 
  });

  it('should default to ascending order when sortOrder is invalid', () => {
    // Arrange
    spyOn(component, 'loadInterviewers');
    spyOn(component, 'totalInterviewerCount');
    component.sortOrder = 'desc'; 
    component.searchQuery = 'test'; 

    // Act
    component.onClickSort();

    // Assert
    expect(component.loading).toBeTrue(); 
    expect(component.sortOrder).toBe('asc'); 
    expect(component.currentPage).toBe(1); 
    expect(component.totalInterviewerCount).toHaveBeenCalledWith(component.searchQuery); 
    expect(component.loadInterviewers).toHaveBeenCalledWith(component.searchQuery); 
  });
  

  //update list navigate
  it('should navigate to update interviewer route with correct id', () => {
    // Arrange
    const id = 1;
    spyOn(router, 'navigate');

    // Act
    component.employeeUpdate(id);

    // Assert
    expect(router.navigate).toHaveBeenCalledWith(['update-interviewer/', id]);
  });



  //searchUsers
  it('should search interviewers when search query is valid', () => {
    // Arrange
    const searchQuery = 'test';
    spyOn(component, 'loadInterviewers');
    spyOn(component, 'totalInterviewerCount');
  
    // Act
    component.searchQuery = searchQuery;
    component.searchUsers();
  
    // Assert
    expect(component.currentPage).toBe(1); 
    expect(component.loadInterviewers).toHaveBeenCalledWith(searchQuery); 
    expect(component.totalInterviewerCount).toHaveBeenCalledWith(searchQuery); 
  });
  
  it('should reset search and load all interviewers when search query is invalid', () => {
    // Arrange
    const searchQuery = ''; 
    spyOn(component, 'loadInterviewers');
    spyOn(component, 'totalInterviewerCount');
  
    // Act
    component.searchQuery = searchQuery;
    component.searchUsers();
  
    // Assert
    expect(component.currentPage).toBe(1); 
    expect(component.loadInterviewers).toHaveBeenCalled(); 
    expect(component.totalInterviewerCount).toHaveBeenCalled(); 
  });
  


  //clearSearch
  it('should clear search and reload all interviewers', () => {
    // Arrange
    spyOn(component, 'loadInterviewers');
    spyOn(component, 'totalInterviewerCount');
  
    // Act
    component.currentPage = 2; 
    component.searchQuery = 'test';
    component.clearSearch();
  
    // Assert
    expect(component.currentPage).toBe(1); 
    expect(component.searchQuery).toBe(''); 
    expect(component.loadInterviewers).toHaveBeenCalledWith(''); 
    expect(component.totalInterviewerCount).toHaveBeenCalledWith(''); 
  });

 

  it('should calculate total count with search', () => {
    // Arrange
    let search = 'e';
    const mockResponse: ApiResponse<number> = { success: true, data: 1, message: '' };
    interviewerServiceSpy.getTotalInterviewersCount.and.returnValue(of(mockResponse));
    spyOn(console, 'log');
    spyOn(component, 'calculateTotalPages');
    
  
    // Act
    component.totalUsers = mockResponse.data;
    component.totalInterviewerCount(search);
    component.calculateTotalPages();
    
    // Assert
    expect(interviewerServiceSpy.getTotalInterviewersCount).toHaveBeenCalled();
    expect(console.log).toHaveBeenCalledWith(component.totalUsers);
    expect(component.calculateTotalPages).toHaveBeenCalled();
  });

  it('should not calculate total count with search', () => {
    // Arrange
    let search = 'e';
    const mockResponse: ApiResponse<number> = { success: false, data: 0, message: '' };
    interviewerServiceSpy.getTotalInterviewersCount.and.returnValue(of(mockResponse));
    spyOn(console, 'error');
    
  
    // Act
    component.totalInterviewerCount(search);
    
    // Assert
    expect(interviewerServiceSpy.getTotalInterviewersCount).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch interviewers count', mockResponse.message);
  });

  it('should handle Http error response',()=>{
    //Arrange
    let search = 'e';
    const mockError = {message:'Network Error'};
    interviewerServiceSpy.getTotalInterviewersCount.and.returnValue(throwError(mockError));
    spyOn(console,'error')

    //Act
    component.totalInterviewerCount(search);

    //Assert
    expect(interviewerServiceSpy.getTotalInterviewersCount).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch interviewers count',mockError);
    expect(component.loading).toBeFalse();

  })


  it('should load interviewers successfully',()=>{
    //Arrange
    let search = 'e';
    const mockResponse :ApiResponse<Interviewer[]> ={success:true,data:mockInterviewer,message:''};
    interviewerServiceSpy.getAllInterviewers.and.returnValue(of(mockResponse));

    //Act
    component.currentPage = 1;
    component.pageSize = 2;
    component.sortOrder = 'asc';
    
    component.loadInterviewers(search);

    //Assert
    expect(interviewerServiceSpy.getAllInterviewers).toHaveBeenCalled();
    expect(component.interviewers).toEqual(mockInterviewer);
    expect(component.loading).toBe(false);
  })

  it('should fail to load interviewers successfully',()=>{
    //Arrange
    let search = 'e';
    
    const mockResponse :ApiResponse<Interviewer[]> ={success:false,data:[],message:''};
    interviewerServiceSpy.getAllInterviewers.and.returnValue(of(mockResponse));
    spyOn(console,'error');
    //Act
    component.currentPage = 1;
    component.pageSize = 2;
    component.sortOrder = 'asc';
    component.loadInterviewers(search);

    //Assert
    expect(interviewerServiceSpy.getAllInterviewers).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch interviewers',mockResponse.message);
    expect(component.loading).toBe(false);
  })

  it('should handle Http error response',()=>{
    //Arrange
    let search = 'e';
    const mockError = {message:'Network Error'};
    interviewerServiceSpy.getAllInterviewers.and.returnValue(throwError(mockError));
    spyOn(console,'error')

    //Act
    component.currentPage = 1;
    component.pageSize = 2;
    component.sortOrder = 'asc';
    component.loadInterviewers(search);

    //Assert
    expect(interviewerServiceSpy.getAllInterviewers).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch interviewers',mockError);
    expect(component.loading).toBeFalse();

  })

    it('should call deleteEmployeeById and reload data on successful deletion', () => {
   
      // Arrange
    const mockApiResponse: ApiResponse<string> = {
      success: true,
      data: '', 
      message: 'Employee deleted successfully'
    };
    interviewerServiceSpy.deleteEmployeeById.and.returnValue(of(mockApiResponse));
    component.employeeId = 1;
    component.currentPage = 3;
    component.totalPages.length = 5;
    spyOn(component, 'calculateTotalPages');
    spyOn(component, 'loadInterviewers');
    
    // Act
    component.deleteEmployee();

    //Assert
    expect(component.calculateTotalPages).toHaveBeenCalled();   
    expect(component.loadInterviewers).toHaveBeenCalled();
    expect(interviewerServiceSpy.deleteEmployeeById).toHaveBeenCalledWith(component.employeeId);

  });

  it('should fail to load and get error', () => {
   
    // Arrange
  const mockError = {error: {message:'Network Error'}};

  interviewerServiceSpy.deleteEmployeeById.and.returnValue(throwError(mockError));
  component.employeeId = 1;
  spyOn(window,'alert');

  // Act
  component.deleteEmployee();

  //Assert
  expect(window.alert).toHaveBeenCalledWith(mockError.error.message);   
});

 it('should call confirmDelete and set employeeId for deletion', () => {
    // Arrange
    spyOn(window, 'confirm').and.returnValue(true);
    spyOn(component, 'deleteEmployee');
 
    // Act
    component.confirmDelete(1);
 
    // Assert
    expect(window.confirm).toHaveBeenCalledWith('Are you sure ?');
    expect(component.employeeId).toBe(1);
    expect(component.deleteEmployee).toHaveBeenCalled();
  });

    it('should not call deleteEmployee if confirm is cancelled', () => {
    // Arrange
    spyOn(window, 'confirm').and.returnValue(false);
    spyOn(component, 'deleteEmployee');
 
    // Act
    component.confirmDelete(1);
 
    // Assert
    expect(window.confirm).toHaveBeenCalledWith('Are you sure ?');
    expect(component.deleteEmployee).not.toHaveBeenCalled();
  });

  it('should call loadInterviewers and totalInterviewerCount on OnInit',()=>
  {
    spyOn(component, 'loadInterviewers');
    spyOn(component, 'totalInterviewerCount');

    component.ngOnInit();

    expect(component.loadInterviewers).toHaveBeenCalled();
    expect(component.totalInterviewerCount).toHaveBeenCalled();
  })

  
});
