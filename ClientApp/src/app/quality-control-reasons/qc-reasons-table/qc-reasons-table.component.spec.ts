import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QcReasonsTableComponent } from './qc-reasons-table.component';

describe('QcReasonsTableComponent', () => {
  let component: QcReasonsTableComponent;
  let fixture: ComponentFixture<QcReasonsTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QcReasonsTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QcReasonsTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
