import { BaseModel } from "../../shared/base-model.model";
import { WelderProject } from "./welder-project.model";

export interface WelderProjectHasEmp extends BaseModel {
  ProjectCodeMasterId?: number;
  ProjectCodeMasterString?: string;
  TotalWelder?: number;
  WelderProjects?: Array<WelderProject>;
}
