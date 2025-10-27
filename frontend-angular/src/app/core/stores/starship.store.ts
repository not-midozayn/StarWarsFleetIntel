import { Injectable, signal, computed, inject } from '@angular/core';
import { StarshipApiService } from '../services/starship-api';
import { Starship, Currency, ModificationType } from '../models/starship.model';
import { PaginatedResult } from '../models/response.models';
import { environment } from '../../../environments/environment.development';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { debounceTime, distinctUntilChanged, switchMap, catchError, of } from 'rxjs';

interface StarshipState {
  starships: Starship[];
  selectedStarship: Starship | null;
  pagination: {
    currentPage: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
  };
  filters: {
    searchTerm: string;
    currency: Currency | null;
    modifications: ModificationType[];
  };
  loading: boolean;
  error: string | null;
}

@Injectable({
  providedIn: 'root'
})
export class StarshipStore {
  private readonly apiService = inject(StarshipApiService);

  // State signals
  private readonly state = signal<StarshipState>({
    starships: [],
    selectedStarship: null,
    pagination: {
      currentPage: 1,
      pageSize: 10,
      totalCount: 0,
      totalPages: 0
    },
    filters: {
      searchTerm: '',
      currency: null,
      modifications: []
    },
    loading: false,
    error: null
  });

  // Selectors (computed signals)
  public readonly starships = computed(() => this.state().starships);
  public readonly selectedStarship = computed(() => this.state().selectedStarship);
  public readonly pagination = computed(() => ({...this.state().pagination,
  hasNext: this.state().pagination.currentPage < this.state().pagination.totalPages,
  hasPreviousPage: this.state().pagination.currentPage > 1
}));

  public readonly filters = computed(() => this.state().filters);
  public readonly loading = computed(() => this.state().loading);
  public readonly error = computed(() => this.state().error);

  public readonly filteredStarships = computed(() => {
    const { searchTerm } = this.state().filters;
    const starships = this.state().starships;

    if (!searchTerm) {
      return starships;
    }

    const searchTermLower = searchTerm.toLowerCase();
    return starships.filter(starship =>
      starship.name.toLowerCase().includes(searchTermLower) ||
      starship.model.toLowerCase().includes(searchTermLower) ||
      starship.manufacturer.toLowerCase().includes(searchTermLower)
    );
  });

  // Actions
  loadStarships(page: number = 1): void {
    this.state.update(state => ({ ...state, loading: true, error: null }));

    const { currency, modifications } = this.state().filters;
    this.apiService.getStarships(
      page, 
      this.state().pagination.pageSize,
      currency || undefined,
      modifications.length > 0 ? modifications : undefined
    ).subscribe({
        next: (result: PaginatedResult<Starship>) => {
          this.state.update(state => ({
            ...state,
            starships: result.items,
            pagination: {
              currentPage: result.pageNumber,
              pageSize: result.pageSize,
              totalCount: result.totalCount,
              totalPages: result.totalPages
            },
            loading: false
          }));
        },
        error: (error) => {
          this.state.update(state => ({
            ...state,
            loading: false,
            error: error.message || 'Failed to load starships'
          }));
        }
      });
  }

  loadStarshipDetails(
    id: number,
    currency?: Currency,
    modifications?: ModificationType[]
  ): void {
    this.state.update(state => ({ ...state, loading: true, error: null }));

    this.apiService.getStarship(id, currency, modifications)
      .subscribe({
        next: (starship: Starship) => {
          // Ensure the starship has an ID
          const enhancedStarship = {
            ...starship,
            id,
            url: `${environment.apiUrl}/starships/${id}`
          };
          
          this.state.update(state => ({
            ...state,
            selectedStarship: enhancedStarship,
            loading: false
          }));
        },
        error: (error) => {
          this.state.update(state => ({
            ...state,
            loading: false,
            error: error.message || 'Failed to load starship details'
          }));
        }
      });
  }

  setSearchTerm(searchTerm: string): void {
    this.state.update(state => ({
      ...state,
      filters: { ...state.filters, searchTerm }
    }));
  }

  setCurrency(currency: Currency | null): void {
    this.state.update(state => ({
      ...state,
      filters: { ...state.filters, currency }
    }));
    // After updating currency, reload starships with new currency
    this.loadStarships(this.state().pagination.currentPage);
  }

  toggleModification(modification: ModificationType): void {
    this.state.update(state => {
      const modifications = state.filters.modifications.includes(modification)
        ? state.filters.modifications.filter(m => m !== modification)
        : [...state.filters.modifications, modification];

      return {
        ...state,
        filters: { ...state.filters, modifications }
      };
    });
  }

  clearSelectedStarship(): void {
    this.state.update(state => ({
      ...state,
      selectedStarship: null
    }));
  }

  nextPage(): void {
    const currentPage = this.state().pagination.currentPage;
    const totalPages = this.state().pagination.totalPages;

    if (currentPage < totalPages) {
      this.loadStarships(currentPage + 1);
    }
  }

  previousPage(): void {
    const currentPage = this.state().pagination.currentPage;

    if (currentPage > 1) {
      this.loadStarships(currentPage - 1);
    }
  }
}