import { BaseModel } from "../../shared/base-model.model";
import { RequireStatusQc } from "./require-status-qc.enum";
import { MasterList } from "../../master-lists/shared/master-list.model";
import { WorkActivity } from "../../work-activities/shared/work-activity.model";
import { RequireQcWelder } from "../../require-qc-welders/shared/require-qc-welder.model";
import { RequireQcHasMasterList } from "./require-qc-has-master-list.model";

export interface RequireQc extends BaseModel {
  RequireQualityControlId: number;
  RequireQualityNo? : string;
  RequireDate?: Date;
  ResponseDate?: Date;
  WelderDate?: Date;
  Description?: string;
  Remark?: string;
  MailReply?: string;
  RequireStatus?: RequireStatusQc;
  //FK
  //ParentRequireQcId
  ParentRequireQcId?: number;
  // GroupMis
  GroupMIS? : string;
  // Employee
  RequireEmp? : string;
  // ProjectCodeDetail
  ProjectCodeDetailId? :number;
  //WorkGroupQualityControl
  WorkGroupQualityControlId? :number;
  //InspectionPoint
  InspectionPointId? :number;
  //WorkActivity
  WorkActivityId? :number;
  //Branch
  BranchId?: number;
  RequireHasMasterProjects?: Array<RequireQcHasMasterList>;
  //ViewModel
  GroupMISString? :string;
  RequireEmpString? :string;
  ProjectCodeDetailString? :string;
  WorkGroupQualityControlString? :string;
  InspectionPointString? :string;
  WorkActivityString? :string;
  BranchString? :string;
  RequireStatusString?: string;
  MasterLists?: Array<MasterList>;
  RequireQcWelder?: Array<RequireQcWelder>;
  RequireQcTime?: Date;
  RequireQcTimeString?: any;
  WelderTime?: Date;
  WelderTimeString?: any;
  Option?: boolean;
  // Attach Model
  AttachFile?: FileList;
  RemoveAttach?: Array<number>;
  MoreWorkActvities?: Array<WorkActivity>;
}
