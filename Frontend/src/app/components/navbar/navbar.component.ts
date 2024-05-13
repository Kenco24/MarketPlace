import { Component,inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit{
  constructor(
    private matSnackBar: MatSnackBar,
    private router: Router
  ) {}
  Balance:any;
  menu2: any;

  ngOnInit(): void {
      this.getUserDetails();
  }
  userDetails : any;
  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }
  authService = inject(AuthService);
  
  logout(): void {
    this.authService.logout();
    this.matSnackBar.open('Logout success', 'Close', {
      duration: 5000,
      horizontalPosition: 'center',
    });
    this.router.navigate(['/login']);
  }

  getUserDetails(): void {
    this.userDetails=this.authService.getUserDetail();
    console.log(this.userDetails.fullName + this.userDetails.email);
  
  }

  
}
