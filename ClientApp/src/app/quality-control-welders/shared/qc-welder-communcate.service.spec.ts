import { TestBed, inject } from '@angular/core/testing';

import { QcWelderCommuncateService } from './qc-welder-communcate.service';

describe('QcWelderCommuncateService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [QcWelderCommuncateService]
    });
  });

  it('should be created', inject([QcWelderCommuncateService], (service: QcWelderCommuncateService) => {
    expect(service).toBeTruthy();
  }));
});
