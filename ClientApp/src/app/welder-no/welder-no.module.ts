import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { WelderNoRoutingModule } from './welder-no-routing.module';
import { WelderNoService } from './shared/welder-no.service';
import { WelderNoCommunicateService } from './shared/welder-no-communicate.service';
import { WelderNoCenterComponent } from './welder-no-center.component';
import { WelderNoTableComponent } from './welder-no-table/welder-no-table.component';
import { WelderNoMasterComponent } from './welder-no-master/welder-no-master.component';
import { WelderNoInfoComponent } from './welder-no-info/welder-no-info.component';
import { ReactiveFormsModule } from '@angular/forms';
import { CustomMaterialModule } from '../shared/customer-material/customer-material.module';
import { WelderProjectMasterComponent } from './welder-project-master/welder-project-master.component';
import { WelderProjectInfoComponent } from './welder-project-info/welder-project-info.component';
import { WelderProjectTableComponent } from './welder-project-table/welder-project-table.component';
import { WelderProjectTableSubComponent } from './welder-project-table-sub/welder-project-table-sub.component';
import { WelderProjectService } from './shared/welder-project.service';
import { WelderProjectCommunicateService } from './shared/welder-project-communicate.service';
import { ProjectMasterService } from '../projects/shared/project-master.service';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CustomMaterialModule,
    WelderNoRoutingModule
  ],
  declarations: [
    WelderNoCenterComponent,
    WelderNoTableComponent,
    WelderNoMasterComponent,
    WelderNoInfoComponent,
    WelderProjectMasterComponent,
    WelderProjectInfoComponent,
    WelderProjectTableComponent,
    WelderProjectTableSubComponent
  ],
  providers: [
    ProjectMasterService,
    WelderNoService,
    WelderNoCommunicateService,
    WelderProjectService,
    WelderProjectCommunicateService
  ]
})
export class WelderNoModule { }
