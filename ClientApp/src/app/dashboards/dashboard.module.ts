import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardService } from './shared/dashboard.service';
import { CustomMaterialModule } from '../shared/customer-material/customer-material.module';
import { ChartTop3Component } from './chart-top3/chart-top3.component';
import { ChartTotalComponent } from './chart-total/chart-total.component';
import { ChartPassComponent } from './chart-pass/chart-pass.component';
import { ChartFailComponent } from './chart-fail/chart-fail.component';

@NgModule({
  imports: [
    CommonModule,
    CustomMaterialModule,
  ],
  declarations: [
    ChartTop3Component,
    ChartTotalComponent,
    ChartPassComponent,
    ChartFailComponent
  ],
  exports: [
    ChartTop3Component,
    ChartTotalComponent,
    ChartPassComponent,
    ChartFailComponent
  ],
  providers: [DashboardService]
})
export class DashboardModule { }
