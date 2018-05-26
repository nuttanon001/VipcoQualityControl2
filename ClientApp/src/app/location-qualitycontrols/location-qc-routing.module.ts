import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LocationQcCenterComponent } from './location-qc-center.component';
import { LocationQcMasterComponent } from './location-qc-master/location-qc-master.component';

const routes: Routes = [
  {
    path: "",
    component: LocationQcCenterComponent,
    children: [
      {
        path: ":key",
        component: LocationQcMasterComponent,
      },
      {
        path: "",
        component: LocationQcMasterComponent,
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LocationQcRoutingModule { }
