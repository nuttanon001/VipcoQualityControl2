import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RequireQcReportComponent } from './require-qc-report.component';

describe('RequireQcReportComponent', () => {
  let component: RequireQcReportComponent;
  let fixture: ComponentFixture<RequireQcReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RequireQcReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RequireQcReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
