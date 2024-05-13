import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { Product } from '../../interfaces/other/product/product';
import { MatDialog } from '@angular/material/dialog';
import { ProductDetailsDialogComponent } from '../product-details-dialog/product-details-dialog.component';
import { Router } from '@angular/router';


@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  currentPage = 1; 
  paginatedRows: Product[][] = []; 

  constructor(private productService: ProductService,public dialog: MatDialog, private router: Router) { }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts() {
    this.productService.getProducts().subscribe(
      (response) => {
        this.products = response;
        this.fetchProductImages(); 
        this.paginateProducts();
      },
      (error) => {
        console.error('Error loading products:', error);
      }
    );
  }

  fetchProductImages() {
    this.products.forEach((product) => {
      this.productService.getProductImage(product.id).subscribe(
        (imageData) => {
          const reader = new FileReader();
          reader.onload = () => {
            product.image = reader.result as string;
          };
          reader.readAsDataURL(imageData);
        },
        (error) => {
          console.error('Error fetching product image:', error);
        }
      );
    });
  }

  paginateProducts() {
    this.paginatedRows = [];
    const productsPerPage = 15; 
    const startIndex = (this.currentPage - 1) * productsPerPage;
    const endIndex = Math.min(startIndex + productsPerPage, this.products.length);
  
    for (let i = startIndex; i < endIndex; i += 5) {
      const row: Product[] = [];
      for (let j = i; j < i + 5 && j < endIndex; j++) {
        row.push(this.products[j]);
      }
      this.paginatedRows.push(row);
    }
  }
  

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.paginateProducts();
    }
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.paginateProducts();
    }
  }

  get totalPages() {
    const productsPerPage = 15; 
    return Math.ceil(this.products.length / productsPerPage);
  }

  getPageNumbers(): number[] {
    return Array.from({ length: this.totalPages }, (_, index) => index + 1);
  }


  openProductDetails(product: Product): void {
    const dialogRef = this.dialog.open(ProductDetailsDialogComponent, {
      width: '400px',
      data: product
    });
  }

  refreshPage() {
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      window.location.reload();
    });
  }
}
