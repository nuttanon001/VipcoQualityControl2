import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QcReasonsViewComponent } from './qc-reasons-view.component';

describe('QcReasonsViewComponent', () => {
  let component: QcReasonsViewComponent;
  let fixture: ComponentFixture<QcReasonsViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QcReasonsViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QcReasonsViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
