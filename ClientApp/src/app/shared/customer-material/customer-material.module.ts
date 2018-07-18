import { NgModule } from "@angular/core";
import { LayoutModule } from '@angular/cdk/layout';
import {
  MatAutocompleteModule,
  MatButtonModule,
  MatCardModule,
  MatCheckboxModule,
  MatDatepickerModule,
  MatDialogModule,
  MatExpansionModule,
  MatIconModule,
  MatInputModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatNativeDateModule,
  MatMenuModule,
  MatTooltipModule,
  MatTabsModule,
  MatTableModule,
  MatSortModule,
  MatStepperModule,
  MatSliderModule,
  MatSidenavModule,
  MatSelectModule,
  MatGridListModule,
} from "@angular/material";

import { MatMomentDateModule } from "@angular/material-moment-adapter";

import {
  DataTableModule,
  DialogModule,
  SharedModule,
  CalendarModule,
  DropdownModule,
  InputMaskModule,
  TreeModule,
  TreeTableModule,
  AccordionModule,
  AutoCompleteModule,
  RadioButtonModule,
  CheckboxModule,
} from "primeng/primeng";
// chart
//import { ChartsModule } from "ng2-charts/ng2-charts";

import { AngularSplitModule } from "angular-split";
//import { NgxDatatableModule } from "@swimlane/ngx-datatable";
// component
import { SearchBoxComponent } from "../search-box/search-box.component";
//import { AttactFileComponent } from "../../components/base-component/attact-file.component";
//import { AttachFileViewComponent } from "../../components/base-component/attach-file-view.component";
import { DateOnlyPipe } from "../../pipes/date-only.pipe";
// Moment
import * as moment from 'moment';
import { MAT_MOMENT_DATE_FORMATS, MomentDateAdapter } from '@angular/material-moment-adapter';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { AttachFileComponent } from '../attach-file/attach-file.component';
import { AttachFileViewComponent } from '../attach-file-view/attach-file-view.component';

@NgModule({
  declarations: [
    // component
    SearchBoxComponent,
    //AttactFileComponent,
    //BaseChartComponent,
    // pipe
    DateOnlyPipe,
    AttachFileComponent,
    AttachFileViewComponent,
  ],
  imports: [
    // material
    MatAutocompleteModule,
    MatButtonModule,
    MatCheckboxModule,
    MatProgressBarModule,
    MatDatepickerModule,
    MatMomentDateModule,
    MatExpansionModule,
    MatTooltipModule,
    MatSidenavModule,
    MatInputModule,
    MatIconModule,
    MatMenuModule,
    MatDialogModule,
    MatTabsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatStepperModule,
    MatSliderModule,
    MatSelectModule,
    MatGridListModule,
    LayoutModule,
    // angularSplit
    AngularSplitModule,
    // ngxDataTable
    // NgxDatatableModule,
    // primeNg
    DataTableModule,
    DialogModule,
    SharedModule,
    CalendarModule,
    DropdownModule,
    InputMaskModule,
    TreeModule,
    TreeTableModule,
    AccordionModule,
    AutoCompleteModule,
    RadioButtonModule,
    CheckboxModule,
    // chart
    //ChartsModule
    //Angular
    CommonModule,
    FormsModule,
  ],
  exports: [
    // material
    MatAutocompleteModule,
    MatButtonModule,
    MatCheckboxModule,
    MatProgressBarModule,
    MatDatepickerModule,
    MatMomentDateModule,
    MatExpansionModule,
    MatTooltipModule,
    MatSidenavModule,
    MatInputModule,
    MatIconModule,
    MatMenuModule,
    MatDialogModule,
    MatTabsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatStepperModule,
    MatSliderModule,
    MatSelectModule,
    MatGridListModule,
    LayoutModule,
    // angularSplit
    AngularSplitModule,
    // ngxDataTable
    // NgxDatatableModule,
    // primeNg
    DataTableModule,
    DialogModule,
    SharedModule,
    CalendarModule,
    DropdownModule,
    InputMaskModule,
    TreeModule,
    TreeTableModule,
    AccordionModule,
    AutoCompleteModule,
    RadioButtonModule,
    CheckboxModule,
    // component
    SearchBoxComponent,
    AttachFileComponent,
    AttachFileViewComponent,
    //AttactFileComponent,
    //BaseChartComponent,
    // pipe
    DateOnlyPipe,
    // chart
    //ChartsModule
  ],
  entryComponents: [
    SearchBoxComponent,
    AttachFileComponent,
    AttachFileViewComponent,
    //AttactFileComponent,
    //BaseChartComponent,
  ],
  providers: [
    // The locale would typically be provided on the root module of your application. We do it at
    // the component level here, due to limitations of our example generation script.
    { provide: MAT_DATE_LOCALE, useValue: 'th-TH' },

    // `MomentDateAdapter` and `MAT_MOMENT_DATE_FORMATS` can be automatically provided by importing
    // `MatMomentDateModule` in your applications root module. We provide it at the component level
    // here, due to limitations of our example generation script.
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS },
  ],
})
export class CustomMaterialModule { }
