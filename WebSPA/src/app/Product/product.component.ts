import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { IProduct } from '../Models/product.model';
import { ProductService } from './product.service';
import { Observable } from 'rxjs/Observable';

@Component({
    selector: 'product-items',
    styleUrls: ['./product.component.css'],
    templateUrl: './product.component.html'
})
export class ProductComponent implements OnInit {
    public products: IProduct[];
    subTitle: string = '';
    public product_name: string;
    public product_price: string;
    one$;

    constructor(private _service: ProductService) { 
        this.loadData();
        this.subTitle = "Angular 5 Products";
    }

    ngOnInit(){
        
    }

    loadData() {
        this.getProducts();
    }

    getProducts() {
        this.one$ = this._service.getProducts()
        .subscribe(
            products => { this.products = products },
            err => this.handleError(err),
            () => console.log('getProducts() complete')
        );
    }

    createProduct(name, price) {
        let product = {name: name, price: price };
        this._service.createProduct(product).subscribe(
            data => {
                // refresh the list
                this.getProducts();
                this.product_name = '';
                this.product_price = '';
                return true;
            },
            error => {
                console.error("Error saving food!");
                this.handleError(error);
            },
            () => console.log('createProduct() complete')
        );
    }

    updateProduct(product) {
        this._service.updateProduct(product).subscribe(
            data => {
                // refresh the list
                this.getProducts();
                return true;
            },
            error => {
                console.error("Error saving food!");
                this.handleError(error);
            },
            () => console.log('updateProduct() complete')
        );
    }
        
    deleteProduct(product) {
        if (confirm("Are you sure you want to delete " + product.name + "?")) {
            this._service.deleteProduct(product).subscribe(
                data => {
                    // refresh the list
                    this.getProducts();
                    return true;
                },
                error => {
                    console.error("Error deleting food!");
                    this.handleError(error);
                },
                () => console.log('deleteProduct() complete')
            );
        }
    }

    private handleError(error: any) {
        console.error(error);
        return Observable.throw(error);
    } 
}