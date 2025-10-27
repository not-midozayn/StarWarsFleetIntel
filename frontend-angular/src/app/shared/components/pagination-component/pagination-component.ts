import { Component, Input, Output, EventEmitter, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pagination-component.html',
  styleUrls: ['./pagination-component.scss']
})
export class PaginationComponent {
  @Input({ required: true }) currentPage!: number;
  @Input({ required: true }) totalPages!: number;
  @Input() hasNext: boolean = false;
  @Input() hasPrevious: boolean = false;

  @Output() pageChange = new EventEmitter<number>();
  @Output() nextPage = new EventEmitter<void>();
  @Output() previousPage = new EventEmitter<void>();

  get pages(): number[] {
    const pages: number[] = [];
    const maxPagesToShow = 5;
    const halfPages = Math.floor(maxPagesToShow / 2);

    let startPage = Math.max(1, this.currentPage - halfPages);
    let endPage = Math.min(this.totalPages, startPage + maxPagesToShow - 1);

    if (endPage - startPage < maxPagesToShow - 1) {
      startPage = Math.max(1, endPage - maxPagesToShow + 1);
    }

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    return pages;
  }

  onPageClick(page: number): void {
    if (page !== this.currentPage && page >= 1 && page <= this.totalPages) {
      this.pageChange.emit(page);
    }
  }

  onPreviousClick(): void {
    if (this.hasPrevious) {
      this.previousPage.emit();
    }
  }

  onNextClick(): void {
    if (this.hasNext) {
      this.nextPage.emit();
    }
  }
}