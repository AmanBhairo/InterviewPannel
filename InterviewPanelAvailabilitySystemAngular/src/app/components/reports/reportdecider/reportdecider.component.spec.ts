import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportdeciderComponent } from './reportdecider.component';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('ReportdeciderComponent', () => {
  let component: ReportdeciderComponent;
  let fixture: ComponentFixture<ReportdeciderComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, HttpClientTestingModule],
      declarations: [ReportdeciderComponent]
    });
    fixture = TestBed.createComponent(ReportdeciderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call showComponent',()=>{
    //Act
    component.showComponent(1);

    //Assert
    expect(component.selectedComponent).toEqual(1);
  })
});
