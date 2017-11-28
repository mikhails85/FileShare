import { Component } from '@angular/core';
import { ViewService } from './../../services/view.service';
@Component({
    templateUrl: './home.component.html'
})
export class HomeComponent {
     constructor(public view: ViewService) {
         view.pageTitle = "Home";
     }
}
