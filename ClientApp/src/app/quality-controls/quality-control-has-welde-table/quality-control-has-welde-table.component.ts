import { Component, OnInit } from '@angular/core';
import { QcReasonsService } from '../../quality-control-reasons/shared/qc-reasons.service';
import { QcWelder } from '../../quality-control-welders/shared/qc-welder.model';
import { BaseTableFontData } from '../../shared/base-table-fontdata.component';
import { QcReasons } from '../../quality-control-reasons/shared/qc-reasons.model';

@Component({
  selector: 'app-quality-control-has-welde-table',
  templateUrl: './quality-control-has-welde-table.component.html',
  styleUrls: ['./quality-control-has-welde-table.component.scss']
})
export class QualityControlHasWeldeTableComponent extends BaseTableFontData<QcWelder> {
  /** custom-mat-table ctor */
  constructor(
    private service: QcReasonsService
  ) {
    super();
    this.displayedColumns = ["WeldingDate", "MarkNo",
      "MarkNoPreview", "WelderNo", "ProcessWeld", "JointNo",
      "Thickness", "TestLength", "Reject", "QualityControlReasonId","edit"];
  }
  // Parameter
  qcReasons: Array<QcReasons>;

  ngOnInit(): void {
    this.service.getAll()
      .subscribe(dbData => {
        if (dbData) {
          this.qcReasons = dbData.slice();
        }
      });
    super.ngOnInit();
  }
  // on blur
  onBlurPassQuantity(changeValue?: number, rowData?: QcWelder): void {
    if (rowData) {
      if (changeValue > -1) {
        rowData.Reject = changeValue > rowData.TestLength ? rowData.TestLength : changeValue;
      } else {
        rowData.Reject = 0;
      }

      this.returnSelected.emit({
        data: rowData,
        option: 2
      });
    }
  }
}
