import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

import { IOrder } from '../Models/order.model';
import {RemoveProductFromOrder } from '../Models/removeproductfromorder.model'

@Injectable()
export class OrderService {
    private orderUrl: string = '';

    constructor(private _http: HttpClient){
        this.orderUrl = 'http://localhost:5050/api/order/';
    }

    getOrders() : Observable<IOrder[]> {
        return this._http.get<IOrder[]>(this.orderUrl + 'orders');
    }

    // createProduct(product) {
    //     let body = JSON.stringify(product);
    //     return this._http.post(this.productUrl + 'PostProduct', body, httpOptions);
    // }

    // updateProduct(product) {
    //     let body = JSON.stringify(product);
    //     return this._http.put(this.productUrl + 'Put/' + product.id, body, httpOptions);
    // }
        
    removeFromOrder(order, orderitem) {
        return this._http.post(this.orderUrl + 'Delete', new RemoveProductFromOrder(order, orderitem));
    }

    private handleError(error: any) {
        console.error('server error: ', error);
        return Observable.throw(error || 'server error');
    }
}