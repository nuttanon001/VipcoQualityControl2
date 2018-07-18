import { Component, OnInit } from '@angular/core';
import { RequireQcWelder } from '../shared/require-qc-welder.model';
import { RequireQcWelderService } from '../shared/require-qc-welder.service';
import { AuthService } from '../../core/auth/auth.service';
import { RequireQc } from '../../require-qulitycontrols/shared/require-qc.model';
import { RequireQualityControlService } from '../../require-qulitycontrols/shared/require-qc.service';
import { BaseTableComponent } from '../../shared/mk2/base-table.component';

@Component({
  selector: 'app-require-qc-welder-table',
  templateUrl: './require-qc-welder-table.component.html',
  styleUrls: ['./require-qc-welder-table.component.scss']
})
  /*
   * RequireQcWelderService GetScorll return list<RequrieQc>
   */
export class RequireQcWelderTableComponent extends BaseTableComponent<RequireQc,RequireQcWelderService> {
  constructor(service: RequireQcWelderService, serviceAuth: AuthService) {
    super(service, serviceAuth);
    this.displayedColumns = ["select", "RequireQualityNo", "InspectionPointString", "RequireDate"];
  }
}
