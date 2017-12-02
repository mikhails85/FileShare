import { Component, Input } from '@angular/core';
import {ContentInfo} from './../../../models';

@Component({
  selector: 'app-local-file-list',
  templateUrl: './app-local-file-list.component.html'
})
export class AppAppLocalFileListComponent {
  @Input() public contentList: Array<ContentInfo>;
  
   public humanFileSize(bytes: number):string {
    const thresh = 1024;
    const units = ['kB','MB','GB','TB','PB','EB','ZB','YB'];
    
    if(Math.abs(bytes) < thresh) {
        return bytes + ' B';
    }
    
    let u = -1;
    do {
        bytes /= thresh;
        ++u;
    } while(Math.abs(bytes) >= thresh && u < units.length - 1);
    
    return bytes.toFixed(1)+' '+units[u];
  }
}
