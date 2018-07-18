import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WelderNoTableComponent } from './welder-no-table.component';

describe('WelderNoTableComponent', () => {
  let component: WelderNoTableComponent;
  let fixture: ComponentFixture<WelderNoTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WelderNoTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WelderNoTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
