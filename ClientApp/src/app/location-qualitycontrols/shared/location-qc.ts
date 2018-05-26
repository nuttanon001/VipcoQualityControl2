import { BaseModel } from "../../shared/base-model.model";
import { WorkgroupHasWorkshop } from "./workgroup-has-workshop.model";

export interface LocationQc extends BaseModel {
  LocationQualityControlId: number;
  Name?: string;
  Description?: string;
  //FK
  WorkGroupHasWorkShops?: Array<WorkgroupHasWorkshop>;
}
