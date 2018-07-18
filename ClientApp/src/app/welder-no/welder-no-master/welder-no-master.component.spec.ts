import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WelderNoMasterComponent } from './welder-no-master.component';

describe('WelderNoMasterComponent', () => {
  let component: WelderNoMasterComponent;
  let fixture: ComponentFixture<WelderNoMasterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WelderNoMasterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WelderNoMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
