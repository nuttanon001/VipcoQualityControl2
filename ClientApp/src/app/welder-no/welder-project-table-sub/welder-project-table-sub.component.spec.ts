import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WelderProjectTableSubComponent } from './welder-project-table-sub.component';

describe('WelderProjectTableSubComponent', () => {
  let component: WelderProjectTableSubComponent;
  let fixture: ComponentFixture<WelderProjectTableSubComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WelderProjectTableSubComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WelderProjectTableSubComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
