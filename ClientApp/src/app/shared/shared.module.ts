import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// Module
import { CustomMaterialModule } from './customer-material/customer-material.module';
import { RequireQcMasterlistTableComponent } from '../require-qulitycontrols/require-qc-masterlist-table/require-qc-masterlist-table.component';
import { QualityControlHasMarknoTableComponent } from '../quality-controls/quality-control-has-markno-table/quality-control-has-markno-table.component';
import { FormsModule } from '@angular/forms';
import { QualityControlHasRequireComponent } from '../quality-controls/quality-control-has-require/quality-control-has-require.component';

// Component
//import { ItemMaintenEmployeeTableComponent } from '../item-maintenances/item-mainten-employee-table/item-mainten-employee-table.component';
//import { ItemMaintenHasRequireComponent } from '../item-maintenances/shared/item-mainten-has-require.component';
//import { ItemMaintenRequisitionTableComponent } from '../item-maintenances/item-mainten-requisition-table/item-mainten-requisition-table.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    CustomMaterialModule,
  ],
  declarations: [
    RequireQcMasterlistTableComponent,
    QualityControlHasRequireComponent,
    QualityControlHasMarknoTableComponent,
    //ItemMaintenEmployeeTableComponent,
    //ItemMaintenHasRequireComponent,
    //ItemMaintenRequisitionTableComponent,
  ],
  exports: [
    RequireQcMasterlistTableComponent,
    QualityControlHasRequireComponent,
    QualityControlHasMarknoTableComponent,
    //ItemMaintenEmployeeTableComponent,
    //ItemMaintenHasRequireComponent,
    //ItemMaintenRequisitionTableComponent,
  ],
  entryComponents: [
    RequireQcMasterlistTableComponent,
    QualityControlHasRequireComponent,
    QualityControlHasMarknoTableComponent,
    //ItemMaintenEmployeeTableComponent,
    //ItemMaintenHasRequireComponent,
    //ItemMaintenRequisitionTableComponent,
  ]
})
export class SharedModule { }
