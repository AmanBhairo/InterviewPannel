import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import { map } from 'rxjs';

export const adminGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  return authService.getUsername().pipe(
    map(username => {
      if (username == "admin@gmail.com") {
        return true;
      } else {
        router.navigate(['/home']); // Redirect to a different route if not admin
        return false;
      }
    })
  );
};
