import { Component } from '@angular/core';
import {ApiService} from './../../services/api.service';
import {ContentInfo} from './../../models';

@Component({
    templateUrl: './files.component.html'
})
export class FilesComponent {
    public contentList: Array<ContentInfo>;
    
    constructor(private api: ApiService) {
      this.contentList = new Array<ContentInfo>();
    }
    
    public refreshContentList()
    {
        
    }
}
