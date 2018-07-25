import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { RequireQcWelderService } from '../shared/require-qc-welder.service';

@Component({
  selector: 'app-require-qc-welder-report',
  templateUrl: './require-qc-welder-report.component.html',
  styleUrls: ['./require-qc-welder-report.component.scss']
})
export class RequireQcWelderReportComponent implements OnInit {

  constructor(
    private service: RequireQcWelderService
  ) { }

  // Parameter
  @Input() RequireQcWelderId: number;
  @Output() Back = new EventEmitter<boolean>();
  ReportData: any;
  // called by Angular after aint-task-detail-paint-report component initialized */
  ngOnInit(): void {
    if (this.RequireQcWelderId) {
      this.service.getWelderReport(this.RequireQcWelderId)
        .subscribe(dbReport => {
          this.ReportData = dbReport;
        });
    } else {
      this.onBackToMaster();
    }
  }

  // on Print OverTimeMaster
  onPrint(): void {
    window.print();
  }

  // on Print OverTimeMaster
  onPrintPdf(): void {
  }

  // on Back
  onBackToMaster(): void {
    this.Back.emit(true);
  }
}
