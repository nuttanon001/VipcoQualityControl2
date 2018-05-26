import { Component, ViewContainerRef, ViewChild } from "@angular/core";
// components
import { BaseMasterComponent } from "../../shared/base-master-component";
// models
import { QcReasons } from "../shared/qc-reasons.model";
// services
import { AuthService } from "../../core/auth/auth.service";
import { DialogsService } from "../../dialogs/shared/dialogs.service";
import { QcReasonsService, QcReasonsCommunicateService } from "../shared/qc-reasons.service";
// timezone
import * as moment from "moment-timezone";
import { QcReasonsTableComponent } from "../qc-reasons-table/qc-reasons-table.component";

@Component({
  selector: 'app-qc-reasons-master',
  templateUrl: './qc-reasons-master.component.html',
  styleUrls: ['./qc-reasons-master.component.scss']
})
export class QcReasonsMasterComponent extends BaseMasterComponent<QcReasons, QcReasonsService> {

  /** require-painting-master ctor */
  constructor(
    service: QcReasonsService,
    serviceCom: QcReasonsCommunicateService,
    authService: AuthService,
    dialogsService: DialogsService,
    viewContainerRef: ViewContainerRef,
  ) {
    super(
      service,
      serviceCom,
      authService,
      dialogsService,
      viewContainerRef
    );
  }

  //Parameter

  @ViewChild(QcReasonsTableComponent)
  private tableComponent: QcReasonsTableComponent;

  // on change time zone befor update to webapi
  changeTimezone(value: QcReasons): QcReasons {
    let zone: string = "Asia/Bangkok";
    if (value !== null) {
      if (value.CreateDate !== null) {
        value.CreateDate = moment.tz(value.CreateDate, zone).toDate();
      }
      if (value.ModifyDate !== null) {
        value.ModifyDate = moment.tz(value.ModifyDate, zone).toDate();
      }
    }
    return value;
  }

  // onReload
  onReloadData(): void {
    this.tableComponent.reloadData();
  }
}
