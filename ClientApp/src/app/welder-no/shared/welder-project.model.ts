import { BaseModel } from "../../shared/base-model.model";

export interface WelderProject extends BaseModel {
  WelderHasProjectId: number;
  Description?: string;
  Remark?: string;
  WelderNoId?: number;
  ProjectCodeMasterId?: number;
  ProjectCodeMasterString?: string;
  EmployeeString?: string;
}
