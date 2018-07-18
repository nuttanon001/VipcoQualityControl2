import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChartPassComponent } from './chart-pass.component';

describe('ChartPassComponent', () => {
  let component: ChartPassComponent;
  let fixture: ComponentFixture<ChartPassComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChartPassComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChartPassComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
