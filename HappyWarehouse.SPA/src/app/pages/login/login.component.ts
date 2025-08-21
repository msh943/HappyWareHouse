import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { applyServerErrors } from '../../core/server-validate';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router) { }
  form = this.fb.group({ email: ['', [Validators.required, Validators.email]], password: ['', [Validators.required]] });
  submitting = false;
  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.submitting = true;
    this.auth.login(this.form.value as any).subscribe({
      next: _ => this.router.navigateByUrl('/'),
      error: err => applyServerErrors(this.form, err)
    }).add(() => this.submitting = false);
  }
}
