import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RequireQcWelderDialogComponent } from './require-qc-welder-dialog.component';

describe('RequireQcWelderDialogComponent', () => {
  let component: RequireQcWelderDialogComponent;
  let fixture: ComponentFixture<RequireQcWelderDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RequireQcWelderDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RequireQcWelderDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
