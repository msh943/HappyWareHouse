import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LookupsService } from '../../../services/lookups.service';
import { UsersService } from '../../../services/users.service';

@Component({
  selector: 'app-user-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './user-edit.component.html',
  styleUrl: './user-edit.component.css'
})
export class UserEditComponent implements OnInit {
  id = 0;
  roles: any[] = [];
  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    fullName: ['', Validators.required],
    roleId: [0, Validators.required],
    isActive: [true, Validators.required],
    password: ['', Validators.required]  
  });

  constructor(private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private users: UsersService, private lookups: LookupsService) { }
  ngOnInit() {
    this.id = +(this.route.snapshot.paramMap.get('id') || 0);
    this.lookups.roles().subscribe(r => this.roles = r);
    if (this.id) this.users.get(this.id).subscribe(u => this.form.patchValue(u));
  }
  save() {
    const payload = this.form.value;
    (this.id ? this.users.update(this.id, payload) : this.users.create(payload))
      .subscribe(_ => this.router.navigateByUrl('/users'));
  }
}
