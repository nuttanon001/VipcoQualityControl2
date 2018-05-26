import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LocationQcEditComponent } from './location-qc-edit.component';

describe('LocationQcEditComponent', () => {
  let component: LocationQcEditComponent;
  let fixture: ComponentFixture<LocationQcEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LocationQcEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LocationQcEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
