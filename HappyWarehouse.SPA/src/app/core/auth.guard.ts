import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = () => {
  const token = localStorage.getItem('jwt');
  if (token) return true;
  inject(Router).navigateByUrl('/login');
  return false;
};
