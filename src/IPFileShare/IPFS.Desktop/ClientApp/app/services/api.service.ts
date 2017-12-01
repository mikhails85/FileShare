import { Injectable, Inject } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

import {
  ClientInfo,
  Result,
  VoidResult
} from '../models';

@Injectable()
export class ApiService {
    
    private baseUrl: string;
    private onErrorEvent = new Subject<VoidResult>();
    
    public get OnErrorEvent(): Observable<VoidResult> { return this.onErrorEvent.asObservable(); }
    
    constructor(private http: Http, @Inject('BASE_URL') baseUrl: string) 
    {
      this.baseUrl = baseUrl;
    }
   
    public getClientInfo(): Observable<ClientInfo> 
    {
        return this.http.get(this.baseUrl + 'api/client/info')
                .map(response => {
                  if(!this.validResponse(response))
                  {
                      return this.handleError(response);
                  }
                  
                  const result = response.json() as Result<ClientInfo>;
                  if(result == null || !result.success)
                  {
                      return this.handleError(result);
                  }
                  return result.value;
                })
                .catch(this.handleError);
    }

    private handleError (error: Response | any) {
      console.error('ApiService::handleError', error);
      
      this.tiggerErrorEvent(error);
      
      return Observable.throw(error);
    }
    
    private tiggerErrorEvent(error: Response | any)
    {
        let result : VoidResult; 
        if(this.validResponse(error))
        {
          result = error.json() as VoidResult;
        }
        else 
        {
          result = error as VoidResult;
        }
        
        if(result)
        {
          this.onErrorEvent.next(error);
        }
    }
    
    private validResponse(resp: Response):boolean {
       const contentType = resp.headers.get('Content-type');
       if (contentType.indexOf('application/json') !== -1 ) {
        return true;   
       }
       return false;
    }
} 