import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChartTop3Component } from './chart-top3.component';

describe('ChartTop3Component', () => {
  let component: ChartTop3Component;
  let fixture: ComponentFixture<ChartTop3Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChartTop3Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChartTop3Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
