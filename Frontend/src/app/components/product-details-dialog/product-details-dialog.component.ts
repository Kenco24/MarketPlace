import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA,MatDialogRef} from '@angular/material/dialog';
import { Product } from '../../interfaces/other/product/product';

@Component({
  selector: 'app-product-details-dialog',
  templateUrl: './product-details-dialog.component.html',
  styleUrls: ['./product-details-dialog.component.css']
})
export class ProductDetailsDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<ProductDetailsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public product: Product
  ) {}
  buyProduct(): void {
    console.log('Buying product:', this.product.name);
    this.dialogRef.close();
  }
}