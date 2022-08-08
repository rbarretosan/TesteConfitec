import { Escolaridade } from "./enum/Escolaridade";
import { HistoricoEscolar } from "./HistoricoEscolar";

export interface Usuario {
  id: number;
  nome: string;
  sobrenome: string;
  email: string;
  dataNascimento?: Date;
  historicoEscolar: HistoricoEscolar[];
  escolaridadeId: Escolaridade;
}
