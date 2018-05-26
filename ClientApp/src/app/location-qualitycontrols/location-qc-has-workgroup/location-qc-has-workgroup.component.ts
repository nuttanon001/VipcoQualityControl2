// Angular Core
import { Component, OnInit, ViewChild, Input, Output, EventEmitter, AfterViewInit, OnChanges } from "@angular/core";
import { MatPaginator, MatSort, MatTableDataSource, MatCheckbox } from "@angular/material";
import { SelectionModel } from '@angular/cdk/collections';
// Rxjs
import { map } from "rxjs/operators/map";
import { Observable } from "rxjs/Observable";
import { merge } from "rxjs/observable/merge";
import { startWith } from "rxjs/operators/startWith";
import { switchMap } from "rxjs/operators/switchMap";
import { catchError } from "rxjs/operators/catchError";
import { of as observableOf } from "rxjs/observable/of";
import { WorkgroupHasWorkshop } from "../shared/workgroup-has-workshop.model";
// Module

@Component({
  selector: 'app-location-qc-has-workgroup',
  templateUrl: './location-qc-has-workgroup.component.html',
  styleUrls: ['./location-qc-has-workgroup.component.scss']
})
export class LocationQcHasWorkgroupComponent implements OnInit, OnChanges, AfterViewInit {
  /** custom-mat-table ctor */
  constructor() { }

  // Parameter
  displayedColumns: Array<string> = ["select", "GroupMis", "GroupMisString", "edit"];
  @Input() WorkShops: Array<WorkgroupHasWorkshop>;
  @Input() readOnly: boolean = false;
  @Output() returnSelected: EventEmitter<{ data: WorkgroupHasWorkshop, mode: number }> = new EventEmitter<{ data: WorkgroupHasWorkshop, mode: number }>();

  // Parameter MatTable
  dataSource = new MatTableDataSource<WorkgroupHasWorkshop>();
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  selection = new SelectionModel<WorkgroupHasWorkshop>(false, [], true);

  // Parameter Component
  resultsLength = 0;
  isLoadingResults = false;
  isRateLimitReached = false;
  selectedRow: WorkgroupHasWorkshop;

  ngOnChanges() {
    // console.log("ONChange", JSON.stringify(this.requisitions));
    this.dataSource = new MatTableDataSource<WorkgroupHasWorkshop>(this.WorkShops);
  }

  // Angular NgOnInit
  ngOnInit() {
    // this.dataSource = new MatTableDataSource<RequisitionStock>(new Array);

    // If the user changes the sort order, reset back to the first page.
    this.selection.onChange.subscribe(selected => {
      if (selected.source.selected[0]) {
        this.selectedRow = selected.source.selected[0];
        // this.returnSelected.emit(selected.source.selected[0]);
      }
    });
  }
  // Angular ngAfterViewInit
  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  // OnAction Click
  onActionClick(data: any, mode: number) {
    this.returnSelected.emit({ data: data, mode: mode });
  }
}
