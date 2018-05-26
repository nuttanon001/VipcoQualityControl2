import { Component, OnInit } from '@angular/core';
import { QualityControlReportComponent } from '../../quality-controls/quality-control-report/quality-control-report.component';
import { QualityControlService } from '../../quality-controls/shared/quality-control.service';

@Component({
  selector: 'app-require-qc-report',
  templateUrl: '../../quality-controls/quality-control-report/quality-control-report.component.html',
  styleUrls: ['../../quality-controls/quality-control-report/quality-control-report.component.scss']
})
export class RequireQcReportComponent extends QualityControlReportComponent {
  constructor(
    service: QualityControlService
  ) {
    super(service);
  }
}
