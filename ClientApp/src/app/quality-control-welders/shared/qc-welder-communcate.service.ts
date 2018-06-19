import { Injectable } from '@angular/core';
import { BaseCommunicateService } from '../../shared/base-communicate.service';
import { QcWelder } from './qc-welder.model';

@Injectable()
export class QcWelderCommuncateService extends BaseCommunicateService<QcWelder> {

  constructor() {
    super();
  }

}
