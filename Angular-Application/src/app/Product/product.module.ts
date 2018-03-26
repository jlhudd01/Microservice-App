import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'
import { MatCardModule, MatInputModule, MatFormFieldModule, MatButtonModule } from '@angular/material';

import { ProductComponent } from './product.component';
import { ProductService } from './product.service';

@NgModule({
    imports: [ BrowserModule, FormsModule, MatCardModule, MatInputModule, MatFormFieldModule, MatButtonModule ],
    declarations: [ ProductComponent ],
    providers: [ ProductService ]
})
export class ProductModule { }