import { DataService } from './data.service';
import { NgModule, ModuleWithProviders } from '@angular/core';

@NgModule({
    imports: [],
    declarations:[],
    exports: []
})

export class SharedModule {
    static forRoot() : ModuleWithProviders {
        return {
            ngModule: SharedModule,
            providers: [
                DataService
            ]
        };
    }
}