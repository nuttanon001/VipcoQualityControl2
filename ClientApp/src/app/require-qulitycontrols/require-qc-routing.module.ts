import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RequireQcCenterComponent } from './require-qc-center.component';
import { RequireQcMasterComponent } from './require-qc-master/require-qc-master.component';
import { RequireQcWaitingComponent } from './require-qc-waiting/require-qc-waiting.component';
import { RequireQcScheduleComponent } from './require-qc-schedule/require-qc-schedule.component';
import { AuthGuard } from '../core/auth/auth-guard.service';

const routes: Routes = [
  {
    path: "",
    component: RequireQcCenterComponent,
    children: [
      {
        path: "require-waiting/:condition",
        component: RequireQcWaitingComponent,
        canActivate: [AuthGuard],
      },
      {
        path: "require-waiting",
        component: RequireQcWaitingComponent,
        canActivate: [AuthGuard],
      },
      {
        path: "require-schedule",
        component: RequireQcScheduleComponent,
      },
      {
        path: "link-email/:condition",
        component: RequireQcScheduleComponent,
      },
      {
        path: "fail-require-schedule/:condition",
        component: RequireQcMasterComponent,
        canActivate: [AuthGuard],
      },
      {
        path: ":key",
        component: RequireQcMasterComponent,
        canActivate: [AuthGuard],
      },
      {
        path: "",
        component: RequireQcMasterComponent,
        canActivate: [AuthGuard],
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RequireQcRoutingModule { }
