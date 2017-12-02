import { Component, Output, EventEmitter } from '@angular/core';
import {ApiService} from './../../../services/api.service';
import {ContentManifestItem} from './../../../models';

@Component({
  selector: 'app-add-file-form',
  templateUrl: './app-add-file-form.component.html'
})
export class AppAddFileFormComponent {
  public content: ContentManifestItem;
  @Output() onAdded = new EventEmitter();  
  
  constructor(private api: ApiService) {
      this.content = new ContentManifestItem();
  }
  
  public addContent()
  {
    this.onAdded.emit(); 
  }
}
