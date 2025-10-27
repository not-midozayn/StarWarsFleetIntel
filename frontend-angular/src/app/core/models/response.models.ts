export interface ApiResult<T> {
  succeeded: boolean;
  message: string;
  errors: string[];
  data: T | null;
}

export interface PaginatedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}