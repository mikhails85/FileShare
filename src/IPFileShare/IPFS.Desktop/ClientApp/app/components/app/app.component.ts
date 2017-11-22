import { Component } from '@angular/core';

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    //styleUrls: ['./app.component.css']
})
export class AppComponent {
    public toggle : boolean = false;

    public onManuToggle = ($event: boolean) => {
       
        this.toggle = $event;
    }
}
