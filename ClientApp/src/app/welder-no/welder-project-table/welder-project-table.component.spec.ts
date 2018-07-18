import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WelderProjectTableComponent } from './welder-project-table.component';

describe('WelderProjectTableComponent', () => {
  let component: WelderProjectTableComponent;
  let fixture: ComponentFixture<WelderProjectTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WelderProjectTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WelderProjectTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
