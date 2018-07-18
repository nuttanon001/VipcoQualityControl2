import { TestBed, inject } from '@angular/core/testing';

import { WelderProjectService } from './welder-project.service';

describe('WelderProjectService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [WelderProjectService]
    });
  });

  it('should be created', inject([WelderProjectService], (service: WelderProjectService) => {
    expect(service).toBeTruthy();
  }));
});
