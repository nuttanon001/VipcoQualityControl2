import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QualitycontrolViewExtendComponent } from './qualitycontrol-view-extend.component';

describe('QualitycontrolViewExtendComponent', () => {
  let component: QualitycontrolViewExtendComponent;
  let fixture: ComponentFixture<QualitycontrolViewExtendComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QualitycontrolViewExtendComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QualitycontrolViewExtendComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
