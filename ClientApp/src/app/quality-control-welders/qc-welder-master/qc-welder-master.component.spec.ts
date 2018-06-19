import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QcWelderMasterComponent } from './qc-welder-master.component';

describe('QcWelderMasterComponent', () => {
  let component: QcWelderMasterComponent;
  let fixture: ComponentFixture<QcWelderMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QcWelderMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QcWelderMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
