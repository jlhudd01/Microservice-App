import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'
import { MatCardModule, MatButtonModule } from '@angular/material';

import { OrderComponent } from './order.component';
import { OrderService } from './order.service';
import { SharedModule } from '../Shared/shared.module';

@NgModule({
    imports: [ BrowserModule, FormsModule, MatCardModule, MatButtonModule, SharedModule ],
    declarations: [ OrderComponent ],
    providers: [ OrderService ]
})
export class OrderModule { }