import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { Product } from '../../interfaces/other/product/product';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  pageSize = 5; // Number of products per page (3 rows per page, each row containing up to 5 products)
  currentPage = 1; // Current page number
  paginatedRows: Product[][] = []; // Define the property for paginated rows

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts() {
    this.productService.getProducts().subscribe(
      (response) => {
        this.products = response;
        // After getting products, fetch images for each product
        this.fetchProductImages();
        // Paginate the products into rows
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
        // Assuming imageData is a Blob or ArrayBuffer
        const reader = new FileReader();
        reader.onload = () => {
          // Set image data to product
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

  get paginatedProducts() {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    return this.products.slice(startIndex, startIndex + this.pageSize);
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    } else {
      this.currentPage = 1; // Reset to the first page
    }
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  get totalPages() {
    return Math.ceil(this.products.length / this.pageSize);
  }

  getPageNumbers(): number[] {
    return Array.from({ length: this.totalPages }, (_, index) => index + 1);
  }

  paginateProducts() {
    this.paginatedRows = [];
    const productsPerPage = 3 * this.pageSize; // 3 rows per page, each row containing up to 5 products
    const startIndex = (this.currentPage - 1) * productsPerPage;
    const endIndex = Math.min(startIndex + productsPerPage, this.products.length);
  
    for (let i = startIndex; i < endIndex; i += 5) {
      this.paginatedRows.push(this.products.slice(i, i + 5));
    }
  }
}
