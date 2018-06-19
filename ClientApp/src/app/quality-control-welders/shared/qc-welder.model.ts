import { BaseModel } from "../../shared/base-model.model";

export interface QcWelder extends BaseModel {
  QualityControlWeldingId: number;
  WeldingDate?: Date;
  MarkNo?: string;
  MarkNoPreview?: string;
  WelderNo?: string;
  ProcessWeld?: string;
  JointNo?: number;
  Thickness?: number;
  TestLength?: number;
  FailLength?: number;
  Reject?: number;
  Remark?: string;
  // Relation
  // QualityControlWelding
  ParentQcWeldingId?: number;
  // ResponseBy
  ResponseBy?: string;
  // RequireQualityControl
  RequireQualityControlId?: number;
  // ProjectCodeMaster
  ProjectCodeMasterId?: number;
  // QualityControlReason
  QualityControlReasonId?: number;
  // ViewModel
  ProjectCodeMasterString?: string;
  RequireQualityControlNo?: string;
  ResponseByString?: string;
  HasFail?: boolean;
}
