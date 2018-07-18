import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RequireQcWelderCenterComponent } from './require-qc-welder-center.component';

describe('RequireQcWelderCenterComponent', () => {
  let component: RequireQcWelderCenterComponent;
  let fixture: ComponentFixture<RequireQcWelderCenterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RequireQcWelderCenterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RequireQcWelderCenterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
