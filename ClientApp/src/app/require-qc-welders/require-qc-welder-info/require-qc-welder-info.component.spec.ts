import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RequireQcWelderInfoComponent } from './require-qc-welder-info.component';

describe('RequireQcWelderInfoComponent', () => {
  let component: RequireQcWelderInfoComponent;
  let fixture: ComponentFixture<RequireQcWelderInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RequireQcWelderInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RequireQcWelderInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
