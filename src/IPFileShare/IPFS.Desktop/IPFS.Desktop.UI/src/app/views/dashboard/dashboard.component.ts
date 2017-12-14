import { Component } from '@angular/core';
import { Router } from '@angular/router';
import {ElectronService} from 'ngx-electron';

@Component({
  templateUrl: 'dashboard.component.html'
})
export class DashboardComponent {

  public eventValue: string;

  constructor(private _electronService: ElectronService) {
    this.eventValue = "Hello World!";
    //if(this._electronService.ipcRenderer) {
      this._electronService.ipcRenderer.on('App:SeyHello', (event, arg)=> {
        this.eventValue = arg;
      });
      this._electronService.ipcRenderer.send('UI:SeyHello');
    //}
  }
}
