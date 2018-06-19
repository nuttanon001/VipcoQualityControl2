import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QcWelderTableComponent } from './qc-welder-table.component';

describe('QcWelderTableComponent', () => {
  let component: QcWelderTableComponent;
  let fixture: ComponentFixture<QcWelderTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QcWelderTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QcWelderTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
