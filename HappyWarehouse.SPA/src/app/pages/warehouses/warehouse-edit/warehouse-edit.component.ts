import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LookupsService } from '../../../services/lookups.service';
import { WarehousesService } from '../../../services/warehouses.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-warehouse-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './warehouse-edit.component.html',
  styleUrl: './warehouse-edit.component.css'
})
export class WarehouseEditComponent implements OnInit {
  id = 0;
  countries: any[] = [];
  form = this.fb.group({
    name: ['', Validators.required],
    address: [''],
    city: [''],
    countryId: [0, Validators.required]
  });
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private svc: WarehousesService, private lookups: LookupsService) { }
  ngOnInit() {
    this.id = +(this.route.snapshot.paramMap.get('id') || 0);
    this.lookups.countries().subscribe(r => this.countries = r);
    if (this.id) this.svc.get(this.id).subscribe(w => this.form.patchValue(w));
  }
  save() {
    const payload = this.form.value;
    (this.id ? this.svc.update(this.id, payload) : this.svc.create(payload))
      .subscribe(_ => this.router.navigateByUrl('/warehouses'));
  }
}
