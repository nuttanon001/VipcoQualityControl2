import { LocationQc } from "./location-qc";
import { BaseModel } from "../../shared/base-model.model";

export interface LocationQcGroup extends BaseModel {
  Name?: string;
  Locations?: Array<LocationQc>;
}
