import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
//Modules
import { SharedModule } from "../shared/shared.module";
import { RequireQcRoutingModule } from './require-qc-routing.module';
import { CustomMaterialModule } from '../shared/customer-material/customer-material.module';
//Components
import { RequireQcCenterComponent } from './require-qc-center.component';
import { RequireQcEditComponent } from './require-qc-edit/require-qc-edit.component';
import { RequireQcViewComponent } from './require-qc-view/require-qc-view.component';
import { RequireQcTableComponent } from './require-qc-table/require-qc-table.component';
import { RequireQcReportComponent } from './require-qc-report/require-qc-report.component';
import { RequireQcMasterComponent } from './require-qc-master/require-qc-master.component';
import { RequireQcWaitingComponent } from './require-qc-waiting/require-qc-waiting.component';
import { RequireQcScheduleComponent } from './require-qc-schedule/require-qc-schedule.component';
import { RequireQcFailEditComponent } from './require-qc-fail-edit/require-qc-fail-edit.component';
//Services
import { BranchService } from '../branchs/shared/branch.service';
import { RequireHasMasterService } from './shared/require-has-master.service';
import { MasterListService } from '../master-lists/shared/master-list.service';
import { WorkActivityService } from '../work-activities/shared/work-activity.service';
import { QcReasonsService } from '../quality-control-reasons/shared/qc-reasons.service';
import { EmployeeGroupMisService } from '../employees/shared/employee-group-mis.service';
import { QualityControlService } from '../quality-controls/shared/quality-control.service';
import { WorkGroupQcService } from '../workgroup-qulitycontrols/shared/workgroup-qc.service';
import { InspectionPointService } from '../inspection-points/shared/inspection-point.service';
import { RequireQualityControlService, RequireQualityControlCommunicateService } from './shared/require-qc.service';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    ReactiveFormsModule,
    CustomMaterialModule,
    RequireQcRoutingModule
  ],
  declarations: [
    RequireQcViewComponent,
    RequireQcEditComponent,
    RequireQcTableComponent,
    RequireQcCenterComponent,
    RequireQcReportComponent,
    RequireQcMasterComponent,
    RequireQcWaitingComponent,
    RequireQcFailEditComponent,
    RequireQcScheduleComponent,
    // RequireQcMasterlistTableComponent,
  ],
  providers: [
    BranchService,
    MasterListService,
    WorkGroupQcService,
    WorkActivityService,
    QualityControlService,
    InspectionPointService,
    EmployeeGroupMisService,
    RequireHasMasterService,
    RequireQualityControlService,
    RequireQualityControlCommunicateService,
  ]
})
export class RequireQcModule { }
