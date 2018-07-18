import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterListInfoV2DialogComponent } from './master-list-info-v2-dialog.component';

describe('MasterListInfoV2DialogComponent', () => {
  let component: MasterListInfoV2DialogComponent;
  let fixture: ComponentFixture<MasterListInfoV2DialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MasterListInfoV2DialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MasterListInfoV2DialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
