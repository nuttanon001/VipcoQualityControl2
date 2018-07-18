import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RequireQcWelderCenterComponent } from './require-qc-welder-center.component';
import { RequireQcWelderMasterComponent } from './require-qc-welder-master/require-qc-welder-master.component';

const routes: Routes = [{
  path: "",
  component: RequireQcWelderCenterComponent,
  children: [
    {
      path: "forfail/:condition",
      component: RequireQcWelderMasterComponent,
    },
    {
      path: ":key",
      component: RequireQcWelderMasterComponent,
    },
    {
      path: "",
      component: RequireQcWelderMasterComponent,
    }
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RequireQcWelderRoutingModule { }
