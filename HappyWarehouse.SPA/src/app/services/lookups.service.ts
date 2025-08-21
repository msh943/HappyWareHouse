import { Injectable } from '@angular/core';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class LookupsService {

  constructor(private api: ApiService) { }
  roles() { return this.api.http.get<any[]>(`${this.api.base}/Lookups/roles`); }
  countries() { return this.api.http.get<any[]>(`${this.api.base}/Lookups/countries`); }
}
