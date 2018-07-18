import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RequireQcWelderTableComponent } from './require-qc-welder-table.component';

describe('RequireQcWelderTableComponent', () => {
  let component: RequireQcWelderTableComponent;
  let fixture: ComponentFixture<RequireQcWelderTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RequireQcWelderTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RequireQcWelderTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
