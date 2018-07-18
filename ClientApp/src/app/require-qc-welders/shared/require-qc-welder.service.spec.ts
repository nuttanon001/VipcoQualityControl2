import { TestBed, inject } from '@angular/core/testing';

import { RequireQcWelderService } from './require-qc-welder.service';

describe('RequireQcWelderService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RequireQcWelderService]
    });
  });

  it('should be created', inject([RequireQcWelderService], (service: RequireQcWelderService) => {
    expect(service).toBeTruthy();
  }));
});
