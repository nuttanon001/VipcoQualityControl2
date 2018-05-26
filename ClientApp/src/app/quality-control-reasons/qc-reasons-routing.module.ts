import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { QcReasonsCenterComponent } from './qc-reasons-center.component';
import { QcReasonsMasterComponent } from './qc-reasons-master/qc-reasons-master.component';

const routes: Routes = [{
  path: "",
  component: QcReasonsCenterComponent,
  children: [
    {
      path: ":key",
      component: QcReasonsMasterComponent,
    },
    {
      path: "",
      component: QcReasonsMasterComponent,
    }
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class QcReasonsRoutingModule { }
