import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { WarehouseItemDto } from '../../../models/warehouse-item.dto';
import { ItemsService } from '../../../services/items.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-items-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './items-list.component.html',
  styleUrl: './items-list.component.css'
})
export class ItemsListComponent {
  rows: WarehouseItemDto[] = [];
  constructor(private svc: ItemsService, private router: Router) { }
  //ngOnInit() { this.load(); }
  //load() { this.svc.list().subscribe(r => this.rows = r.items); }
  //add() { this.router.navigateByUrl('/items/new'); }
  //edit(id: number) { this.router.navigateByUrl(`/items/edit/${id}`); }
  //remove(id: number) { if (confirm('Delete item?')) this.svc.delete(id).subscribe(_ => this.load()); }
}
