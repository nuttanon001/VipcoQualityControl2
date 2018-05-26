import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LocationQcHasWorkgroupComponent } from './location-qc-has-workgroup.component';

describe('LocationQcHasWorkgroupComponent', () => {
  let component: LocationQcHasWorkgroupComponent;
  let fixture: ComponentFixture<LocationQcHasWorkgroupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LocationQcHasWorkgroupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LocationQcHasWorkgroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
