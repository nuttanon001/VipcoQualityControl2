import { Injectable } from '@angular/core';
import { BaseRestService } from '../../shared/base-rest.service';
import { RequireMoreWorkactivity } from './require-more-workactivity.model';
import { HttpErrorHandler } from '../../shared/http-error-handler.service';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class RequireMoreWorkactivityService extends BaseRestService<RequireMoreWorkactivity> {
  constructor(
    http: HttpClient,
    httpErrorHandler: HttpErrorHandler
  ) {
    super(
      http,
      "api/RequireQcMoreWorkActvity/",
      "RequireQcMoreWorkActvityService",
      "RequireQcMoreWorkActvityId",
      httpErrorHandler
    )
  }
}
