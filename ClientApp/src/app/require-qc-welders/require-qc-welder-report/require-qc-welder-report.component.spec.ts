import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RequireQcWelderReportComponent } from './require-qc-welder-report.component';

describe('RequireQcWelderReportComponent', () => {
  let component: RequireQcWelderReportComponent;
  let fixture: ComponentFixture<RequireQcWelderReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RequireQcWelderReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RequireQcWelderReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
