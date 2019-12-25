import { Component, OnInit, AfterViewInit } from '@angular/core';
import { UserServiceService } from '../_service/user-service.service';
import { RouterModule, Routes, Router, ActivatedRoute, ParamMap } from '@angular/router';
import * as $ from 'jquery';
import { SecurityProvider, SecurityUser } from '../SecurityProvider/SecurityProvider';

@Component({
  selector: 'app-top-nav-menu',
  templateUrl: './top-nav-menu.component.html',
  styleUrls: ['./top-nav-menu.component.scss']
})
export class TopNavMenuComponent implements OnInit, AfterViewInit {

  constructor(private user: SecurityUser, private route: ActivatedRoute, private router: Router) {
    this.userName = this.user.getName();
  }

  ngOnInit() {
    this.userName = this.user.getName();
  }

  ngAfterViewInit() {
    this.userName = this.user.getName();
  }

  userName: string;
  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
    if (this.isExpanded) {
      $(".main_container").addClass("minMenu");
      $(".child_menu").removeClass("show");
    }
    else {
      $(".main_container").removeClass("minMenu");
    }
  }
}
