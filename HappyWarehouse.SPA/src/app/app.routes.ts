import { Routes } from '@angular/router';
import { authGuard } from './core/auth.guard';

export const routes: Routes = [
  { path: '', loadComponent: () => import('./pages/welcome/welcome.component').then(m => m.WelcomeComponent), canActivate: [authGuard] },
  { path: 'login', loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent) },


  { path: 'dashboard', loadComponent: () => import('./pages/dashboard/dashboard.component').then(m => m.DashboardComponent), canActivate: [authGuard] },

  { path: 'warehouses', loadComponent: () => import('./pages/warehouses/warehouses-list/warehouses-list.component').then(m => m.WarehousesListComponent), canActivate: [authGuard] },
  { path: 'warehouses/new', loadComponent: () => import('./pages/warehouses/warehouse-edit/warehouse-edit.component').then(m => m.WarehouseEditComponent), canActivate: [authGuard] },
  { path: 'warehouses/edit/:id', loadComponent: () => import('./pages/warehouses/warehouse-edit/warehouse-edit.component').then(m => m.WarehouseEditComponent), canActivate: [authGuard] },


  { path: 'items', loadComponent: () => import('./pages/warehouses-items/items-list/items-list.component').then(m => m.ItemsListComponent), canActivate: [authGuard] },
  { path: 'items/new', loadComponent: () => import('./pages/warehouses-items/item-edit/item-edit.component').then(m => m.ItemEditComponent), canActivate: [authGuard] },
  { path: 'items/edit/:id', loadComponent: () => import('./pages/warehouses-items/item-edit/item-edit.component').then(m => m.ItemEditComponent), canActivate: [authGuard] },


  { path: 'users', loadComponent: () => import('./pages/users/users-list/users-list.component').then(m => m.UsersListComponent), canActivate: [authGuard] },
  { path: 'users/new', loadComponent: () => import('./pages/users/user-edit/user-edit.component').then(m => m.UserEditComponent), canActivate: [authGuard] },
  { path: 'users/edit/:id', loadComponent: () => import('./pages/users/user-edit/user-edit.component').then(m => m.UserEditComponent), canActivate: [authGuard] },

  { path: 'logs', loadComponent: () => import('./pages/logs-view/logs-view.component').then(m => m.LogsViewComponent), canActivate: [authGuard] },
  { path: '**', redirectTo: '' }
];
