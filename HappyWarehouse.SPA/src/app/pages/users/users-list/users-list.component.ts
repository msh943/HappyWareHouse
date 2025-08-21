import { Component, OnInit } from '@angular/core';
import { UserDto } from '../../../models/user.dto';
import { UsersService } from '../../../services/users.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-users-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './users-list.component.html',
  styleUrl: './users-list.component.css'
})
export class UsersListComponent implements OnInit {
  rows: UserDto[] = [];
  constructor(private svc: UsersService, private router: Router) { }
  ngOnInit() { this.load(); }
  load() { this.svc.list().subscribe(r => this.rows = r.users); }
  add() { this.router.navigateByUrl('/users/new'); }
  edit(id: number) { this.router.navigateByUrl(`/users/edit/${id}`); }
  remove(id: number) { if (confirm('Delete user?')) this.svc.delete(id).subscribe(_ => this.load()); }
}
