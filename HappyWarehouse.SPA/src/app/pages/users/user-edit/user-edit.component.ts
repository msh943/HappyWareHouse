import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LookupsService } from '../../../services/lookups.service';
import { UsersService } from '../../../services/users.service';
import { applyServerErrors } from '../../../core/server-validate';
import { BackButtonComponent } from '../../../shared/back-button/back-button.component';

@Component({
  selector: 'app-user-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, BackButtonComponent],
  templateUrl: './user-edit.component.html',
  styleUrl: './user-edit.component.css'
})
export class UserEditComponent implements OnInit {
  id = 0;
  roles: any[] = [];
  readonly passwordPattern = /^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$/;
  saving: boolean = false;

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    fullName: ['', Validators.required],
    roleId: [0, [Validators.required, Validators.min(1)]],
    isActive: [true, Validators.required],
    password: ['']  
  });

  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private users: UsersService, private lookups: LookupsService) { }
  ngOnInit() {
    this.id = +(this.route.snapshot.paramMap.get('id') || 0);
    const passwordControl = this.form.controls.password;
    if (this.id === 0) {
      passwordControl.setValidators([Validators.required, Validators.pattern(this.passwordPattern)]);
    } else {
      passwordControl.setValidators([Validators.pattern(this.passwordPattern)]);
    }
    passwordControl.updateValueAndValidity({ emitEvent: false });
    this.lookups.roles().subscribe(r => this.roles = r);
    if (this.id) this.users.get(this.id).subscribe(u => this.form.patchValue(u));
  }
  save() {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.saving = true;
    const rawValues = this.form.getRawValue();
    if (this.id === 0) {

      const payload = {
        email: rawValues.email,
        fullName: rawValues.fullName,
        roleId: rawValues.roleId,
        isActive: rawValues.isActive,
        password: rawValues.password
      };
      this.users.create(payload).subscribe({
        next: _ => this.router.navigateByUrl('/users'),
        error: err => applyServerErrors(this.form, err)
      }).add(() => this.saving = false);
    } else {
      const payload: any = {
        email: rawValues.email,
        fullName: rawValues.fullName,
        roleId: rawValues.roleId,
        isActive: rawValues.isActive
      };
      if (rawValues.password?.trim()) {
        payload.password = rawValues.password;
      }

      this.users.update(this.id, payload).subscribe({
        next: _ => this.router.navigateByUrl('/users'),
        error: err => applyServerErrors(this.form, err)
      }).add(() => this.saving = false);
    }
  }
}
