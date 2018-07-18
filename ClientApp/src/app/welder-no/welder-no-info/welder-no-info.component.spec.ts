import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WelderNoInfoComponent } from './welder-no-info.component';

describe('WelderNoInfoComponent', () => {
  let component: WelderNoInfoComponent;
  let fixture: ComponentFixture<WelderNoInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WelderNoInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WelderNoInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
