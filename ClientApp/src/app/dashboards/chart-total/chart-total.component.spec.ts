import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChartTotalComponent } from './chart-total.component';

describe('ChartTotalComponent', () => {
  let component: ChartTotalComponent;
  let fixture: ComponentFixture<ChartTotalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChartTotalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChartTotalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
