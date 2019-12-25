import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { TopNavMenuComponent } from './top-nav-menu.component';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { UserServiceService } from '../_service/user-service.service';
import { UserServiceServiceMock } from '../mocks/user-service.service.mock';
import { RouterTestingModule } from '@angular/router/testing';
import { RouterModule, Routes, Router, ActivatedRoute, ParamMap } from '@angular/router';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { InjectionToken } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { SecurityUser } from '../SecurityProvider/SecurityProvider';
import { SecurityUserMock } from '../mocks/SecurityProvider.mock';
export const BASE_URL = new InjectionToken<string>('BASE_URL');

describe('TopNavMenuComponent', () => {
  let component: TopNavMenuComponent;
  let fixture: ComponentFixture<TopNavMenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [TopNavMenuComponent],
      schemas: [
        CUSTOM_ELEMENTS_SCHEMA
      ],
      imports: [ReactiveFormsModule, RouterTestingModule, HttpClientTestingModule],
      providers: [
        { provide: SecurityUser, useClass: SecurityUserMock },
        { provide: Router, useClass: RouterTestingModule },
        { provide: ActivatedRoute, useClass: RouterTestingModule }
      ]
    }).compileComponents().then(() => {
      fixture = TestBed.createComponent(TopNavMenuComponent);
      component = fixture.componentInstance;      
    });
  }));

  it('Should create TopNavMenuComponent', () => {
    expect(component).toBeTruthy();
  });

  it('User Name should be John Doe', () => {
    expect(component.userName).toEqual('John Doe');
  });
});
