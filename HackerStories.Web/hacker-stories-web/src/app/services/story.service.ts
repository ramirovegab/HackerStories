import { Injectable } from '@angular/core';
import { catchError, Observable, of } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { PaginatedStories } from '../models/paginatedStories';
import { API_BASE_URL } from '../config/local.config';

@Injectable({
  providedIn: 'root'
})
export class StoryService {

  constructor(private httpClient: HttpClient) { }

  getStories(pageNumber: number, pageSize: number, searchText: string): Observable<PaginatedStories> {
    return this.httpClient.get<PaginatedStories>(this.buildStoriesUrl(pageNumber, pageSize, searchText)).pipe(
      catchError((error) => { 
        console.error(error); 
        return of(new PaginatedStories());
      } )
    );
  }

  buildStoriesUrl(pageNumber: number, pageSize: number, searchText: string): string {
    let url = `${API_BASE_URL}pageNumber=${pageNumber}&pageSize=${pageSize}`;
    if (searchText !== '') {
      url += `&searchText=${searchText}`;
    }
    return url;
  }
}
