import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WelderProjectInfoComponent } from './welder-project-info.component';

describe('WelderProjectInfoComponent', () => {
  let component: WelderProjectInfoComponent;
  let fixture: ComponentFixture<WelderProjectInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WelderProjectInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WelderProjectInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
