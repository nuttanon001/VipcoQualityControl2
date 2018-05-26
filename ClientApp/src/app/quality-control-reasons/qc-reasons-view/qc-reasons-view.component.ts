// angular
import { Component, Output, EventEmitter, Input } from "@angular/core";
// models
import { QcReasons } from "../shared/qc-reasons.model";
// components
import { BaseViewComponent } from "../../shared/base-view-component";

@Component({
  selector: 'app-qc-reasons-view',
  templateUrl: '../../shared/base-view-component.html',
  styleUrls: ['./qc-reasons-view.component.scss']
})
export class QcReasonsViewComponent extends BaseViewComponent<QcReasons> {
  constructor(
  ) {
    super();
    this.titelLabel = "Quality-Control information view";
  }

  // load more data
  onLoadMoreData(qcReasons: QcReasons) {
  }
}
