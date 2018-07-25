import { Component } from '@angular/core';
// Components
import { QualityControlViewComponent } from '../../quality-controls/quality-control-view/quality-control-view.component';
import { RequireQualityControlService } from '../../require-qulitycontrols/shared/require-qc.service';
import { RequireHasMasterService } from '../../require-qulitycontrols/shared/require-has-master.service';
// Servers
@Component({
  selector: 'app-qualitycontrol-view-extend',
  templateUrl: "../../quality-controls/quality-control-view/quality-control-view.component.html",
  styleUrls: ["./qualitycontrol-view-extend.component.scss"]
})
export class QualitycontrolViewExtendComponent extends QualityControlViewComponent {
  /**
   * Constructor data
   * @param serviceRequireQc
   * @param serviceRequireHasMaster
   */
  constructor(
    serviceRequireQc: RequireQualityControlService,
    serviceRequireHasMaster: RequireHasMasterService,
  ) {
    super(serviceRequireQc, serviceRequireHasMaster);
    this.isDialog = true;
  }
}
