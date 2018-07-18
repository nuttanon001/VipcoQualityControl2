import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WelderTeamInfoDialogComponent } from './welder-team-info-dialog.component';

describe('WelderTeamInfoDialogComponent', () => {
  let component: WelderTeamInfoDialogComponent;
  let fixture: ComponentFixture<WelderTeamInfoDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WelderTeamInfoDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WelderTeamInfoDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
