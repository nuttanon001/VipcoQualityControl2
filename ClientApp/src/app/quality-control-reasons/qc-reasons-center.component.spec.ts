import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QcReasonsCenterComponent } from './qc-reasons-center.component';

describe('QcReasonsCenterComponent', () => {
  let component: QcReasonsCenterComponent;
  let fixture: ComponentFixture<QcReasonsCenterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QcReasonsCenterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QcReasonsCenterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
