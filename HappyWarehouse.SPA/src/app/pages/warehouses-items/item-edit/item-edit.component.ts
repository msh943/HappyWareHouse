import { Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ItemsService } from '../../../services/items.service';
import { WarehousesService } from '../../../services/warehouses.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-item-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './item-edit.component.html',
  styleUrl: './item-edit.component.css'
})
export class ItemEditComponent {
  //id = 0;
  //warehouses: any[] = [];
  //form = this.fb.group({
  //  itemName: ['', Validators.required],
  //  skuCode: [''],
  //  qty: [0, Validators.required],
  //  costPrice: [0, Validators.required],
  //  msrpPrice: [0],
  //  warehouseId: [0, Validators.required]
  //});

  //constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private svc: ItemsService, private wh: WarehousesService) { }
  //ngOnInit() {
  //  this.id = +(this.route.snapshot.paramMap.get('id') || 0);
  //  this.wh.getAll(1, 1000).subscribe(r => this.warehouses = r.warehouses);
  //  if (this.id) this.svc.get(this.id).subscribe(x => this.form.patchValue(x));
  //}
  ////save() {
  ////  const payload = this.form.value;
  ////  (this.id ? this.svc.update(this.id, payload) : this.svc.create(payload))
  ////    .subscribe(_ => this.router.navigateByUrl('/items'));
  ////}
}
