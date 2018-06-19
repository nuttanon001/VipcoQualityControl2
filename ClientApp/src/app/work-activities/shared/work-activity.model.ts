import { BaseModel } from "../../shared/base-model.model";
import { TypeworkActivity } from "./typework-activity.enum";

export interface WorkActivity extends BaseModel {
  WorkActivityId: number;
  Name ?:string;
  Description ?:string;
  Remark?: string;
  TypeWorkActivity?: TypeworkActivity;
}
