export interface UserInfo {
  id: string;
  name: string;
  userName: string;
  email: string;
  phoneNumber: string;
}

export class UserMangementAuditHistoryModel{
  rowId: number;
  userName: string;
  updatedBy: string;
  updatedDate: string;
  roleId: string;
  actionType: string;
  locationCode: string;
  modifiedStartDate?: string;
  modifiedEndDate?: string;
  componentName?: string;
  countyName?: string;
}