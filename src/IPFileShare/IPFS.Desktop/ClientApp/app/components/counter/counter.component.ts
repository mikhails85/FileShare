import { Component } from '@angular/core';
import { ViewService } from './../../services/view.service';
@Component({
    selector: 'counter',
    templateUrl: './counter.component.html'
})
export class CounterComponent {
    public currentCount = 0;

    constructor(public view: ViewService) { 
        view.pageTitle = "Fetch Data";
    }
    
    public incrementCounter() {
        this.currentCount++;
    }
}
