import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LocationQcMasterComponent } from './location-qc-master.component';

describe('LocationQcMasterComponent', () => {
  let component: LocationQcMasterComponent;
  let fixture: ComponentFixture<LocationQcMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LocationQcMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LocationQcMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
