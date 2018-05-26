import { BaseModel } from "../../shared/base-model.model";

export interface QcReasons extends BaseModel {
  QualityControlReasonId: number;
  Name?: string;
  Description?: string;
  Remark?: string;
}
