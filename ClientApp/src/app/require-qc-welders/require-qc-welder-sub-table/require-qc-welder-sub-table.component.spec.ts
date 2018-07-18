import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RequireQcWelderSubTableComponent } from './require-qc-welder-sub-table.component';

describe('RequireQcWelderSubTableComponent', () => {
  let component: RequireQcWelderSubTableComponent;
  let fixture: ComponentFixture<RequireQcWelderSubTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RequireQcWelderSubTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RequireQcWelderSubTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
