import { BaseModel } from "../../shared/base-model.model";

export interface WelderNo extends BaseModel {
  WelderNoId: number;
  WelderNoCode?: string;
  TeamWelderNo?: string;
  RegisterDate?: Date;
  ExprireDate?: Date;
  Description?: string;
  Remark?: string;
  NameThai?: string;
  NameEnglish?: string;
  //Relation
  EmpCode?: string;
  EmployeeString?: string;
}
