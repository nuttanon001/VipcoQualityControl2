import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WelderNoTableDailogComponent } from './welder-no-table-dailog.component';

describe('WelderNoTableDailogComponent', () => {
  let component: WelderNoTableDailogComponent;
  let fixture: ComponentFixture<WelderNoTableDailogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WelderNoTableDailogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WelderNoTableDailogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
