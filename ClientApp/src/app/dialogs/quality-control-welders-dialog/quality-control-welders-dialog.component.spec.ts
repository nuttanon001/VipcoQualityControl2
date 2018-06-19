import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QualityControlWeldersDialogComponent } from './quality-control-welders-dialog.component';

describe('QualityControlWeldersDialogComponent', () => {
  let component: QualityControlWeldersDialogComponent;
  let fixture: ComponentFixture<QualityControlWeldersDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QualityControlWeldersDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QualityControlWeldersDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
