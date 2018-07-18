import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterListV2DialogComponent } from './master-list-v2-dialog.component';

describe('MasterListV2DialogComponent', () => {
  let component: MasterListV2DialogComponent;
  let fixture: ComponentFixture<MasterListV2DialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MasterListV2DialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MasterListV2DialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
