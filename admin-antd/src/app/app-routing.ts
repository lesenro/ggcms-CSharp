import {NgModule, Inject} from '@angular/core';
import {CommonModule} from '@angular/common';
import {
    RouterModule,
    Routes,
    Router,
    NavigationEnd,
    NavigationStart,
    ActivatedRoute
} from '@angular/router';
import {LoginComponent} from "./routes";
let routes : Routes = [
    {
        path: '',
        redirectTo: '/login',
        pathMatch: 'full'
    }, {
        path: 'login',
        component: LoginComponent,
        data: {
            "title": "登录"
        }
    }
];

@NgModule({
    imports: [
        CommonModule, RouterModule.forRoot(routes)
    ],
    exports: [RouterModule],
    declarations: []
})
export class AppRoutingModule {
    constructor(private router : Router, private actRoute : ActivatedRoute) {}
}