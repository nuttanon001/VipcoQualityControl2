import { Injectable } from '@angular/core';
import { BaseCommunicateService } from '../../shared/mk2/base-communicate.service';
import { WelderProjectHasEmp } from './welder-project-has-emp.model';

@Injectable()
export class WelderProjectCommunicateService extends BaseCommunicateService<WelderProjectHasEmp> {
  constructor() { super(); }
}
