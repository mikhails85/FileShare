import { Injectable } from '@angular/core';

@Injectable()
export class ViewService {
    public manuToggleState : boolean = false;     
    public manuToggle = () => {
        this.manuToggleState = !this.manuToggleState;
    }
}