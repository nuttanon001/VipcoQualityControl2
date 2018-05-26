import { Component, OnInit, Input, EventEmitter, Output } from "@angular/core";
import { QualityControlService } from "../shared/quality-control.service";
import * as jsPDF from 'jspdf'

@Component({
  selector: 'app-quality-control-report',
  templateUrl: './quality-control-report.component.html',
  styleUrls: ['./quality-control-report.component.scss']
})
export class QualityControlReportComponent implements OnInit {

  constructor(
    private service:QualityControlService
  ) { }

  // Parameter
  @Input() QualityControlId: number;
  @Output() Back = new EventEmitter<boolean>();
  ReportData: any;
  // called by Angular after aint-task-detail-paint-report component initialized */
  ngOnInit(): void {
    if (this.QualityControlId) {
      this.service.getQualityControlReport(this.QualityControlId)
        .subscribe(dbReport => {
          this.ReportData = dbReport;
        });
    } else {
      this.onBackToMaster();
    }
  }

  // on Print OverTimeMaster
  onPrintOverTimeMaster(): void {
    window.print();
  }

  // on Print OverTimeMaster
  onPrintPdf(): void {
    let doc: jsPDF = new jsPDF();

    doc.add
    doc.text('Hello world!', 1, 1)
    doc.save('two-by-four.pdf');
  }

  // on Back
  onBackToMaster(): void {
    this.Back.emit(true);
  }

}
