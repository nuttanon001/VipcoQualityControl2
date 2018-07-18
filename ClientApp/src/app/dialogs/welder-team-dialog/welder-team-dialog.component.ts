import { Component, OnInit, Inject } from '@angular/core';
import { BaseDialogEntryComponent } from '../../shared/base-dialog-entry.component';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { BaseDialogComponent } from '../../shared/base-dialog.component';
import { WelderNo } from '../../welder-no/shared/welder-no.model';
import { WelderNoService } from '../../welder-no/shared/welder-no.service';

@Component({
  selector: 'app-welder-team-dialog',
  templateUrl: './welder-team-dialog.component.html',
  styleUrls: ['./welder-team-dialog.component.scss'],
  providers:[WelderNoService]
  
})
export class WelderTeamDialogComponent extends BaseDialogComponent<WelderNo, WelderNoService> {

  constructor(
    public service: WelderNoService,
    public dialogRef: MatDialogRef<WelderTeamDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public InfoValue?: WelderNo
  ) {
    super(service, dialogRef);
  }
  NeedWelder: boolean;
  // on init
  onInit(): void {
    this.fastSelectd = this.InfoValue.WelderNoId === -99;
  }
  // on complate or cancel entity
  onComplateOrCancelEntity(infoValue?: { data: WelderNo | undefined, force: boolean }): void {
    if (infoValue) {
      if (infoValue.data) {
        this.getValue = infoValue.data;
        if (infoValue.force) {
          this.onSelectedClick();
        }
      }
    }
  }
}
