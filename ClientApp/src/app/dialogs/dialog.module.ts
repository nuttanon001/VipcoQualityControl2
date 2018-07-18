// angular core
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
// 3rd party
import "rxjs/Rx";
import "hammerjs";
// services
import { DialogsService } from "./shared/dialogs.service";
// modules
import { CustomMaterialModule } from "../shared/customer-material/customer-material.module";
import { SharedModule } from "../shared/shared.module";
// components
import {
  ConfirmDialog, ContextDialog,
  ErrorDialog, EmployeeDialogComponent,
  GroupmisDialogComponent, 
  ProjectDialogComponent,
  EmployeeTableComponent,
  GroupmisTableComponent, ProjectTableComponent,
} from "./dialog.index";
import { ProjectDetailTableComponent } from './project-dialog/project-detail-table/project-detail-table.component';
import { MasterListDialogComponent } from './master-list-dialog/master-list-dialog.component';
import { LocationDialogComponent } from './location-dialog/location-dialog.component';
import { LocationTableComponent } from './location-dialog/location-table/location-table.component';
import { LocationEditComponent } from './location-dialog/location-edit/location-edit.component';
import { RequireQcDialogComponent } from './require-qc-dialog/require-qc-dialog.component';
import { RequireQcViewExtendComponent } from './require-qc-dialog/require-qc-view-extend.component';
import { RequireQcChangeDialogComponent } from './require-qc-dialog/require-qc-change-dialog.component';
import { QualitycontrolDialogComponent } from './qualitycontrol-dialog/qualitycontrol-dialog.component';
import { QualitycontrolViewExtendComponent } from "./qualitycontrol-dialog/qualitycontrol-view-extend.component";
import { QualityControlWeldersDialogComponent } from './quality-control-welders-dialog/quality-control-welders-dialog.component';
import { MasterListV2DialogComponent } from './master-list-v2-dialog/master-list-v2-dialog.component';
import { MasterListInfoV2DialogComponent } from "./master-list-v2-dialog/master-list-info-v2-dialog/master-list-info-v2-dialog.component";
import { WelderTeamDialogComponent } from './welder-team-dialog/welder-team-dialog.component';
import { WelderTeamInfoDialogComponent } from './welder-team-dialog/welder-team-info-dialog/welder-team-info-dialog.component';
import { WelderNoDialogComponent } from './welder-no-dialog/welder-no-dialog.component';
import { WelderNoTableDailogComponent } from './welder-no-dialog/welder-no-table-dailog/welder-no-table-dailog.component';
import { RequireQcWelderDialogComponent } from './require-qc-welder-dialog/require-qc-welder-dialog.component';
import { RequireQcWelderInfoDialogComponent } from './require-qc-welder-dialog/require-qc-welder-info-dialog/require-qc-welder-info-dialog.component';

@NgModule({
  imports: [
    // angular
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    // customer Module
    SharedModule,
    CustomMaterialModule,
  ],
  declarations: [
    ErrorDialog,
    ConfirmDialog,
    ContextDialog,
    EmployeeDialogComponent,
    EmployeeTableComponent,
    ProjectDialogComponent,
    ProjectTableComponent,
    //WorkgroupDialogComponent,
    GroupmisDialogComponent,
    GroupmisTableComponent,
    MasterListDialogComponent,
    ProjectDetailTableComponent,
    LocationEditComponent,
    LocationTableComponent,
    LocationDialogComponent,
    RequireQcDialogComponent,
    RequireQcViewExtendComponent,
    RequireQcChangeDialogComponent,
    QualitycontrolDialogComponent,
    QualitycontrolViewExtendComponent,
    QualityControlWeldersDialogComponent,
    MasterListV2DialogComponent,
    MasterListInfoV2DialogComponent,
    WelderTeamDialogComponent,
    WelderTeamInfoDialogComponent,
    WelderNoDialogComponent,
    WelderNoTableDailogComponent,
    RequireQcWelderDialogComponent,
    RequireQcWelderInfoDialogComponent,
  ],
  providers: [
    DialogsService,
  ],
  // a list of components that are not referenced in a reachable component template.
  // doc url is :https://angular.io/guide/ngmodule-faq
  entryComponents: [
    ErrorDialog,
    ConfirmDialog,
    ContextDialog,
    GroupmisTableComponent,
    ProjectDialogComponent,
    EmployeeDialogComponent,
    GroupmisDialogComponent,
    MasterListDialogComponent,
    ProjectDetailTableComponent,
    LocationEditComponent,
    LocationTableComponent,
    LocationDialogComponent,
    RequireQcDialogComponent,
    RequireQcViewExtendComponent,
    RequireQcChangeDialogComponent,
    QualitycontrolDialogComponent,
    QualitycontrolViewExtendComponent,
    QualityControlWeldersDialogComponent,
    MasterListV2DialogComponent,
    MasterListInfoV2DialogComponent,
    WelderTeamDialogComponent,
    WelderTeamInfoDialogComponent,
    WelderNoDialogComponent,
    WelderNoTableDailogComponent,
    RequireQcWelderDialogComponent,
    RequireQcWelderInfoDialogComponent,
  ],
})
export class DialogsModule { }
