import { Component, Output, EventEmitter} from '@angular/core';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
//    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {
    @Output() onManuToggle = new EventEmitter<boolean>();
    public toggle : boolean = false;
    
    public ManuToggle = () => {
        this.toggle = !this.toggle;
        this.onManuToggle.emit(this.toggle)
    }
}
