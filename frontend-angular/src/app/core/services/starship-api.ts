import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { Starship, Currency, ModificationType } from '../models/starship.model';
import { PaginatedResult, ApiResult } from '../models/response.models';

@Injectable({
  providedIn: 'root'
})
export class StarshipApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/starships`;

  getStarship(
    id: number,
    currency?: Currency,
    modifications?: ModificationType[],
    runPreFlightChecks: boolean = true
  ): Observable<Starship> {
    let params = new HttpParams()
      .set('runPreFlightChecks', runPreFlightChecks.toString());

    if (currency) {
      params = params.set('currency', currency);
    }

    if (modifications && modifications.length > 0) {
      params = params.set('modifications', modifications.join(','));
    }

    return this.http
      .get<ApiResult<Starship>>(`${this.baseUrl}/${id}`, { params })
      .pipe(map(result => {
        if (!result.data) throw new Error('Starship not found');
        return result.data;
      }));
  }

  getStarships(
    page: number = 1,
    pageSize: number = 10,
    currency?: Currency,
    modifications?: ModificationType[]
  ): Observable<PaginatedResult<Starship>> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    
    if (currency) {
      params = params.set('currency', currency);
    }

    if (modifications && modifications.length > 0) {
      params = params.set('modifications', modifications.join(','));
    }

    return this.http
      .get<ApiResult<PaginatedResult<Starship>>>(this.baseUrl, { params })
      .pipe(
        map(result => {
          // If there's no data or empty items array
          if (!result.data || result.data.items.length === 0) {
            return {
              items: [],
              pageNumber: page,
              pageSize: pageSize,
              totalCount: 0,
              totalPages: 0,
              hasPreviousPage: page > 1,
              hasNextPage: false
            };
          }

          // Add ID and URL to each starship
          const enhancedItems = result.data.items.map(starship => {
            // Handle both url and Url
            const originalUrl = starship.url || (starship as any).Url;

            if (!originalUrl) {
              console.error('No URL found for starship:', starship);
              return {
                ...starship,
                id: 0,
                url: `${this.baseUrl}/0`
              };
            }

            const urlMatch = originalUrl.match(/\/(\d+)\/?$/);
            const id = urlMatch ? parseInt(urlMatch[1]) : 0;

            return {
              ...starship,
              id,
              url: `${this.baseUrl}/${id}`
            };
          });

          // If we got fewer items than the page size, we're on the last page
          const isLastPage = enhancedItems.length < pageSize;
          const totalPages = isLastPage ? page : result.data.totalPages;

          return {
            ...result.data,
            items: enhancedItems,
            totalPages: totalPages,
            hasNextPage: !isLastPage && page < totalPages,
            hasPreviousPage: page > 1
          };
        })
      );
  }
}