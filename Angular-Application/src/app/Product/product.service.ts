import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

import { IProduct } from '../Models/product.model';
import { DataService }  from '../Shared/data.service';
// const httpOptions = {
//     headers: new HttpHeaders({ 'Content-Type': 'application/json' })
// };

@Injectable()
export class ProductService {
    private productUrl: string = '';

    constructor(private data: DataService){
        this.productUrl = 'http://localhost:5000/api/product/';
    }

    getProducts() : Observable<IProduct[]> {
        return this.data.get<IProduct[]>(this.productUrl + 'products');
    }

    createProduct(product) {
        return this.data.post(this.productUrl + 'PostProduct', product);
    }

    updateProduct(product) {
        return this.data.put(this.productUrl + 'Put', product);
    }
        
    deleteProduct(product) {
        return this.data.post(this.productUrl + 'Delete', product);
    }

    private handleError(error: any) {
        console.error('server error: ', error);
        return Observable.throw(error || 'server error');
    }
}