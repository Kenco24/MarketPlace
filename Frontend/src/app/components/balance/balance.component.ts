import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { BalanceService } from '../../services/balance.service';
import { DepositDialogComponent } from '../deposit-dialog/deposit-dialog.component';
import { MatDialog } from '@angular/material/dialog';


@Component({
  selector: 'app-balance',
  templateUrl: './balance.component.html',
  styleUrls: ['./balance.component.css']
})
export class BalanceComponent implements OnInit, OnDestroy {
  balance: number = 0; // Initialize balance
  balanceSubscription!: Subscription; // Initialize balanceSubscription

  constructor(private balanceService: BalanceService,public dialog: MatDialog) { }

  ngOnInit(): void {
    this.getBalance();
    this.balanceSubscription = this.balanceService.getBalanceUpdates().subscribe(balance => {
      this.balance = balance;
    });
  }

  ngOnDestroy(): void {
    this.balanceSubscription.unsubscribe();
  }

  getBalance(): void {
    this.balanceService.getBalance().subscribe(balance => {
      this.balance = balance;
    });
  }

  openDepositDialog(): void {
    const dialogRef = this.dialog.open(DepositDialogComponent, {
      width: '250px'
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      // Handle any actions after dialog close if needed
    });
  }

 

 
}
