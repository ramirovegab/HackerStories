import { Component } from '@angular/core';
import { ITEMS_PER_PAGE } from '../config/local.config';
import { PaginatedStories } from '../models/paginatedStories';
import { StoryService } from '../services/story.service';

@Component({
  selector: 'app-stories',
  templateUrl: './stories.component.html',
  styleUrls: ['./stories.component.css']
})
export class StoriesComponent {
  
  stories: PaginatedStories = new PaginatedStories();
  itemsPerPage: number = ITEMS_PER_PAGE;
  searchText: string = '';
  constructor(private storyService: StoryService) {}

  ngOnInit(): void {
    this.getStories();
  }

  getStories(): void {
    this.storyService.getStories(this.stories.pageNumber, this.itemsPerPage, this.stories.searchText)
        .subscribe(stories => this.stories = stories);
  }

  pageChanged(pageNumber: number): void {
    this.stories.pageNumber = pageNumber;
    this.getStories(); 
  }

  getFiltered(): void {
    if (this.searchText !== this.stories.searchText) {
      this.stories.searchText = this.searchText;
      this.getStories();
    }
  }

  clearSearch(): void {
    this.searchText = '';
    this.getFiltered();
  }
}
