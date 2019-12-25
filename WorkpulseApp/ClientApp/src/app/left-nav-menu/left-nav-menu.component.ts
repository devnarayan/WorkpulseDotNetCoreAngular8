import { Component, OnInit, AfterViewInit } from '@angular/core';

import { RouterModule, Routes, Router, ActivatedRoute, ParamMap } from '@angular/router';
import * as $ from 'jquery';
import { SecurityProvider, SecurityUser, User } from '../SecurityProvider/SecurityProvider';

@Component({
  selector: 'app-left-nav-menu',
  templateUrl: './left-nav-menu.component.html',
  styleUrls: ['./left-nav-menu.component.scss'],
})
export class LeftNavMenuComponent implements OnInit, AfterViewInit {
  currentView = "workbasketLeftMenu";
  userName: string;
  isDMUser: boolean = false;
  isDMAdmin: boolean = false;
  isMFCUser: boolean = false;
  isAOBUser: boolean = false;
  isSystemAdmin: boolean = false;
  isCORTNEDev: boolean = false;
  isDMCommenter: boolean = false;
  isDMViewer: boolean = false;
  canReadWriteUserManagement: boolean = false;
  mySubscription: any;
  currentUser: User;

  constructor(private user: SecurityUser, private route: ActivatedRoute, private router: Router) {
    //this.user.currentUser.subscribe(x => this.currentUser = x);
    this.userName = this.user.getName();

    if (this.userName == null) {
      window.location.href = "/Account/signin";      
    }
    else {
      this.isSystemAdmin = this.user.IsSystemAdmin();
      this.isDMAdmin = this.user.IsDebitMemoAdmin();
      this.isDMUser = this.user.IsDebitMemoUser();
      this.isAOBUser = this.user.IsAOBUser();
      this.isMFCUser = this.user.IsMFCUser();
      this.isCORTNEDev = this.user.IsCORTNEDev();
      this.isDMCommenter = this.user.IsDebitMemoCommenter();
      this.isDMViewer = this.user.IsDebitMemoViewer();
      this.canReadWriteUserManagement = this.user.CanReadWriteUserManagement();
    }  
  }

  ngOnInit() {
    
  }

  ngAfterViewInit() {
    
  }

  toggleView(view) {
    this.currentView = view;
    var bl = $("#" + view).hasClass("show");
    if (bl) {
      $("#" + view).removeClass("show");
    }
    else {
      $("#" + view).addClass("show");
    }
  }

  currentSubView = "";
  toggleSubView(view) {
    this.currentSubView = view;
    var bl = $("#" + view).hasClass("show");
    if (bl) {
      $("#" + view).removeClass("show");
    }
    else {
      $("#" + view).addClass("show");
    }
  }
}
