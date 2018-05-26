import { TestBed, inject } from '@angular/core/testing';

import { QcReasonsService } from './qc-reasons.service';

describe('QcReasonsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [QcReasonsService]
    });
  });

  it('should be created', inject([QcReasonsService], (service: QcReasonsService) => {
    expect(service).toBeTruthy();
  }));
});
