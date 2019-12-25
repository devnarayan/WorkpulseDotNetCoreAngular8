import { Injectable } from "@angular/core";
import { UserServiceService, RoleModel } from "../_service/user-service.service";
import { async } from 'rxjs/internal/scheduler/async';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable()
export class SecurityProvider {
    constructor(private service: UserServiceService) {
    }

    loadUserInfo() {
        return this.loadUser();
    }

    loadUser = (): Promise<any> => {
        return new Promise(async (resolve, reject) => {
            var result = await this.service.getSignInSecurityUserInfo().toPromise().then(result => {
                var userInfo = result;
                if (userInfo == null) {
                    window.location.href = "/Account/signin";
                }
                else {
                    //sessionStorage.setItem('name', userInfo.name);
                    //sessionStorage.setItem('userName', userInfo.userName);
                    //sessionStorage.setItem('token', userInfo.token);
                    //sessionStorage.setItem('userRoles', JSON.stringify(userInfo.userRoles));

                    resolve(true);
                }
            }, error => {
                console.error(error);
                reject(false);
            })
        });

    }

}

export class User {
    name: string;
    userName: string;
    userRoles: RoleModel[];
    token: string;
}

export class SecurityUser {
    private currentUserSubject: BehaviorSubject<User>;
    public currentUser: Observable<User>;
    user = new User();
    userRoles: RoleModel[];
    token: string;

    constructor() {
        this.user = this.getSessionUserModel();
        this.setUserRoles();
        this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('CORTNE_User')));
        this.currentUser = this.currentUserSubject.asObservable();
    }

    public get currentUserValue(): User {
        return this.currentUserSubject.value;
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('CORTNE_User');
        this.currentUserSubject.next(null);
    }

    getSessionUserModel = () => {
        return (<any>Object).assign(new User(), JSON.parse(sessionStorage.getItem('CORTNE_User')));
    }

    getName = () => {
        return sessionStorage.getItem('name');
    }

    getUserName = () => {
        return sessionStorage.getItem('userName');
    }

    getToken = () => {
        return sessionStorage.getItem('token');
    }

    setUserRoles = () => {
        this.userRoles = JSON.parse(sessionStorage.getItem('userRoles'));
    }

    hasRoleByName = (roleName: string) => {
        if (this.userRoles == null) {
            this.userRoles = JSON.parse(sessionStorage.getItem('userRoles'));
        }

        if (this.userRoles != undefined && this.userRoles != null) {
            var result = this.userRoles.find(x => x.roleName == roleName && x.assigned == true);
            if (result == undefined) {
                return false;
            }
            else if (result != null) {
                return true;
            }
            else {
                return false;
            }
        }
    }

    hasRoleByNameOld = (roleName: string) => {
        if (this.user.userRoles != undefined && this.user.userRoles != null) {
            var result = this.user.userRoles.find(x => x.roleName == roleName && x.assigned == true);
            if (result == undefined) {
                return false;
            }
            else if (result != null) {
                return true;
            }
            else {
                return false;
            }
        }
    }

    IsSystemAdmin = () => {
        return this.hasRoleByName('System Admin');
    }

    IsAOBAdmin = () => {
        return this.hasRoleByName('AOB Admin');
    }

    IsAOBUser = () => {
        return this.hasRoleByName('AOB User');
    }

    IsBudgetAnalyst = () => {
        return this.hasRoleByName('Budget Analyst');
    }

    IsDOAdmin = () => {
        return this.hasRoleByName('DO Admin');
    }

    IsDODirector = () => {
        return this.hasRoleByName('DO Director');
    }

    IsDOLockUnlock = () => {
        return this.hasRoleByName('DO Lock/Unlock');
    }

    IsARSAdmin = () => {
        return this.hasRoleByName('ARS Admin');
    }

    IsARSUser = () => {
        return this.hasRoleByName('ARS User');
    }

    IsDebitMemoAdmin = () => {
        return this.hasRoleByName('Debit Memo Admin');
    }

    IsDebitMemoCommenter = () => {
        return this.hasRoleByName('Debit Memo Commenter');
    }

    IsDebitMemoUser = () => {
        return this.hasRoleByName('Debit Memo User');
    }

    IsDebitMemoViewer = () => {
        return this.hasRoleByName('Debit Memo Viewer');
    }

  IsDebitMemoMQAAdmin = () => {
    return this.hasRoleByName('Debit Memo MQA Admin');
  }

  IsDebitMemoMQACommenter = () => {
    return this.hasRoleByName('Debit Memo MQA Commenter');
  }

  IsDebitMemoMQAViewer = () => {
    return this.hasRoleByName('Debit Memo MQA Viewer');
  }


  IsDPRAdmin = () => {
    return this.hasRoleByName('DPR Admin');
  }

    IsDPRUser = () => {
        return this.hasRoleByName('DPR User');
    }

    IsLogManagement = () => {
        return this.hasRoleByName('Log Management');
    }

    IsUserManagement = () => {
        return this.hasRoleByName('User Management');
    }

    IsMFCAdmin = () => {
        return this.hasRoleByName('MFC Admin');
    }

    IsMFCUser = () => {
        return this.hasRoleByName('MFC User');
    }

    IsOCAApprover = () => {
        return this.hasRoleByName('OCA Approver');
    }

    IsOCARequester = () => {
        return this.hasRoleByName('OCA Requester');
    }

    IsReportViewer = () => {
        return this.hasRoleByName('Reports Viewer');
    }

    IsCORTNEDev = () => {
        return this.hasRoleByName('CORTNE Dev');
    }

    IsEmployee = () => {
        return this.hasRoleByName('Employee');
    }

    IsCRLogUser = () => {
        return this.hasRoleByName('CR Log User');
    }
    IsCRAccountUser = () => {
        return this.hasRoleByName('CR Account User');
    }
    IsCRAdmin = () => {
        return this.hasRoleByName('CR Admin');
    }

    IsMQAAdmin = () => {
        return this.hasRoleByName('Debit Memo MQA Admin');
    }

    IsMQAUser = () => {
        return this.hasRoleByName('Debit Memo MQA User');
    }
    CanEditDebitMemo = () => {

        return this.IsDebitMemoAdmin() || this.IsSystemAdmin() || this.IsDebitMemoUser();

    }
    CanReadWriteUserManagement = () => {

        return this.IsSystemAdmin() || this.IsDOAdmin() || this.IsAOBUser() || this.IsBudgetAnalyst() || this.IsCORTNEDev() || this.IsDebitMemoAdmin() || this.IsDebitMemoCommenter()
            || this.IsDebitMemoUser() || this.IsDODirector() || this.IsDOLockUnlock() || this.IsDPRAdmin() || this.IsDPRUser() || this.IsEmployee()
            || this.IsLogManagement() || this.IsMFCAdmin() || this.IsMFCUser() || this.IsOCAApprover() || this.IsOCARequester() || this.IsReportViewer()
            || this.IsUserManagement();

    }
}
