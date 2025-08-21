import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LookupsService } from '../../../services/lookups.service';
import { WarehousesService } from '../../../services/warehouses.service';
import { CommonModule } from '@angular/common';
import { applyServerErrors } from '../../../core/server-validate';
import { BackButtonComponent } from '../../../shared/back-button/back-button.component';

@Component({
  selector: 'app-warehouse-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, BackButtonComponent],
  templateUrl: './warehouse-edit.component.html',
  styleUrl: './warehouse-edit.component.css'
})
export class WarehouseEditComponent implements OnInit {
  id = 0;
  countries: any[] = [];
  saving = false;
  form = this.fb.group({
    name: ['', Validators.required],
    address: ['', [Validators.required]],
    city: ['', [Validators.required]],
    countryId: [0, [Validators.required, Validators.min(1)]]
  });
  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private svc: WarehousesService, private lookups: LookupsService) { }
  ngOnInit() {
    this.id = +(this.route.snapshot.paramMap.get('id') || 0);
    this.lookups.countries().subscribe(r => this.countries = r);
    if (this.id) this.svc.get(this.id).subscribe(w => this.form.patchValue(w));
  }
  save() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }

    this.saving = true;
    const rawValues = this.form.getRawValue();
    const req$ = this.id ? this.svc.update(this.id, rawValues) : this.svc.create(rawValues);
    req$.subscribe({
      next: _ => this.router.navigateByUrl('/warehouses'),
      error: err => applyServerErrors(this.form, err)
    }).add(() => this.saving = false);
  }
}
