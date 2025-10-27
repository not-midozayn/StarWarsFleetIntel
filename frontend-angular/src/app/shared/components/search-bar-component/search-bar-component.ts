import { Component, Output, EventEmitter, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-search-bar',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="search-bar">
      <input
        type="text"
        class="search-bar__input"
        [placeholder]="placeholder"
        [(ngModel)]="searchTerm"
        (input)="onSearchChange()"
        aria-label="Search"
      />
      <span class="search-bar__icon">🔍</span>
    </div>
  `,
  styleUrls: ['./search-bar-component.scss']
})
export class SearchBarComponent {
  @Output() search = new EventEmitter<string>();
  
  placeholder = 'Search...';
  searchTerm = signal('');
  private debounceTimer: any;

  constructor() {
    // Effect to handle debounced search
    effect(() => {
      const term = this.searchTerm();
      this.emitSearch(term);
    });
  }

  onSearchChange(): void {
    clearTimeout(this.debounceTimer);
    this.debounceTimer = setTimeout(() => {
      this.searchTerm.set((event!.target as HTMLInputElement).value);
    }, 300);
  }

  private emitSearch(term: string): void {
    this.search.emit(term);
  }
}