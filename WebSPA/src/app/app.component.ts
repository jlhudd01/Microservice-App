import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Angular Test App';
  tabLinks = [ { label: 'Products', link: 'product' },
               { label: 'Orders', link: 'order' }];
}
