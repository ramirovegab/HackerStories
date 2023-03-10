import { HttpClient, HttpHandler } from '@angular/common/http';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { PaginatedStories } from '../models/paginatedStories';
import { StoryService } from '../services/story.service';
import { HttpTestingController } from '@angular/common/http/testing';

import { StoriesComponent } from './stories.component';
import { Type } from '@angular/core';
import { NgxPaginationModule } from 'ngx-pagination';
import { FormsModule } from '@angular/forms';

describe('StoriesComponent', () => {
  let component: StoriesComponent;
  let fixture: ComponentFixture<StoriesComponent>;
  let storiesService: StoryService;
  let data: PaginatedStories;
  let httpClient: HttpClient;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        NgxPaginationModule,
        FormsModule
     ],
      declarations: [ StoriesComponent ],
      providers: [StoryService, HttpTestingController, HttpClient, HttpHandler]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StoriesComponent);
    component = fixture.componentInstance;
    fixture.debugElement.injector.get<HttpTestingController>(HttpTestingController as Type<HttpTestingController>)
    fixture.detectChanges();

    httpClient = TestBed.inject(HttpClient);
    storiesService = TestBed.inject(StoryService);
    data = new PaginatedStories();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should get stories', () => {
    spyOn(storiesService, 'getStories').and.returnValue(of(data))
    component.getStories();
    expect(component.stories).toBe(data);
    expect(storiesService.getStories).toHaveBeenCalled();
  });
});
