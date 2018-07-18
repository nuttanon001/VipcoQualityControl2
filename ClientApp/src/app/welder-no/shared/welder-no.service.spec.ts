import { TestBed, inject } from '@angular/core/testing';

import { WelderNoService } from './welder-no.service';

describe('WelderNoService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [WelderNoService]
    });
  });

  it('should be created', inject([WelderNoService], (service: WelderNoService) => {
    expect(service).toBeTruthy();
  }));
});
