import { CommonModule, Location } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-back-button',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './back-button.component.html',
  styleUrl: './back-button.component.css'
})
export class BackButtonComponent {
  @Input() label = 'Back';

  @Input() fallbackUrl = '/';

  @Input() btnClass = 'btn-link';

  constructor(private location: Location, private router: Router) { }

  goBack() {
    if (window.history.length > 1) this.location.back();
    else this.router.navigateByUrl(this.fallbackUrl);
  }
}
