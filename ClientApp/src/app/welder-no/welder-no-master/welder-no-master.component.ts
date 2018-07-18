import { Component, OnInit, ViewContainerRef, ViewChild } from '@angular/core';
import { BaseMasterComponent } from '../../shared/mk2/base-master-component';
import { WelderNo } from '../shared/welder-no.model';
import { WelderNoService } from '../shared/welder-no.service';
import { WelderNoCommunicateService } from '../shared/welder-no-communicate.service';
import { AuthService } from '../../core/auth/auth.service';
import { DialogsService } from '../../dialogs/shared/dialogs.service';
import { WelderNoTableComponent } from '../welder-no-table/welder-no-table.component';

@Component({
  selector: 'app-welder-no-master',
  templateUrl: './welder-no-master.component.html',
  styleUrls: ['./welder-no-master.component.scss']
})
export class WelderNoMasterComponent extends BaseMasterComponent<WelderNo, WelderNoService, WelderNoCommunicateService> {
  constructor(
    service: WelderNoService,
    serviceCom: WelderNoCommunicateService,
    serviceAuth: AuthService,
    serviceDia: DialogsService,
    viewConRef: ViewContainerRef
  ) { super(service, serviceCom, serviceAuth, serviceDia, viewConRef); }

  @ViewChild(WelderNoTableComponent)
  private tableComponent: WelderNoTableComponent;

  onReloadData(): void {
    this.tableComponent.reloadData();
  }
}
