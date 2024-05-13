import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { AuthService } from './auth.service'; 

@Injectable({
  providedIn: 'root'
})
export class BalanceService {
  private baseUrl = 'https://localhost:7182/api/Balance'; 
  private balanceSubject = new Subject<number>();

  constructor(private http: HttpClient, private authService: AuthService) { } 

  getBalance(): Observable<number> {
    const token = this.authService.getToken(); 
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<number>(this.baseUrl, { headers });
  }

  deposit(amount: number): Observable<any> {
    const token = this.authService.getToken(); 
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.post<any>(`${this.baseUrl}/deposit`, amount, { headers });
  }

  withdraw(amount: number): Observable<any> {
    const token = this.authService.getToken(); 
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.post<any>(`${this.baseUrl}/withdraw`, amount, { headers });
  }

  updateBalance(balance: number): void {
    this.balanceSubject.next(balance); 
  }

  getBalanceUpdates(): Observable<number> {
    return this.balanceSubject.asObservable();
  }
}
