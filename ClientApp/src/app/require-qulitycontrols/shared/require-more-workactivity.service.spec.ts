import { TestBed, inject } from '@angular/core/testing';

import { RequireMoreWorkactivityService } from './require-more-workactivity.service';

describe('RequireMoreWorkactivityService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RequireMoreWorkactivityService]
    });
  });

  it('should be created', inject([RequireMoreWorkactivityService], (service: RequireMoreWorkactivityService) => {
    expect(service).toBeTruthy();
  }));
});
