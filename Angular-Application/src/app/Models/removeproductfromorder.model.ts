import { IOrder } from  './order.model';
import { IOrderItem } from  './orderitem.model';

export class RemoveProductFromOrder {
    constructor(private order: IOrder, private orderitem: IOrderItem)
    {

    }
}