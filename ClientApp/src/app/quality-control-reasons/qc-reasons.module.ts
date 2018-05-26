//Angular Core
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
//Modules
import { CustomMaterialModule } from '../shared/customer-material/customer-material.module';
import { QcReasonsRoutingModule } from './qc-reasons-routing.module';
//Components
import { QcReasonsCenterComponent } from './qc-reasons-center.component';
import { QcReasonsTableComponent } from './qc-reasons-table/qc-reasons-table.component';
import { QcReasonsMasterComponent } from './qc-reasons-master/qc-reasons-master.component';
import { QcReasonsViewComponent } from './qc-reasons-view/qc-reasons-view.component';
import { QcReasonsEditComponent } from './qc-reasons-edit/qc-reasons-edit.component';
import { QcReasonsService, QcReasonsCommunicateService } from './shared/qc-reasons.service';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CustomMaterialModule,
    QcReasonsRoutingModule
  ],
  declarations: [
    QcReasonsCenterComponent,
    QcReasonsTableComponent,
    QcReasonsMasterComponent,
    QcReasonsViewComponent,
    QcReasonsEditComponent,
  ],
  providers: [
    QcReasonsService,
    QcReasonsCommunicateService,
  ]
})
export class QcReasonsModule { }
