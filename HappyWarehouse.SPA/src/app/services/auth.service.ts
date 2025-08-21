import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { tap } from 'rxjs/operators';
import { LoginRequest, LoginResponse } from '../models/login.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private api: ApiService) { }
  login(req: LoginRequest) {
    debugger;
    return this.api.http.post<LoginResponse>(`${this.api.base}/Auth/Login`, req)
      .pipe(tap(res => localStorage.setItem('jwt', res.token)));
  }
}
