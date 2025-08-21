import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

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
  submitting = false; error = '';
  submit() {
    debugger;
    if (this.form.invalid) return;
    this.submitting = true;
    this.auth.login(this.form.value as any).subscribe({
      next: _ => this.router.navigateByUrl('/'),
      error: err => { this.error = err?.error?.message ?? 'Login failed'; this.submitting = false; }
    });
  }
}
