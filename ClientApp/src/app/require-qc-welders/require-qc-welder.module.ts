import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
// Models
import { RequireQcWelderRoutingModule } from './require-qc-welder-routing.module';
import { CustomMaterialModule } from '../shared/customer-material/customer-material.module';
// Services
import { RequireQcWelderService } from './shared/require-qc-welder.service';
import { RequireQcWelderCommunicateService } from './shared/require-qc-welder-communicate.service';
// Component
import { RequireQcWelderCenterComponent } from './require-qc-welder-center.component';
import { RequireQcWelderInfoComponent } from './require-qc-welder-info/require-qc-welder-info.component';
import { RequireQcWelderTableComponent } from './require-qc-welder-table/require-qc-welder-table.component';
import { RequireQcWelderMasterComponent } from './require-qc-welder-master/require-qc-welder-master.component';
import { RequireQcWelderSubTableComponent } from './require-qc-welder-sub-table/require-qc-welder-sub-table.component';
import { BranchService } from '../branchs/shared/branch.service';
import { MasterListService } from '../master-lists/shared/master-list.service';
import { WorkGroupQcService } from '../workgroup-qulitycontrols/shared/workgroup-qc.service';
import { WorkActivityService } from '../work-activities/shared/work-activity.service';
import { QualityControlService } from '../quality-controls/shared/quality-control.service';
import { InspectionPointService } from '../inspection-points/shared/inspection-point.service';
import { EmployeeGroupMisService } from '../employees/shared/employee-group-mis.service';
import { RequireHasMasterService } from '../require-qulitycontrols/shared/require-has-master.service';
import { RequireQualityControlService } from '../require-qulitycontrols/shared/require-qc.service';
import { RequireMoreWorkactivityService } from '../require-qulitycontrols/shared/require-more-workactivity.service';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    ReactiveFormsModule,
    CustomMaterialModule,
    RequireQcWelderRoutingModule
  ],
  declarations: [
    RequireQcWelderCenterComponent,
    RequireQcWelderMasterComponent,
    RequireQcWelderInfoComponent,
    RequireQcWelderTableComponent,
    RequireQcWelderSubTableComponent
  ],
  providers: [
    BranchService,
    WorkGroupQcService,
    WorkActivityService,
    QualityControlService,
    InspectionPointService,
    RequireQcWelderService,
    EmployeeGroupMisService,
    RequireHasMasterService,
    RequireQualityControlService,
    RequireQcWelderCommunicateService,
    RequireMoreWorkactivityService,
  ]
})
export class RequireQcWelderModule { }
