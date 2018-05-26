//AngularCore
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
//Services
import { LocationQcService, LocationQcCommunicateService } from './shared/location-qc.service';
//Modules
import { CustomMaterialModule } from '../shared/customer-material/customer-material.module';
import { LocationQcRoutingModule } from './location-qc-routing.module';
//Components
import { LocationQcCenterComponent } from './location-qc-center.component';
import { LocationQcMasterComponent } from './location-qc-master/location-qc-master.component';
import { LocationQcTableComponent } from './location-qc-table/location-qc-table.component';
import { LocationQcEditComponent } from './location-qc-edit/location-qc-edit.component';
import { LocationQcHasWorkgroupComponent } from './location-qc-has-workgroup/location-qc-has-workgroup.component';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CustomMaterialModule,
    LocationQcRoutingModule,
  ],
  declarations: [
    LocationQcCenterComponent,
    LocationQcMasterComponent,
    LocationQcTableComponent,
    LocationQcEditComponent,
    LocationQcHasWorkgroupComponent
  ],
  providers: [
    LocationQcService,
    LocationQcCommunicateService
  ]
})
export class LocationQcModule { }
