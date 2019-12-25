import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { UserManagementAuditHistoryComponent } from './userManagement-audit-history.component';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { FormsModule, ReactiveFormsModule, FormControl, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { DebitmemoService } from '../../../_service/debitmemo.service';
import { CashReceiptServiceMock } from '../../../Mocks/cashreceipt.service.mock';
import { CommonModule } from '@angular/common';
import { DevExtremeModule } from 'devextreme-angular';
import { DxDataGridModule } from 'devextreme-angular'
import { Router, ActivatedRoute } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { AlertService } from '../../alert';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { SecurityUser } from '../../../SecurityProvider/SecurityProvider';
import { SecurityUserMock } from '../../../mocks/SecurityProvider.mock';


describe('UserManagementAuditHistoryComponent', () => {
  let component: UserManagementAuditHistoryComponent;
  let fixture: ComponentFixture<UserManagementAuditHistoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [UserManagementAuditHistoryComponent],
      schemas: [
        CUSTOM_ELEMENTS_SCHEMA,
        NO_ERRORS_SCHEMA
      ],
      imports: [ReactiveFormsModule, HttpClientTestingModule, RouterTestingModule, FormsModule, CommonModule, DevExtremeModule, DxDataGridModule, MatDialogModule],
      providers: [
        { provide: 'BASE_URL', value: 'https://localhost:44357' },
        { provide: MatDialog, useClass: MatDialog },
        { provide: SecurityUser, useClass: SecurityUserMock },
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserManagementAuditHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
