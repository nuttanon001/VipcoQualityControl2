import { MatDialogRef } from '@angular/material';
import { Component } from '@angular/core';

@Component({
    selector: 'confirm-dialog',
    template: `
    <div>
        <h4 class="text-info">
        <i class="fa fa-x2 fa-meh-o" aria-hidden="true"></i>
            {{ title }}
        </h4>
    </div>
    <p>{{ message }}</p>
    <hr/>
    <mat-form-field>
      <input type="number" [(ngModel)]="numberReturn" matInput
             placeholder="Specify number here.">
    </mat-form-field>
    <button type="button" mat-raised-button (click)="summitClick(true)" color="accent">Yes</button>
    <button type="button" mat-button (click)="dialogRef.close()" color="warn">No</button>
    `,
  styles: [`
      mat-form-field {
        width: 95%;
        min-height: 70px;
        margin: 5px;
      }
  `]
})
export class NumberMessageDialog {

  public title: string;
  public message: string;
  public numberReturn: number;

  constructor(public dialogRef: MatDialogRef<NumberMessageDialog>) {
  }

  summitClick(result:boolean):void {
    this.dialogRef.close({ result: result, numberReturn: this.numberReturn });
  }
}