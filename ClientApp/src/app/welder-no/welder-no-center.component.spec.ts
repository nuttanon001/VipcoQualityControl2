import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WelderNoCenterComponent } from './welder-no-center.component';

describe('WelderNoCenterComponent', () => {
  let component: WelderNoCenterComponent;
  let fixture: ComponentFixture<WelderNoCenterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WelderNoCenterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WelderNoCenterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
