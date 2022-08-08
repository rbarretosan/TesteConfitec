import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Usuario } from '@app/models/Usuario';
import { UsuarioService } from '@app/services/usuario.service';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-usuario-lista',
  templateUrl: './usuario-lista.component.html',
  styleUrls: ['./usuario-lista.component.scss'],
})
export class UsuarioListaComponent implements OnInit {
  modalRef: BsModalRef;
  public usuarios: Usuario[] = [];
  public usuariosFiltrados: Usuario[] = [];
  public usuarioId = 0;

  public larguraImagem = 150;
  public margemImagem = 2;
  public exibirImagem = true;
  private filtroListado = '';

  public get filtroLista(): string {
    return this.filtroListado;
  }

  public set filtroLista(value: string) {
    this.filtroListado = value;
    this.usuariosFiltrados = this.filtroLista
      ? this.filtrarUsuarios(this.filtroLista)
      : this.usuarios;
  }

  public filtrarUsuarios(filtrarPor: string): Usuario[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.usuarios.filter(
      (usuario) =>
        usuario.nome.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
        usuario.sobrenome.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  constructor(
    private usuarioService: UsuarioService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {}

  public ngOnInit(): void {
    this.spinner.show();
    this.carregarUsuarios();
  }

  public alterarImagem(): void {
    this.exibirImagem = !this.exibirImagem;
  }

  public mostraImagem(imagemURL: string): string {
    return (imagemURL !== '')
      ? `${environment.apiURL}resources/images/${imagemURL}`
      : 'assets/img/semImagem.jpeg';
  }

  public carregarUsuarios(): void {
    this.usuarioService.getUsuarios().subscribe({
      next: (usuarios: Usuario[]) => {
        this.usuarios = usuarios;
        this.usuariosFiltrados = this.usuarios;
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao Carregar os Usuarios', 'Erro!');
      },
      complete: () => this.spinner.hide(),
    });
  }

  openModal(event: any, template: TemplateRef<any>, usuarioId: number): void {
    event.stopPropagation();
    this.usuarioId = usuarioId;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  confirm(): void {
    this.modalRef.hide();
    this.spinner.show();

    this.usuarioService
      .deleteUsuario(this.usuarioId)
      .subscribe(
        (result: any) => {
          if (result.message === 'Deletado') {
            this.toastr.success(
              'O Usuario foi deletado com Sucesso.',
              'Deletado!'
            );
            this.carregarUsuarios();
          }
        },
        (error: any) => {
          console.error(error);
          this.toastr.error(
            `Erro ao tentar deletar o usuario ${this.usuarioId}`,
            'Erro'
          );
        }
      )
      .add(() => this.spinner.hide());
  }

  decline(): void {
    this.modalRef.hide();
  }

  detalheUsuario(id: number): void {
    this.router.navigate([`usuarios/detalhe/${id}`]);
  }
}
