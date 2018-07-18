import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WelderNoDialogComponent } from './welder-no-dialog.component';

describe('WelderNoDialogComponent', () => {
  let component: WelderNoDialogComponent;
  let fixture: ComponentFixture<WelderNoDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WelderNoDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WelderNoDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
