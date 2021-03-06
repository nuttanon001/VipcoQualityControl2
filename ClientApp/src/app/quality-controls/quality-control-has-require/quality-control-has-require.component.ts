//AngularCore
import { Component, OnInit } from '@angular/core';
// Components
import { RequireQcViewComponent } from '../../require-qulitycontrols/require-qc-view/require-qc-view.component';
// Services
import { MasterListService } from '../../master-lists/shared/master-list.service';
import { RequireQualityControlService } from '../../require-qulitycontrols/shared/require-qc.service';
import { RequireMoreWorkactivityService } from '../../require-qulitycontrols/shared/require-more-workactivity.service';

@Component({
  selector: 'app-quality-control-has-require',
  templateUrl: "../../require-qulitycontrols/require-qc-view/require-qc-view.component.html",
  styleUrls: ["../../require-qulitycontrols/require-qc-view/require-qc-view.component.scss"],
  providers: [ RequireQualityControlService , MasterListService ]
})

export class QualityControlHasRequireComponent extends RequireQcViewComponent {
  constructor(
    service: RequireQualityControlService,
    serviceMasterList: MasterListService,
    serviceMoreWorkActivities: RequireMoreWorkactivityService,
  ) {
    super(service, serviceMasterList, serviceMoreWorkActivities);
    this.noMasterList = true;
  }
}
