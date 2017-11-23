import { Component} from '@angular/core';
import { ViewService } from './../../services/view.service';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html'
})
export class NavMenuComponent {
     constructor(public view: ViewService) { }
}
