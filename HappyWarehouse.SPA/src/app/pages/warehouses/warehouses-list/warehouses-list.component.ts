import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { WarehouseDto } from '../../../models/warehouse.dto';
import { WarehousesService } from '../../../services/warehouses.service';
import { CommonModule } from '@angular/common';
import { Validators, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { WarehouseItemDto } from '../../../models/warehouse-item.dto';
import { ItemsService } from '../../../services/items.service';
import { take } from 'rxjs/operators';
import { applyServerErrors } from '../../../core/server-validate';

@Component({
  selector: 'app-warehouses-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './warehouses-list.component.html',
  styleUrl: './warehouses-list.component.css'
})
export class WarehousesListComponent implements OnInit {
  rows: WarehouseDto[] = [];


  expandedId: number | null = null;
  items: WarehouseItemDto[] = [];
  warehouseName = '';


  editingId = 0;
  itemForm = this.fb.nonNullable.group({
    itemName: ['', Validators.required],
    skuCode: [''],
    qty: [0, [Validators.required, Validators.min(1)]],
    costPrice: [0, [Validators.required, Validators.min(0.01)]],
    msrpPrice: [0, [Validators.min(0)]]
  });

  constructor(
    private svc: WarehousesService,
    private itemsSvc: ItemsService,
    private router: Router,
    private fb: FormBuilder
  ) { }

  ngOnInit() { this.load(); }
  load() { this.svc.getAll().subscribe(r => this.rows = r.warehouses); }

  add() { this.router.navigateByUrl('/warehouses/new'); }
  edit(id: number) { this.router.navigateByUrl(`/warehouses/edit/${id}`); }
  remove(id: number) {
    if (confirm('Delete warehouse?')) this.svc.delete(id).subscribe(_ => this.load());
  }


  toggleItems(w: WarehouseDto) {
    if (this.expandedId === w.id) { this.expandedId = null; this.items = []; return; }
    this.expandedId = w.id;
    this.fetchItems();
  }

  fetchItems() {
    if (this.expandedId == null) return;
    this.itemsSvc.listByWarehouse(this.expandedId).subscribe(r => {
      this.items = r.items;
      this.warehouseName = r.warehouseName;
    });
  }


  Add() {
    this.editingId = 0;
    this.itemForm.reset({ itemName: '', skuCode: '', qty: 0, costPrice: 0, msrpPrice: 0 });
  }

  Edit(id : number) {
    this.editingId = id;
    this.itemsSvc.getForEdit(id)
      .pipe(take(1))
      .subscribe(v => this.itemForm.setValue(v));
  }

  saveItem() {
    if (this.expandedId == null || this.itemForm.invalid) return;
    const payload = {
      warehouseId: this.expandedId,
      ...this.itemForm.getRawValue()
    };

    const save$ = this.editingId
      ? this.itemsSvc.update(this.editingId, payload)
      : this.itemsSvc.createForWarehouse(payload);

    save$.subscribe({
      next: _ => {
        this.fetchItems();
        (document.getElementById('itemModalClose') as HTMLButtonElement)?.click();
      },
      error: err => applyServerErrors(this.itemForm, err)
    });
  }

  removeItem(id: number) {
    if (!confirm('Delete item?')) return;
    this.itemsSvc.delete(id).subscribe(_ => this.fetchItems());
  }
}
