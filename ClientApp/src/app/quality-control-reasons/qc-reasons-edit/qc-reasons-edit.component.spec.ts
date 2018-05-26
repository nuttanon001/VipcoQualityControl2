import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QcReasonsEditComponent } from './qc-reasons-edit.component';

describe('QcReasonsEditComponent', () => {
  let component: QcReasonsEditComponent;
  let fixture: ComponentFixture<QcReasonsEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QcReasonsEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QcReasonsEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
