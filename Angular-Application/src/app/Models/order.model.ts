import { IOrderItem } from './orderitem.model';

export interface IOrder {
    id: number,
    orderItems: IOrderItem[]
}