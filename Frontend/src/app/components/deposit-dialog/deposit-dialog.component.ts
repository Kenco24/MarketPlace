import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BalanceService } from '../../services/balance.service';
import { MatDialogRef } from '@angular/material/dialog'; 


@Component({
  selector: 'app-deposit-dialog',
  templateUrl: './deposit-dialog.component.html',
  styleUrls: ['./deposit-dialog.component.css']
})
export class DepositDialogComponent implements OnInit {
  depositForm: FormGroup;
  isLoading = false;
  isDeposited = false; 


  constructor(private formBuilder: FormBuilder, private balanceService: BalanceService,  private dialogRef: MatDialogRef<DepositDialogComponent> ) {
    this.depositForm = this.formBuilder.group({
      cardNumber: ['', [Validators.required, Validators.pattern(/^\d{4}\s?\d{4}\s?\d{4}\s?\d{4}$/)]], // 4x4-digit Card number with optional spaces
      expirationDate: ['', [Validators.required, Validators.pattern(/^(0[1-9]|1[0-2])\/\d{2}$/)]], // MM/YY format
      cvc: ['', [Validators.required, Validators.pattern(/^\d{3}$/)]], // 3-digit CVC
      amount: ['', [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit(): void {
  }

  onSubmit(): void {
    if (this.depositForm.invalid) {
      console.log("Invalid form");
      return;
    }

    const amount = this.depositForm.value.amount;
    this.isLoading = true;

    this.balanceService.deposit(amount).subscribe(
      (response) => {
        this.isLoading = false;
        console.log('Deposit successful:', response);
        this.isDeposited = true; 
        this.balanceService.getBalance().subscribe(
          (balance) => {
           
            this.balanceService.updateBalance(balance); 
            this.dialogRef.close();
          },
          (error) => {
            console.error('Error retrieving balance:', error);
          }
        );
      },
      (error) => {
        this.isLoading = false;
        console.error('Deposit failed:', error);
      }
    );
  }
}
