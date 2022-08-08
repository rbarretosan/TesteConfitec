import { Usuario } from "./Usuario";

export interface HistoricoEscolar {
  id: number;
  formato: string;
  nome: number;
  arquivoURL: string;
  usuarioId: number;
  usuario: Usuario;
}
