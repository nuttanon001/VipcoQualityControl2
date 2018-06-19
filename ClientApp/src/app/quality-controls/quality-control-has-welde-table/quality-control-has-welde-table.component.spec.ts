import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QualityControlHasWeldeTableComponent } from './quality-control-has-welde-table.component';

describe('QualityControlHasWeldeTableComponent', () => {
  let component: QualityControlHasWeldeTableComponent;
  let fixture: ComponentFixture<QualityControlHasWeldeTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QualityControlHasWeldeTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QualityControlHasWeldeTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
