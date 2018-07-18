import { Injectable } from '@angular/core';
import { BaseCommunicateService } from '../../shared/mk2/base-communicate.service';
import { WelderNo } from './welder-no.model';

@Injectable()
export class WelderNoCommunicateService extends BaseCommunicateService<WelderNo> {
  constructor() { super(); }
}
