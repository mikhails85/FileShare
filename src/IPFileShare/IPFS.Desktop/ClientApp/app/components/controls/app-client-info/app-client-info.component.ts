import { Component } from '@angular/core';
import {ApiService} from './../../../services/api.service';
import {ClientInfo} from './../../../models';
@Component({
  selector: 'app-client-info',
  templateUrl: './app-client-info.component.html'
})
export class AppClientInfoComponent {
  public clientInfo: ClientInfo;
  
  constructor(api: ApiService) {
      this.clientInfo = new ClientInfo();
      
      api.getClientInfo().subscribe(
        (data) => {
          this.clientInfo = data;
        }
      );
  }
  
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
