import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { v4 as uuid } from 'uuid';

@Injectable()
export class DataService {
    constructor(private http: HttpClient) { }

    get<T>(url: string, params?: any) : Observable<T> {
        return this.http.get<T>(url);
    }

    post(url: string, data: any, params?: any) {
        return this.http.post(url, data, { headers: this.CreateHeaders() });
    }

    put(url: string, data: any, params?: any) {
        return this.http.put(url, data, { headers: this.CreateHeaders() });
    }

    private handleError(error: any) {
        console.error('server error: ', error);
        return Observable.throw(error || 'server error');
    }

    private CreateHeaders() : HttpHeaders {
        let headers = new HttpHeaders();

        headers = headers.set('x-requestid', uuid());

        return headers;
    }
}