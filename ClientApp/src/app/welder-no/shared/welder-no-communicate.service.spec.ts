import { TestBed, inject } from '@angular/core/testing';

import { WelderNoCommunicateService } from './welder-no-communicate.service';

describe('WelderNoCommunicateService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [WelderNoCommunicateService]
    });
  });

  it('should be created', inject([WelderNoCommunicateService], (service: WelderNoCommunicateService) => {
    expect(service).toBeTruthy();
  }));
});
