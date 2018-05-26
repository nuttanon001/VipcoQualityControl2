import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LocationQcTableComponent } from './location-qc-table.component';

describe('LocationQcTableComponent', () => {
  let component: LocationQcTableComponent;
  let fixture: ComponentFixture<LocationQcTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LocationQcTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LocationQcTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
