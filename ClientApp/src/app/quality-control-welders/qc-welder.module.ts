import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { QcWelderRoutingModule } from './qc-welder-routing.module';
import { QcWelderService } from './shared/qc-welder.service';
import { QcWelderCenterComponent } from './qc-welder-center.component';
import { QcWelderTableComponent } from './qc-welder-table/qc-welder-table.component';
import { QcWelderMasterComponent } from './qc-welder-master/qc-welder-master.component';
import { QcWelderCommuncateService } from './shared/qc-welder-communcate.service';
import { ReactiveFormsModule } from '@angular/forms';
import { CustomMaterialModule } from '../shared/customer-material/customer-material.module';
import { RequireQualityControlService } from '../require-qulitycontrols/shared/require-qc.service';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CustomMaterialModule,
    QcWelderRoutingModule
  ],
  declarations: [
    QcWelderCenterComponent,
    QcWelderTableComponent,
    QcWelderMasterComponent],
  providers: [
    QcWelderService,
    QcWelderCommuncateService,
    RequireQualityControlService
  ]
})
export class QcWelderModule { }
