import { TestBed, inject } from '@angular/core/testing';

import { WelderProjectCommunicateService } from './welder-project-communicate.service';

describe('WelderProjectCommunicateService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [WelderProjectCommunicateService]
    });
  });

  it('should be created', inject([WelderProjectCommunicateService], (service: WelderProjectCommunicateService) => {
    expect(service).toBeTruthy();
  }));
});
