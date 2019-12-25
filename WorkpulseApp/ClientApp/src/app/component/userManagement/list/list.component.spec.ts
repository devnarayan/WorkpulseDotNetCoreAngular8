import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserManagementListComponent } from './list.component';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { CommonModule } from '@angular/common';
import { DevExtremeModule, DxDataGridModule } from 'devextreme-angular';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { UserServiceService } from '../../../_service/user-service.service';
import { UserServiceServiceMock } from '../../../Mocks/user-service.service.mock';
import { SecurityUser } from '../../../SecurityProvider/SecurityProvider';
import { SecurityUserMock } from '../../../Mocks/SecurityProvider.mock';

describe('ListComponent', () => {
  let component: UserManagementListComponent;
  let fixture: ComponentFixture<UserManagementListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [UserManagementListComponent],
      schemas: [
        CUSTOM_ELEMENTS_SCHEMA,
        NO_ERRORS_SCHEMA
      ],
      imports: [ReactiveFormsModule, HttpClientTestingModule, RouterTestingModule, FormsModule, CommonModule, DevExtremeModule, DxDataGridModule, MatDialogModule],
      providers: [
        { provide: UserServiceService, useClass: UserServiceServiceMock },
        { provide: MatDialog, useClass: MatDialog },
        { provide: 'BASE_URL', value: 'https://localhost:44357' },
        { provide: SecurityUser, useClass: SecurityUserMock },
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserManagementListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
