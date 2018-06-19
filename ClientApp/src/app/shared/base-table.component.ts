// Angular Core
import { OnInit, ViewChild, Input, Output,EventEmitter, OnDestroy } from "@angular/core";
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
// Models
import { Scroll } from "./scroll.model";
// Component
import { SearchBoxComponent } from "./search-box/search-box.component";
// Services
import { BaseRestService } from "./base-rest.service";
//rxjs
import { debounce } from "rxjs/operators";
import { debounceTime } from "rxjs/operator/debounceTime";
import { Subscription } from "rxjs/Subscription";
import { AuthService } from "../core/auth/auth.service";

/** custom-mat-table component*/
export class BaseTableComponent<Model,Service extends BaseRestService<Model>> implements OnInit {
  /** custom-mat-table ctor */
  constructor(
    protected service: Service,
    protected authService: AuthService,
  ) { }

  // Parameter
  displayedColumns: Array<string> = ["select", "Col1", "Col2", "Col3"];
  @Input() isOnlyCreate: boolean = false;
  @Input() isDisabled: boolean = true;

  _isMultiple = false;
  @Input()
  get isMultiple(): boolean {
    return this._isMultiple;
  }
  set isMultiple(data: boolean) {
    this._isMultiple = data;
    // Selection
    this.selection = new SelectionModel<Model>(this.isMultiple, [], true)
    this.selection.onChange.subscribe(selected => {
      if (this.isMultiple) {
        if (selected.source) {
          // this.selectedRow = selected.source.selected[0];
          this.returnSelectesd.emit(selected.source.selected);
        }
      } else {
        if (selected.source.selected[0]) {
          this.selectedRow = selected.source.selected[0];
          this.returnSelected.emit(selected.source.selected[0]);
        }
      }
    });
  }

  @Input() isDialog: boolean = false;
  @Input() WhereId: number | undefined;
  @Input() Where: string | undefined;
  @Input() isSubAction: string = "GetScroll/";
  @Output() returnSelected: EventEmitter<Model> = new EventEmitter<Model>();
  @Output() returnSelectesd: EventEmitter<Array<Model>> = new EventEmitter<Array<Model>>();
  @Output() returnSelectedWith: EventEmitter<{ data: Model, option: number }> = new EventEmitter<{ data: Model, option: number }>();

  // Parameter MatTable
  dataSource = new MatTableDataSource<Model>();
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(SearchBoxComponent) searchBox: SearchBoxComponent;
  selection:SelectionModel<Model>;

  // Parameter Component
  resultsLength = 0;
  isLoadingResults = true;
  isRateLimitReached = false;
  selectedRow: Model;

  // Angular NgOnInit
  ngOnInit() {
    if (!this.isDialog) {
      if (this.displayedColumns.indexOf("Command") === -1) {
        this.displayedColumns.push("Command");
      }
    }
  
    this.searchBox.onlyCreate2 = this.isOnlyCreate;
    // If the user changes the sort order, reset back to the first page.
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    // Merge
    //, this.searchBox.search, this.searchBox.onlyCreate
    merge(this.sort.sortChange, this.paginator.page,
      this.searchBox.search, this.searchBox.onlyCreate)
      .pipe(
      startWith({}),
      switchMap(() => {
        this.isLoadingResults = true;
        let scroll: Scroll = {
          Skip: this.paginator.pageIndex * this.paginator.pageSize,
          Take: this.paginator.pageSize,
          Filter: this.searchBox.search2,
          SortField: this.sort.active,
          SortOrder: this.sort.direction === "desc" ? 1 : -1,
          Where: this.searchBox.onlyCreate2 ? this.authService.getAuth.UserName || "" : "",
          WhereId: this.WhereId
        };
        return this.service.getAllWithScroll(scroll,this.isSubAction);
      }),
      map(data => {
        // Flip flag to show that loading has finished.
        this.isLoadingResults = false;
        this.isRateLimitReached = false;
        this.resultsLength = data.Scroll.TotalRow;
        return data.Data;
      }),
      catchError(() => {
        this.isLoadingResults = false;
        // Catch if the GitHub API has reached its rate limit. Return empty data.
        this.isRateLimitReached = true;
        return observableOf([]);
      })
    ).subscribe(data => {
      this.dataSource.data = data;
    });
  }

   // Reload
  reloadData(): void {
    this.searchBox.onlyCreate.emit(true);
    //let scroll: Scroll = {
    //  Skip: 0,
    //  Take: this.paginator.pageSize,
    //  Filter: this.searchBox.search2,
    //  SortField: this.sort.active,
    //  SortOrder: this.sort.direction === "desc" ? 1 : -1,
    //  Where:  ""
    //};
    //this.service.getAllWithScroll(scroll, this.isSubAction).subscribe(dbData => {
    //  this.isLoadingResults = false;
    //  this.isRateLimitReached = false;
    //  // Set Data
    //  this.resultsLength = dbData.Scroll.TotalRow;
    //  this.dataSource.data = dbData.Data;
    //});
  }
  // OnAction Click
  onActionClick(data: Model, option: number = 0) {
    this.returnSelectedWith.emit({ data: data, option: option });
  }
}

