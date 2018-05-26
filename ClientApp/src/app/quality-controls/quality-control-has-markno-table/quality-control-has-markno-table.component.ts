// Angular Core
import { Component } from "@angular/core";
// Components
import { BaseTableFontData } from "../../shared/base-table-fontdata.component";
// Module
import { RequireQcHasMasterList } from "../../require-qulitycontrols/shared/require-qc-has-master-list.model";
import { Row } from "primeng/primeng";
import { QcReasonsService } from "../../quality-control-reasons/shared/qc-reasons.service";
import { QcReasons } from "../../quality-control-reasons/shared/qc-reasons.model";
// Services


@Component({
  selector: 'app-quality-control-has-markno-table',
  templateUrl: './quality-control-has-markno-table.component.html',
  styleUrls: ['./quality-control-has-markno-table.component.scss']
})

export class QualityControlHasMarknoTableComponent extends BaseTableFontData<RequireQcHasMasterList> {
  /** custom-mat-table ctor */
  constructor(
    private service:QcReasonsService
  ) {
    super();
    this.displayedColumns = ["MarkNoString", "Quantity", "PassQuantity","QualityControlReasonId","edit"];
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
  onBlurPassQuantity(changeValue?:number,rowData?: RequireQcHasMasterList): void {
    if (rowData) {
      if (changeValue > -1) {
        rowData.PassQuantity = changeValue > rowData.Quantity ? rowData.Quantity : changeValue;
      } else {
        rowData.PassQuantity = 0;
      }

      this.returnSelected.emit({
        data: rowData,
        option : 2
      });
    }
  }
}
