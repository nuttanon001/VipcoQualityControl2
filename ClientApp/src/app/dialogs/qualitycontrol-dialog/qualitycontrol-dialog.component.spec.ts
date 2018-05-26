import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QualitycontrolDialogComponent } from './qualitycontrol-dialog.component';

describe('QualitycontrolDialogComponent', () => {
  let component: QualitycontrolDialogComponent;
  let fixture: ComponentFixture<QualitycontrolDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QualitycontrolDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QualitycontrolDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
