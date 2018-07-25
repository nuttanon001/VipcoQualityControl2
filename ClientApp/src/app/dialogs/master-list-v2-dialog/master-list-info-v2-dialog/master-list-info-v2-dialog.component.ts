// Angular Core
import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, Validators, AbstractControl } from '@angular/forms';
// rxjs
import { debounceTime, distinctUntilChanged } from "rxjs/operators";
// Models
import { MasterList } from '../../../master-lists/shared/master-list.model';
import { AutoComplate } from '../../shared/auto-complate';
import { ResultAutoComplate } from '../../shared/result-auto-complate';
// Services
import { MasterListService } from '../../../master-lists/shared/master-list.service';
// Extends
import { BaseInfoDialogComponent } from '../../../shared/base-info-dialog-component';

@Component({
  selector: 'app-master-list-info-v2-dialog',
  templateUrl: './master-list-info-v2-dialog.component.html',
  styleUrls: ['./master-list-info-v2-dialog.component.scss']
})
export class MasterListInfoV2DialogComponent extends BaseInfoDialogComponent<MasterList> {

  constructor(
    private fb: FormBuilder,
    private service:MasterListService,
  ) { super() }
  // parameter
  @Input() NeedWelder: boolean = false;

  resultAutoComplate: Array<ResultAutoComplate>;
  // methods
  buildForm(): void {
    this.InfoValueForm = this.fb.group({
      MasterProjectListId: [this.InfoValue.MasterProjectListId],
      Name: [this.InfoValue.Name,
        [
          Validators.maxLength(50)
        ]
      ],
      Description: [this.InfoValue.Description,
        [
          Validators.maxLength(200)
        ]
      ],
      Remark: [this.InfoValue.Remark,
        [
          Validators.maxLength(200)
        ]
      ],
      DrawingNo: [this.InfoValue.DrawingNo,
        [
          Validators.required,
          Validators.maxLength(200)
        ]
      ],
      MarkNo: [this.InfoValue.MarkNo,
        [
          Validators.required,
          Validators.maxLength(200),
        ]
      ],
      Length: [this.InfoValue.Length],
      Width: [this.InfoValue.Width],
      Heigth: [this.InfoValue.Heigth],
      Weigth: [this.InfoValue.Weigth],
      Quantity: [this.InfoValue.Quantity,
        [
          Validators.min(1),
          Validators.required,
        ]
      ],
      UnitNo: [this.InfoValue.UnitNo,
        [
          Validators.required,
          Validators.min(0)
        ]
      ],
      Box: [this.InfoValue.Box,
        [
          Validators.min(0)
        ]
      ],
      Revised: [this.InfoValue.Revised],
      ProjectCodeDetailId: [this.InfoValue.ProjectCodeDetailId],
      ProjectCodeDetailString: [this.InfoValue.ProjectCodeDetailString],
      // Update Welder
      Thickness: [this.InfoValue.Thickness,[Validators.min(0)]],
      JointNumber: [this.InfoValue.JointNumber, [Validators.min(0)]],
      GradeMaterial1: [this.InfoValue.GradeMaterial1, [Validators.maxLength(200)]],
      GradeMaterial2: [this.InfoValue.GradeMaterial2, [Validators.maxLength(200)]],
      TypeMaterial1: [this.InfoValue.TypeMaterial1, [Validators.maxLength(200)]],
      TypeMaterial2: [this.InfoValue.TypeMaterial2, [Validators.maxLength(200)]],
      RequireHasWelder:[this.InfoValue.RequireHasWelder],
      // BaseModel
      Creator: [this.InfoValue.Creator],
      CreateDate: [this.InfoValue.CreateDate],
      Modifyer: [this.InfoValue.Modifyer],
      ModifyDate: [this.InfoValue.ModifyDate],
    });

    this.InfoValueForm.valueChanges.pipe(debounceTime(250), distinctUntilChanged()).subscribe(data => {
      if (!this.InfoValueForm) { return; }
      if (this.InfoValueForm.valid) {
        this.InfoValue = this.InfoValueForm.value;
        this.SubmitOrCancel.emit({ data: this.InfoValue, force: false });
      }
    });
    // Get Control
    this.InfoValueForm.get("MarkNo").valueChanges
      .pipe(debounceTime(500), distinctUntilChanged()).subscribe((data: string) => {
        if (data) {
          this.fillAutoComplated({ ByColumn: "MarkNo", Filter: data });
        } else {
          this.resultAutoComplate = new Array;
        }
      });
    this.InfoValueForm.get("DrawingNo").valueChanges
      .pipe(debounceTime(500), distinctUntilChanged()).subscribe((data: string) => {
        if (data) {
          this.fillAutoComplated({ ByColumn: "DrawingNo", Filter: data });
        } else {
          this.resultAutoComplate = new Array;
        }
      });
    this.InfoValueForm.get("GradeMaterial1").valueChanges
      .pipe(debounceTime(500), distinctUntilChanged()).subscribe((data: string) => {
        if (data) {
          this.fillAutoComplated({ ByColumn: "GradeMaterial1", Filter: data });
        } else {
          this.resultAutoComplate = new Array;
        }
      });
    this.InfoValueForm.get("GradeMaterial2").valueChanges
      .pipe(debounceTime(500), distinctUntilChanged()).subscribe((data: string) => {
        if (data) {
          this.fillAutoComplated({ ByColumn: "GradeMaterial2", Filter: data });
        } else {
          this.resultAutoComplate = new Array;
        }
      });
    this.InfoValueForm.get("TypeMaterial1").valueChanges
      .pipe(debounceTime(500), distinctUntilChanged()).subscribe((data: string) => {
        if (data) {
          this.fillAutoComplated({ ByColumn: "TypeMaterial1", Filter: data });
        } else {
          this.resultAutoComplate = new Array;
        }
      });
    this.InfoValueForm.get("TypeMaterial2").valueChanges
      .pipe(debounceTime(500), distinctUntilChanged()).subscribe((data: string) => {
        if (data) {
          this.fillAutoComplated({ ByColumn: "TypeMaterial2", Filter: data });
        } else {
          this.resultAutoComplate = new Array;
        }
      });
   
  }
  // fill auto complate
  fillAutoComplated(fillAuto: AutoComplate) {
    this.service.getAutoComplateEdition(fillAuto)
      .subscribe(resultAuto => {
        this.resultAutoComplate = new Array;
        if (resultAuto) {
          this.resultAutoComplate = [...resultAuto];
        }
      });
  }
}
