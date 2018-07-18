import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChartFailComponent } from './chart-fail.component';

describe('ChartFailComponent', () => {
  let component: ChartFailComponent;
  let fixture: ComponentFixture<ChartFailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChartFailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChartFailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
