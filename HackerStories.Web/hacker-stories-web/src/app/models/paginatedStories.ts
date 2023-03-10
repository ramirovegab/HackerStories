import { Story } from "./story";

export class PaginatedStories {

    items: ReadonlyArray<Story>;
    pageNumber: number;
    totalPages: number;
    totalCount: number;
    hasNextPage: boolean;
    hasPreviousPage: boolean;
    searchText: string;
    
    constructor() {
        this.items = [];
        this.pageNumber = 1;
        this.totalCount = 0;
        this.totalPages = 0;
        this.hasNextPage = false;
        this.hasPreviousPage = false;
        this.searchText = '';
    }
}