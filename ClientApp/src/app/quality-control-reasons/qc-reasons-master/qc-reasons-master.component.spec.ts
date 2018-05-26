import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QcReasonsMasterComponent } from './qc-reasons-master.component';

describe('QcReasonsMasterComponent', () => {
  let component: QcReasonsMasterComponent;
  let fixture: ComponentFixture<QcReasonsMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QcReasonsMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QcReasonsMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
