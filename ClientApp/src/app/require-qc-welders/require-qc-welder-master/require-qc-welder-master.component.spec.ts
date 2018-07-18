import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RequireQcWelderMasterComponent } from './require-qc-welder-master.component';

describe('RequireQcWelderMasterComponent', () => {
  let component: RequireQcWelderMasterComponent;
  let fixture: ComponentFixture<RequireQcWelderMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RequireQcWelderMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RequireQcWelderMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
