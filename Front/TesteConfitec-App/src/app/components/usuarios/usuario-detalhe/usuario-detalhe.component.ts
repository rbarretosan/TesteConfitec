import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { HistoricoEscolarService } from './../../../services/historicoEscolar.service';
import { UsuarioService } from '@app/services/usuario.service';
import { Usuario } from '@app/models/Usuario';
import { HistoricoEscolar } from '@app/models/HistoricoEscolar';
import { DatePipe } from '@angular/common';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-usuario-detalhe',
  templateUrl: './usuario-detalhe.component.html',
  styleUrls: ['./usuario-detalhe.component.scss'],
  providers: [ DatePipe ]
})
export class UsuarioDetalheComponent implements OnInit {
  modalRef: BsModalRef;
  usuarioId: number;
  usuario = {} as Usuario;
  form: FormGroup;
  estadoSalvar = 'post';
  historicoEscolarAtual = {id: 0, formato: '', nome: '', usuarioId: 0, arquivoURL: '', indice: 0};

  bsValue = new Date();
  bsRangeValue: Date[];
  maxDate = new Date();
  minDate = new Date();

  arquivoURL = 'assets/img/upload.png';
  file: File;

  get modoEditar(): boolean {
    return this.estadoSalvar === 'put';
  }

  get historicosEscolares(): FormArray {
    return this.form.get('historicosEscolares') as FormArray;
  }

  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY',
      containerClass: 'theme-default',
      showWeekNumbers: false,
      showTodayButton: true
    };
  }

  constructor(private fb: FormBuilder,
              private localeService: BsLocaleService,
              private activatedRouter: ActivatedRoute,
              private usuarioService: UsuarioService,
              private spinner: NgxSpinnerService,
              private toastr: ToastrService,
              private modalService: BsModalService,
              private router: Router,
              private historicoEscolarService: HistoricoEscolarService,
              private datePipe: DatePipe)
  {
    this.localeService.use('pt-br');

    this.minDate.setDate(this.minDate.getDate());
    this.maxDate.setDate(this.maxDate.getDate());
    this.bsRangeValue = [this.bsValue, this.maxDate];
  }

  public carregarUsuario(): void {
    this.usuarioId = +this.activatedRouter.snapshot.paramMap.get('id');

    if (this.usuarioId !== null && this.usuarioId !== 0) {
      this.spinner.show();

      this.estadoSalvar = 'put';

      this.usuarioService.getUsuarioById(this.usuarioId).subscribe(
        (usuario: Usuario) => {
          this.usuario = {...usuario};
          this.form.patchValue(this.usuario);
          this.carregarHistoricosEscolares();
        },
        (error: any) => {
          this.toastr.error('Erro ao tentar carregar Usuario.', 'Erro!');
          console.error(error);
        }
      ).add(() => this.spinner.hide());
    }
  }

  public carregarHistoricosEscolares(): void {
    this.historicoEscolarService.getHistoricosEscolaresByUsuarioId(this.usuarioId).subscribe(
      (historicosEscolaresRetorno: HistoricoEscolar[]) => {
        historicosEscolaresRetorno.forEach(historicoEscolar => {
          this.historicosEscolares.push(this.criarHistoricoEscolar(historicoEscolar));
        });
      },
      (error: any) => {
        this.toastr.error('Erro ao tentar carregar historicosEscolares', 'Erro');
        console.error(error);
      }
    ).add(() => this.spinner.hide());
  }

  ngOnInit(): void {
    this.carregarUsuario();
    this.validation();
  }

  public validation(): void {
    this.form = this.fb.group({
      nome: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      sobrenome: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]],
      dataNascimento: ['', Validators.required ],
      escolaridadeId: ['', Validators.required],
      historicosEscolares: this.fb.array([])
    });
  }

  adicionarHistoricoEscolar(): void {
    this.historicosEscolares.push(this.criarHistoricoEscolar({id: 0} as HistoricoEscolar));
  }

  criarHistoricoEscolar(historicoEscolar: HistoricoEscolar): FormGroup {
    return this.fb.group({
      id: [historicoEscolar.id],
      formato: historicoEscolar.formato,
      nome: [historicoEscolar.nome, Validators.required],
      arquivoURL: historicoEscolar.arquivoURL,
    });
  }

  public retornaTituloHistoricoEscolar(nome: string): string {
    return nome === null || nome === '' ? 'Nome do historicoEscolar' : nome;
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl | AbstractControl): any {
    return {'is-invalid': campoForm.errors && campoForm.touched};
  }

  public salvarUsuario(): void {
    this.spinner.show();
    if (this.form.valid) {

      this.usuario = (this.estadoSalvar === 'post')
                ? {...this.form.value}
                : {id: this.usuario.id, ...this.form.value};

      this.usuarioService[this.estadoSalvar](this.usuario).subscribe(
        (usuarioRetorno: Usuario) => {
          this.toastr.success('Usuario salvo com Sucesso!', 'Sucesso');
          this.router.navigate([`usuarios/detalhe/${usuarioRetorno.id}`]);
          //setTimeout(() => { this.router.navigate([`usuarios/lista`]); }, 500);
        },
        (error: any) => {
          console.error(error);
          this.spinner.hide();
          this.toastr.error('Error ao salvar usuario', 'Erro');
        },
        () => this.spinner.hide()
      );
    }
  }

  public salvarHistoricosEscolares(): void {
    if (this.form.controls.historicosEscolares.valid) {
      this.spinner.show();
      this.historicoEscolarService.saveHistoricoEscolar(this.usuarioId, this.form.value.historicosEscolares)
        .subscribe(
          () => {
            this.historicosEscolares.clear();
            this.carregarHistoricosEscolares();
            this.toastr.success('Historicos Escolares salvos com Sucesso!', 'Sucesso!');
          },
          (error: any) => {
            this.toastr.error('Erro ao tentar salvar Historicos Escolares.', 'Erro');
            console.error(error);
          }
        ).add(() => this.spinner.hide());
    }
  }

  public removerHistoricoEscolar(template: TemplateRef<any>, indice: number): void {

    this.historicoEscolarAtual.id = this.historicosEscolares.get(indice + '.id').value;
    this.historicoEscolarAtual.formato = this.historicosEscolares.get(indice + '.formato').value;
    this.historicoEscolarAtual.nome = this.historicosEscolares.get(indice + '.nome').value;
    this.historicoEscolarAtual.arquivoURL = this.historicosEscolares.get(indice + '.arquivoURL').value;
    this.historicoEscolarAtual.indice = indice;

    this.modalRef = this.modalService.show(template, {class: 'modal-sm' });
  }

  confirmDeleteHistoricoEscolar(): void {
    this.modalRef.hide();
    this.spinner.show();

    this.historicoEscolarService.deleteHistoricoEscolar(this.usuarioId, this.historicoEscolarAtual.id)
      .subscribe(
        () => {
          this.historicosEscolares.clear();
          this.carregarHistoricosEscolares();
          this.toastr.success('HistoricoEscolar deletado com sucesso', 'Sucesso');
        },
        (error: any) => {
          this.toastr.error(`Erro ao tentar deletar o HistoricoEscolar ${this.historicoEscolarAtual.id}`, 'Erro');
          console.error(error);
        }
      ).add(() => this.spinner.hide());
  }

  declineDeleteHistoricoEscolar(): void {
    this.modalRef.hide();
  }

  download(template: TemplateRef<any>, indice: number): void{
    this.historicoEscolarAtual.arquivoURL = this.historicosEscolares.get(indice + '.arquivoURL').value;
    const uri = environment.apiURL + environment.resourcesFiles;
    const file = this.historicoEscolarAtual.arquivoURL;
    this.downloadUri(uri + file, file);
  }

  downloadUri(url: string, filename: string): void {
    fetch(url).then(function(t){
        return t.blob().then((b) => {
            const a = document.createElement('a');
            a.href = URL.createObjectURL(b);
            a.setAttribute('download', filename);
            a.click();
        });
    });
  }

  onFileChange(ev: any, template: TemplateRef<any>, indice: number): void {

    const reader = new FileReader();

    this.historicoEscolarAtual.usuarioId = this.usuario.id;
    this.historicoEscolarAtual.id = this.historicosEscolares.get(indice + '.id').value;

    this.file = ev.target.files;
    reader.readAsDataURL(this.file[0]);

    this.uploadFile(this.historicoEscolarAtual.usuarioId, this.historicoEscolarAtual.id, this.file);
  }

  uploadFile(usuarioId: number, historicoEscolarId: number, file: File): void{
    this.spinner.show();
    this.historicoEscolarService.postUpload(usuarioId, historicoEscolarId, file)
      .subscribe(
        () => {
          this.historicosEscolares.clear();
          this.carregarHistoricosEscolares();
          this.toastr.success('Arquivo carregado com sucesso!', 'Sucesso!');
        },
        (error: any) => {
          this.toastr.error('Erro ao tentar carregar arquivo.', 'Erro');
          console.error(error);
        }
      ).add(() => this.spinner.hide());
  }
}
