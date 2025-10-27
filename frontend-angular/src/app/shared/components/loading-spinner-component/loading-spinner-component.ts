import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-loading-spinner',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="loading-spinner">
      <div class="loading-spinner__spinner"></div>
      @if (message) {
        <p class="loading-spinner__message">{{ message }}</p>
      }
    </div> 
  `,
  styleUrls: ['./loading-spinner-component.scss']
})
export class LoadingSpinnerComponent {
  @Input() message: string = 'Loading...';
}