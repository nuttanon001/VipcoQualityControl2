// Angular Core
import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
// Components
import { BaseDialogComponent } from '../../shared/base-dialog.component';
// Models
import { ResultAutoComplate } from '../shared/result-auto-complate';
import { MasterList } from '../../master-lists/shared/master-list.model';
import { QcWelder } from '../../quality-control-welders/shared/qc-welder.model';
// Services
import { MasterListService } from '../../master-lists/shared/master-list.service';
import { QcWelderService } from '../../quality-control-welders/shared/qc-welder.service';
// Rxjs
import { filter } from "rxjs/operator/filter";
import { debounceTime, distinctUntilChanged, switchMap, retry } from "rxjs/operators";
import { AutoComplate } from '../shared/auto-complate';

@Component({
  selector: 'app-quality-control-welders-dialog',
  templateUrl: './quality-control-welders-dialog.component.html',
  styleUrls: ['./quality-control-welders-dialog.component.scss'],
  providers: [QcWelderService,MasterListService]
})
export class QualityControlWeldersDialogComponent implements OnInit {
  constructor(
    private service: QcWelderService,
    private serviceMasterList: MasterListService,
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<QualityControlWeldersDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public dataQcWelder: QcWelder
  ) { }

  //Parameter
  masterLists: Array<MasterList>;
  resultAutoComplate: Array<ResultAutoComplate>;
  editValueForm: FormGroup;
  // Angular on init
  ngOnInit(): void {
    if (!this.dataQcWelder) {
      this.dataQcWelder = {
        QualityControlWeldingId: 0,
        WeldingDate: new Date
      };
    }

    this.buildForm();
  }
  // Bulid form
  buildForm(): void {
    this.editValueForm = this.fb.group({
      QualityControlWeldingId: [this.dataQcWelder.QualityControlWeldingId],
      WeldingDate: [this.dataQcWelder.WeldingDate,
        [
          Validators.required
        ]
      ],
      MarkNo: [this.dataQcWelder.MarkNo,
        [
          Validators.maxLength(100),
          Validators.required
        ]
      ],
      MarkNoPreview: [this.dataQcWelder.MarkNoPreview,
        [
          Validators.maxLength(100),
          Validators.required
        ]
      ],
      WelderNo: [this.dataQcWelder.WelderNo,
        [
          Validators.maxLength(100),
          Validators.required
        ]
      ],
      ProcessWeld: [this.dataQcWelder.ProcessWeld,
        [
          Validators.maxLength(100),
          Validators.required
        ]
      ],
      JointNo: [this.dataQcWelder.JointNo,
        [
          Validators.min(0)
        ]
      ],
      Thickness: [this.dataQcWelder.Thickness,
        [
          Validators.min(0)
        ]
      ],
      TestLength: [this.dataQcWelder.TestLength,
        [
          Validators.required,
          Validators.min(0)
        ]
      ],
      FailLength: [this.dataQcWelder.FailLength,
        [
          Validators.min(0)
        ]
      ],
      Reject: [this.dataQcWelder.Reject,
        [
          Validators.min(0)
        ]
      ],
      Remark: [this.dataQcWelder.Remark,
        [
          Validators.maxLength(200)
        ]
      ],
      // BaseModel
      Creator: [this.dataQcWelder.Creator],
      CreateDate: [this.dataQcWelder.CreateDate],
      Modifyer: [this.dataQcWelder.Modifyer],
      ModifyDate: [this.dataQcWelder.ModifyDate],
      // Relation
      ParentQcWeldingId: [this.dataQcWelder.ParentQcWeldingId],
      ResponseBy: [this.dataQcWelder.ResponseBy],
      RequireQualityControlId: [this.dataQcWelder.RequireQualityControlId],
      ProjectCodeMasterId: [this.dataQcWelder.ProjectCodeMasterId],
      QualityControlReasonId: [this.dataQcWelder.QualityControlReasonId],
      ProjectCodeMasterString: [this.dataQcWelder.ProjectCodeMasterString],
      RequireQualityControlNo: [this.dataQcWelder.RequireQualityControlNo],
    });
    const ControlMarkNoPre: AbstractControl | undefined = this.editValueForm.get("MarkNoPreview");
    const ControlWelderNo: AbstractControl | undefined = this.editValueForm.get("WelderNo");
    const ControlProcessWeld: AbstractControl | undefined = this.editValueForm.get("ProcessWeld");
    const ControlMarkNo: AbstractControl | undefined = this.editValueForm.get("MarkNo");
    // Fill Autocomplate
    if (ControlProcessWeld) {
      ControlProcessWeld.valueChanges
        .pipe(debounceTime(500), distinctUntilChanged()).subscribe((data: string) => {
          if (data) {
            this.fillAutoComplated({ ByColumn: "ProcessWeld", Filter: data });
          } else {
            this.resultAutoComplate = new Array;
          }
        });
    }
    if (ControlWelderNo) {
      ControlWelderNo.valueChanges
        .pipe(debounceTime(500), distinctUntilChanged()).subscribe((data: string) => {
          if (data) {
            this.fillAutoComplated({ ByColumn: "WelderNo", Filter: data });
          } else {
            this.resultAutoComplate = new Array;
          }
        });
    }
    if (ControlMarkNoPre) {
      ControlMarkNoPre.valueChanges
        .pipe(debounceTime(500), distinctUntilChanged()).subscribe((data: string) => {
          if (data) {
            this.fillAutoComplated({ ByColumn: "MarkNoPreview", Filter: data });
          } else {
            this.resultAutoComplate = new Array;
          }
        });
    }
    if (ControlMarkNo) {
      ControlMarkNo.valueChanges
        .pipe(
          debounceTime(500),
          distinctUntilChanged()).subscribe((data: string) => {
            if (data) {
              this.filterMasterList(data);
            } else {
              this.masterLists = new Array;
            }
          });
    }
  }
  // old style auto complate
  filterMasterList(makrNo: string) {
    this.serviceMasterList.masterProjectListAutoComplate(makrNo)
      .subscribe(markNoAuto => {
        this.masterLists = new Array;
        this.masterLists = [...markNoAuto];
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
  // On cancel click
  onCancelForm(): void {
    this.dialogRef.close();
  }
  // On submit click
  onSubmitForm(): void {
    if (this.editValueForm) {
      // If Valid is fail
      if (!this.editValueForm.valid) { return; }

      this.dataQcWelder = this.editValueForm.value;
      if (this.dataQcWelder) {
        this.dialogRef.close(this.dataQcWelder);
      }
    }
  }
}
