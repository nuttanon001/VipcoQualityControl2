import { Component, OnInit, Inject } from '@angular/core';
import { BaseDialogEntryComponent } from '../../shared/base-dialog-entry.component';
import { MasterList } from '../../master-lists/shared/master-list.model';
import { MasterListService } from '../../master-lists/shared/master-list.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { BaseDialogComponent } from '../../shared/base-dialog.component';

@Component({
  selector: 'app-master-list-v2-dialog',
  templateUrl: './master-list-v2-dialog.component.html',
  styleUrls: ['./master-list-v2-dialog.component.scss'],
  providers: [ MasterListService ]
})
export class MasterListV2DialogComponent extends BaseDialogComponent<MasterList,MasterListService> {

  constructor(
    public service: MasterListService,
    public dialogRef: MatDialogRef<MasterListV2DialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dataFrom?: { InfoValue: MasterList,NeedWelder:boolean }
  ) {
    super(service, dialogRef);
  }
  InfoValue: MasterList;
  NeedWelder: boolean;
  // on init
  onInit(): void {
    this.InfoValue = this.dataFrom.InfoValue;
    this.NeedWelder = this.dataFrom.NeedWelder;
    this.fastSelectd = this.InfoValue.MasterProjectListId === -99;
  }
  // on complate or cancel entity
  onComplateOrCancelEntity(infoValue?: { data: MasterList | undefined, force: boolean }): void {
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
