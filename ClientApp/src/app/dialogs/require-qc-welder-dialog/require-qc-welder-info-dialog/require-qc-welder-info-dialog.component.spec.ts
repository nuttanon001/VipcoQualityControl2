import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RequireQcWelderInfoDialogComponent } from './require-qc-welder-info-dialog.component';

describe('RequireQcWelderInfoDialogComponent', () => {
  let component: RequireQcWelderInfoDialogComponent;
  let fixture: ComponentFixture<RequireQcWelderInfoDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RequireQcWelderInfoDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RequireQcWelderInfoDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
