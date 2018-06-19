import { BaseModel } from "../../shared/base-model.model";

export interface RequireMoreWorkactivity extends BaseModel {
  RequireQcMoreWorkActvityId: number;
  //Relation
  //RequireQualityControl
  RequireQualityControlId?: number;
  //WorkActivity
  WorkActivityId?: number;
  //ViewModel
  WorkActivityName?: string;
}
