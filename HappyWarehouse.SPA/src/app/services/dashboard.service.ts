import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { WarehouseStatusDto } from '../models/warehouse-status-dto.dto';
import { WarehouseItemDto } from '../models/warehouse-item.dto';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  constructor(private api: ApiService) { }
  status() { return this.api.http.get<WarehouseStatusDto[]>(`${this.api.base}/Dashboard/GetStatus`); }
  topHigh(top = 10) { return this.api.http.get<WarehouseItemDto[]>(`${this.api.base}/Dashboard/HighItems`, { params: { top } }); }
  topLow(top = 10) { return this.api.http.get<WarehouseItemDto[]>(`${this.api.base}/Dashboard/LowItems`, { params: { top } }); }
}
