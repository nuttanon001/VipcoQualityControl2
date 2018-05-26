import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
// services
import { HttpErrorHandler } from "../../shared/http-error-handler.service"
// models
import { QcReasons } from "./qc-reasons.model";
// Base-Services
import { BaseRestService } from "../../shared/base-rest.service";
import { BaseCommunicateService } from "../../shared/base-communicate.service";
// rxjs
import { Observable } from "rxjs/Observable";
import { catchError } from "rxjs/operators";

@Injectable()
export class QcReasonsService extends BaseRestService<QcReasons> {
  constructor(
    http: HttpClient,
    httpErrorHandler: HttpErrorHandler
  ) {
    super(
      http,
      "api/QualityControlReason/",
      "QualityControlReasonService",
      "QualityControlReasonId",
      httpErrorHandler
    )
  }
}

@Injectable()
export class QcReasonsCommunicateService extends BaseCommunicateService<QcReasons> {
  constructor() { super(); }
}
