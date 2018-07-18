import { Injectable } from '@angular/core';
import { RequireQcWelder } from './require-qc-welder.model';
import { BaseCommunicateService } from '../../shared/mk2/base-communicate.service';
import { RequireQc } from '../../require-qulitycontrols/shared/require-qc.model';

@Injectable()
export class RequireQcWelderCommunicateService extends BaseCommunicateService<RequireQc> {
  constructor() { super() }
}
