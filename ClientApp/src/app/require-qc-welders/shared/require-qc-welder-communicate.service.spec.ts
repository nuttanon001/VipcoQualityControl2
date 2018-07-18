import { TestBed, inject } from '@angular/core/testing';

import { RequireQcWelderCommunicateService } from './require-qc-welder-communicate.service';

describe('RequireQcWelderCommunicateService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RequireQcWelderCommunicateService]
    });
  });

  it('should be created', inject([RequireQcWelderCommunicateService], (service: RequireQcWelderCommunicateService) => {
    expect(service).toBeTruthy();
  }));
});
