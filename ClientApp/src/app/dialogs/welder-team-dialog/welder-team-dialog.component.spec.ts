import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WelderTeamDialogComponent } from './welder-team-dialog.component';

describe('WelderTeamDialogComponent', () => {
  let component: WelderTeamDialogComponent;
  let fixture: ComponentFixture<WelderTeamDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WelderTeamDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WelderTeamDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
