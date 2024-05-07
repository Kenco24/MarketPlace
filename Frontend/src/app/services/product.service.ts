import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private baseUrl = 'https://localhost:7182/api/Product'; // Update with your API base URL

  constructor(private http: HttpClient, private authService:AuthService) { }

  // Add methods to interact with your API endpoints

  addProduct(productData: any): Observable<any> {
    const token = this.authService.getToken();
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.post<any>(`${this.baseUrl}/addProduct`, productData, { headers });
  }

  editProduct(productId: number, productData: any): Observable<any> {
    const token = this.authService.getToken();
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.put<any>(`${this.baseUrl}/${productId}`, productData, { headers });
  }
  deleteProduct(productId: number): Observable<any> {
    const token = this.authService.getToken();
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.delete<any>(`${this.baseUrl}/${productId}`, { headers });
  }

  getProducts(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/getProducts`);
  }

  getProductImage(productId: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/${productId}/image`, { responseType: 'blob' as 'json' });
}

  getProductsByUser(userId: string): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/${userId}/user`);
  }

  getProductsByCategory(categoryId: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/category/${categoryId}`);
  }

  getProductById(productId: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/${productId}`);
  }

 

}
