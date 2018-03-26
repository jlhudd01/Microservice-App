import { Component, OnInit } from '@angular/core';

import { OrderService } from './order.service';
import { IOrder } from '../Models/order.model';
import { Observable } from 'rxjs/Observable';

@Component({
    selector: 'order-items',
    styleUrls: ['./order.component.css'],
    templateUrl: './order.component.html'
})
export class OrderComponent implements OnInit {
    subTitle: string;
    orders: IOrder[];

    constructor(private _service: OrderService) {
        this.getOrders();
    }

    ngOnInit(): void {
        this.subTitle = 'Angular 5 Orders';
    }

    getOrders() {
        this._service.getOrders().subscribe(
            orders => this.orders = orders,
            err => this.handleError(err),
            () => console.log("getOrders completed")
        );
    }

    removeFromOrder(order, orderItem) {
        if (confirm('Are you sure you want to remove this item from your order?')) {
            this._service.removeFromOrder(order, orderItem).subscribe(
                data => {
                    this.getOrders();
                },
                err => this.handleError(err),
                () => console.log('removeFromOrder() completed')
            );
        }
    }

    private handleError(error: any) {
        return Observable.throw(error);
    } 
}