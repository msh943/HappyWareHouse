import { Component, OnInit } from '@angular/core';
import { WarehouseStatusDto } from '../../models/warehouse-status-dto.dto';
import { WarehouseItemDto } from '../../models/warehouse-item.dto';
import { DashboardService } from '../../services/dashboard.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {

  status: WarehouseStatusDto[] = [];
  topHigh: WarehouseItemDto[] = [];
  topLow: WarehouseItemDto[] = [];
  constructor(private svc: DashboardService) { }
  ngOnInit() {
    this.svc.status().subscribe(r => this.status = r);
    this.svc.topHigh(10).subscribe(r => this.topHigh = r);
    this.svc.topLow(10).subscribe(r => this.topLow = r);
  }
}
