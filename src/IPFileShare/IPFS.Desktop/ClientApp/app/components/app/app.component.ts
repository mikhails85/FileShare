import { Component } from '@angular/core';
import { ViewService } from './../../services/view.service';

@Component({
    selector: 'app',
    templateUrl: './app.component.html'
})
export class AppComponent {
    constructor(public view: ViewService) { }
}
