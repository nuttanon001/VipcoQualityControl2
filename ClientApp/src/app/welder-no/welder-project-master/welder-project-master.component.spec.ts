import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WelderProjectMasterComponent } from './welder-project-master.component';

describe('WelderProjectMasterComponent', () => {
  let component: WelderProjectMasterComponent;
  let fixture: ComponentFixture<WelderProjectMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WelderProjectMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WelderProjectMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
