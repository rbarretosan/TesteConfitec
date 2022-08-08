import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DashboardComponent } from './components/dashboard/dashboard.component';

import { UsuarioDetalheComponent } from './components/usuarios/usuario-detalhe/usuario-detalhe.component';
import { UsuarioListaComponent } from './components/usuarios/usuario-lista/usuario-lista.component';
import { UsuariosComponent } from './components/usuarios/usuarios.component';

const routes: Routes = [

  { path: 'usuarios', redirectTo: 'usuarios/lista' },
  {
    path: 'usuarios', component: UsuariosComponent,
    children: [
      { path: 'detalhe/:id', component: UsuarioDetalheComponent },
      { path: 'detalhe', component: UsuarioDetalheComponent },
      { path: 'lista', component: UsuarioListaComponent },
    ],
  },

  { path: 'dashboard', component: DashboardComponent },
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: '**', redirectTo: 'dashboard', pathMatch: 'full' },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
