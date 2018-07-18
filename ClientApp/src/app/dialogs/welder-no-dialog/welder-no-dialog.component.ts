import { Component, OnInit, Inject } from '@angular/core';
import { BaseDialogComponent } from '../../shared/mk2/base-dialog.component';
import { WelderNo } from '../../welder-no/shared/welder-no.model';
import { WelderNoService } from '../../welder-no/shared/welder-no.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-welder-no-dialog',
  templateUrl: './welder-no-dialog.component.html',
  styleUrls: ['./welder-no-dialog.component.scss'],
  providers: [WelderNoService]
})
export class WelderNoDialogComponent extends BaseDialogComponent<WelderNo,WelderNoService> {

  constructor(
    service: WelderNoService,
    dialogRef: MatDialogRef<WelderNoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public mode: number
  ) {
    super(service, dialogRef);
  }
  // on init
  onInit(): void {
    this.fastSelectd = this.mode === 0 ? true : false;
  }
}
