import { Component, OnInit, Inject } from '@angular/core';
import { BaseDialogEntryComponent } from '../../shared/base-dialog-entry.component';
import { MasterList } from '../../master-lists/shared/master-list.model';
import { MasterListService } from '../../master-lists/shared/master-list.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { BaseDialogComponent } from '../../shared/base-dialog.component';
import { RequireQcWelder } from '../../require-qc-welders/shared/require-qc-welder.model';
import { RequireQcWelderService } from '../../require-qc-welders/shared/require-qc-welder.service';
import { DialogsService } from '../shared/dialogs.service';

@Component({
  selector: 'app-require-qc-welder-dialog',
  templateUrl: './require-qc-welder-dialog.component.html',
  styleUrls: ['./require-qc-welder-dialog.component.scss'],
  providers: [RequireQcWelderService,MasterListService]
})
export class RequireQcWelderDialogComponent extends BaseDialogComponent<RequireQcWelder, RequireQcWelderService> {

  constructor(
    public service: RequireQcWelderService,
    public dialogRef: MatDialogRef<RequireQcWelderDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dataFrom?: { InfoValue: RequireQcWelder, Option: boolean }
  ) {
    super(service, dialogRef);
  }
  InfoValue: RequireQcWelder;
  // on init
  onInit(): void {
    this.InfoValue = this.dataFrom.InfoValue;
    this.fastSelectd = this.InfoValue.RequireHasWelderId === -99;
  }
  // on complate or cancel entity
  onComplateOrCancelEntity(infoValue?: { data: RequireQcWelder | undefined, force: boolean }): void {
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
