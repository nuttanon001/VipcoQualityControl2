import { Component, OnInit } from '@angular/core';
import { CustomMatTableComponent } from '../../shared/custom-mat-table/custom-mat-table.component';
import { BaseTableComponent } from '../../shared/base-table.component';
import { QcWelder } from '../shared/qc-welder.model';
import { QcWelderService } from '../shared/qc-welder.service';
import { AuthService } from '../../core/auth/auth.service';

@Component({
  selector: 'app-qc-welder-table',
  templateUrl: './qc-welder-table.component.html',
  styleUrls: ['./qc-welder-table.component.scss']
})
export class QcWelderTableComponent extends BaseTableComponent<QcWelder,QcWelderService> {
  constructor(
    service: QcWelderService,
    serviveAuth: AuthService,
  ) {
    super(service, serviveAuth);
    this.displayedColumns = ["select", "RequireQualityControlNo","ProjectCodeMasterString", "WeldingDate", "MarkNo", "MarkNoPreview", "WelderNo", "ProcessWeld", "JointNo", "Thickness","TestLength"];
  }
}
