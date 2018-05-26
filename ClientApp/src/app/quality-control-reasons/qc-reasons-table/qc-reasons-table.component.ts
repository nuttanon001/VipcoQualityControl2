// Angular Core
import { Component } from "@angular/core";
// Components
import { CustomMatTableComponent } from "../../shared/custom-mat-table/custom-mat-table.component";
// Models
import { QcReasons } from "../shared/qc-reasons.model";
// Services
import { AuthService } from "../../core/auth/auth.service";
import { QcReasonsService } from "../shared/qc-reasons.service";

@Component({
  selector: 'app-qc-reasons-table',
  templateUrl: './qc-reasons-table.component.html',
  styleUrls: ['./qc-reasons-table.component.scss']
})
export class QcReasonsTableComponent extends CustomMatTableComponent<QcReasons,QcReasonsService>{
  // Constructor
  constructor(
    service: QcReasonsService,
    authService: AuthService,
  ) {
    super(service, authService);
    this.displayedColumns = ["select", "Name", "Description"];
  }
}
