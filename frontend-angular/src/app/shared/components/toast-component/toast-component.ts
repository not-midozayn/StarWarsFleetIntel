import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastService, Toast } from '../../../core/services/toast';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './toast-component.html',
  styleUrls: ['./toast-component.scss']
})
export class ToastComponent {
  private readonly toastService = inject(ToastService);
  toasts = this.toastService.toasts;

  close(id: string): void {
    this.toastService.remove(id);
  }

  getIcon(type: Toast['type']): string {
    switch (type) {
      case 'success': return '✓';
      case 'error': return '✗';
      case 'warning': return '⚠';
      case 'info': return 'ℹ';
      default: return '';
    }
  }
}