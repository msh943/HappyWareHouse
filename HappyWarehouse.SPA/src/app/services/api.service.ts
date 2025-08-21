import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  readonly base = environment.apiUrl;


  constructor(public http: HttpClient) {
    console.log(
      '[ApiService] API BASE =', environment.apiUrl,
      '| production =', environment.production
    )
  }
}
