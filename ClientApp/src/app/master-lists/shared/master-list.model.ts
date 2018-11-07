import { BaseModel } from "../../shared/base-model.model";
import { RequireQcWelder } from "../../require-qc-welders/shared/require-qc-welder.model";

export interface MasterList extends BaseModel {
  MasterProjectListId?: number;
  Name?: string;
  Description?: string;
  Remark?: string;
  DrawingNo?: string;
  MarkNo?: string;
  UnitNo?: string;
  Box?: number;
  Length?: number;
  Width?: number;
  Heigth?: number;
  Weigth?: number;
  Quantity?: number;
  Revised?: number;
  // Update Welder 11/07/18
  Thickness?: number;
  Thickness2?: number;
  JointNumber?: string;
  TypeMaterial1?: string;
  GradeMaterial1?: string;
  TypeMaterial2?: string;
  GradeMaterial2?: string;
  //FK
  ProjectCodeDetailId?: number;
  //ViewModel
  ProjectCodeDetailString?: string;
  RequireHasWelder?: RequireQcWelder;
  FailQuantity?: number;
  RemarkExter?: string;
}
