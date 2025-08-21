import { trigger, transition, style, animate, query, stagger } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-welcome',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './welcome.component.html',
  styleUrl: './welcome.component.css',
  animations: [
    trigger('fadeUp', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(12px)' }),
        animate('600ms ease-out', style({ opacity: 1, transform: 'translateY(0)' }))
      ])
    ]),
    trigger('staggerCards', [
      transition(':enter', [
        query('.wel-card', [
          style({ opacity: 0, transform: 'translateY(14px)' }),
          stagger(130, animate('550ms 100ms ease-out',
            style({ opacity: 1, transform: 'translateY(0)' })
          ))
        ], { optional: true })
      ])
    ])
  ]
})
export class WelcomeComponent {
  get greeting(): string {
    const h = new Date().getHours();
    if (h < 12) return 'Good morning';
    if (h < 18) return 'Good afternoon';
    return 'Good evening';
  }
}
