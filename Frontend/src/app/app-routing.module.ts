import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { HomeComponent } from './pages/home/home.component';
import { RegisterComponent } from './pages/register/register.component';
import { MyProductsComponent } from './pages/my-products/my-products.component';

const routes: Routes = [ 
  { path: '', component: HomeComponent,},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path:'my-products', component: MyProductsComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
