import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QcWelderCenterComponent } from './qc-welder-center.component';

describe('QcWelderCenterComponent', () => {
  let component: QcWelderCenterComponent;
  let fixture: ComponentFixture<QcWelderCenterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QcWelderCenterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QcWelderCenterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
