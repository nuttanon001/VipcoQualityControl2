import { BaseModel } from "../../shared/base-model.model";
import { WelderStatus } from "./welder-status.enum";
import { WelderProcess } from "./welder-process.enum";
import { MasterList } from "../../master-lists/shared/master-list.model";

export interface RequireQcWelder extends BaseModel {
  RequireHasWelderId?: number;
  VTStaus?: WelderStatus;
  QcStatus?: WelderStatus;
  WelderProcess?: WelderProcess;
  WelderDate?: Date;
  PercentNDE?: number;
  Remark?: string;
  //Relation
  //RequireHasMasterProjectId
  RequireHasMasterProjectId?: number;
  RequireHasMasterProject?: MasterList;
  //Welder1
  WelderNo1Id?: number;
  WelderNo1Name?: string;
  //Welder2
  WelderNo2Id?: number;
  WelderNo2Name?: string;
  //ViewModel
  WelderNo1String?: string;
  WelderNo2String?: string;
  VTStausString?: string;
  WelderProcessString?: string;
}
