import { TestBed, inject } from '@angular/core/testing';

import { QcWelderService } from './qc-welder.service';

describe('QcWelderService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [QcWelderService]
    });
  });

  it('should be created', inject([QcWelderService], (service: QcWelderService) => {
    expect(service).toBeTruthy();
  }));
});
