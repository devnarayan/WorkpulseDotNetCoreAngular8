import { Component, OnInit } from '@angular/core';
import { SecurityProvider, SecurityUser, User } from './SecurityProvider/SecurityProvider';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Workpulse';
  currentUser: User;

  constructor(private router: Router, private securityUser: SecurityUser) {
    this.securityUser.currentUser.subscribe(x => this.currentUser = x);
  }

  ngOnInit() {

  }

  logout() {
    this.securityUser.logout();
    this.router.navigate(['/login']);
  }
}
