import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WelderNoCenterComponent } from './welder-no-center.component';
import { WelderNoMasterComponent } from './welder-no-master/welder-no-master.component';
import { WelderProjectMasterComponent } from './welder-project-master/welder-project-master.component';

const routes: Routes = [{
  path: "",
  component: WelderNoCenterComponent,
  children: [
    {
      path: "welder-project:key",
      component: WelderProjectMasterComponent,
    },
    {
      path: "welder-project",
      component: WelderProjectMasterComponent,
    },
    {
      path: ":key",
      component: WelderNoMasterComponent,
    },
    {
      path: "",
      component: WelderNoMasterComponent,
    }
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WelderNoRoutingModule { }
