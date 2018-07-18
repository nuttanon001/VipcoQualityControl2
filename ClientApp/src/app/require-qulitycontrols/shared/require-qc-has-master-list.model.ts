import { BaseModel } from "../../shared/base-model.model";
import { MasterList } from "../../master-lists/shared/master-list.model";
import { RequireQcWelder } from "../../require-qc-welders/shared/require-qc-welder.model";

export interface RequireQcHasMasterList extends BaseModel {
  RequireHasMasterProjectId: number;
  Quantity?: number;
  PassQuantity?: number;
  // FK
  // MasterProjectList
  MasterProjectListId?: number;
  MasterProjectList?: MasterList;
  RequireHasWelder?: RequireQcWelder;
  // RequireQualityControl
  RequireQualityControlId?: number;
  //QualityControlReason
  QualityControlReasonId?: number;
  // ViewModel
  MarkNoString?: string;
  HasFail?: boolean;
  DrawingNo ?: string;
  UnitNo ?:number;
  Box ?:number;
}
