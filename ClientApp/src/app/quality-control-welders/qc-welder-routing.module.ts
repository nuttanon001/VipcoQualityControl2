import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { QcWelderCenterComponent } from './qc-welder-center.component';
import { QcWelderMasterComponent } from './qc-welder-master/qc-welder-master.component';

const routes: Routes = [{
  path: "",
  component: QcWelderCenterComponent,
  children: [
    {
      path: ":condition",
      component: QcWelderMasterComponent,
    },
    {
      path: "",
      component: QcWelderMasterComponent,
    }
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class QcWelderRoutingModule { }
