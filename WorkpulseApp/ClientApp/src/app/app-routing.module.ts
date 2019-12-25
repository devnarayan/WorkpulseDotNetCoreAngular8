import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { UserManagementListComponent } from './component/userManagement/list/list.component';
import { AlertComponent } from './component/alert/alert.component';
import { QlikReportComponent } from './component/qlik-report/qlik-report.component';
import { AuthGuard } from '../app/_helpers/auth.guard';
import { PrintLayoutComponent } from './print-layout/print-layout.component';
import { UserManagementAuditHistoryComponent } from './component/userManagement/userManagement-audit-history/userManagement-audit-history.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent, pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent, pathMatch: 'full' },
  { path: 'user-management', component: UserManagementListComponent, pathMatch: 'full' },
  { path: 'user-management-audit-history', component: UserManagementAuditHistoryComponent, pathMatch: 'full' },
  { path: 'alert', component: AlertComponent, pathMatch: 'full' },
  { path: 'qlik-report', component: QlikReportComponent, pathMatch: 'full' },
  {
    path: 'print', outlet: 'print', component: PrintLayoutComponent,
    children: [
    ]
    },
    { path: '**', component: DashboardComponent },

];

@NgModule({
  imports: [RouterModule.forRoot(routes, { enableTracing: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
