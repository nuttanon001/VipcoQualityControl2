import { BaseModel } from "../../shared/base-model.model";

export interface Dashboard extends BaseModel {
  DashBoardDailyId?: number;
  DashBoardDate?: Date;
  TotalRequire?: number;
  TotalRequirePass?: number;
  TotalRequireFail?: number;
  Top1Name?: string;
  Top1Require?: number;
  Top2Name?: string;
  Top2Require?: number;
  Top3Name?: string;
  Top3Require?: number;
}
