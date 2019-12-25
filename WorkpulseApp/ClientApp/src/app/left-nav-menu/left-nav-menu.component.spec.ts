import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { UserServiceService } from '../_service/user-service.service';
import { UserServiceServiceMock } from '../mocks/user-service.service.mock';
import { RouterTestingModule } from '@angular/router/testing';
import { RouterModule, Routes, Router, ActivatedRoute, ParamMap } from '@angular/router';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { InjectionToken } from '@angular/core';
export const BASE_URL = new InjectionToken<string>('BASE_URL');
import { LeftNavMenuComponent } from './left-nav-menu.component';
import { FormsModule, ReactiveFormsModule, FormControl, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { SecurityUser } from '../SecurityProvider/SecurityProvider';
import { SecurityUserMock } from '../mocks/SecurityProvider.mock';


describe('LeftNavMenuComponent', () => {
  let component: LeftNavMenuComponent;
  let fixture: ComponentFixture<LeftNavMenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [LeftNavMenuComponent],
      schemas: [
        CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA
      ],
      imports: [ReactiveFormsModule, HttpClientTestingModule],
      providers: [
        { provide: SecurityUser, useClass: SecurityUserMock },
        { provide: Router, useClass: RouterTestingModule },
        { provide: ActivatedRoute, useClass: RouterTestingModule }
      ]
    })
      .compileComponents().then(() => {
        fixture = TestBed.createComponent(LeftNavMenuComponent);
        component = fixture.componentInstance;
      });
  }));

  it('should create LeftNavMenuComponent', () => {
    expect(component).toBeTruthy();
  });

  it('User Name should be John Doe', () => {
    expect(component.userName).toEqual('John Doe');
  });
});
