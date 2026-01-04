using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.BLL
{
    public class ProcessoCampanha
    {
        public event OutputLogs OnOutputLogs;

        private void OutputLog(string msg)
        {
            var mensagem = string.Format("{0} - {1}", DateTime.Now, msg);
            Console.Out.WriteLine(mensagem);
            if (OnOutputLogs != null)
                OnOutputLogs(mensagem);
        }

        #region Propriedades

        public int IdHistorico { get; set; }

        public DTO.Campanha Campanha { get; set; }

        public List<DTO.Carga> Cargas { get; set; }

        public List<DTO.CargaTelefoneRobo> Telefones { get; set; }

        public List<DTO.UsuarioRobo> Operadores { get; set; }

        #endregion

        #region Contrutores

        public ProcessoCampanha()
        {
            Cargas = new List<DTO.Carga>();
            Telefones = new List<DTO.CargaTelefoneRobo>();
            Operadores = new List<DTO.UsuarioRobo>();
        }

        public ProcessoCampanha(DTO.Campanha campanha)
            : this()
        {
            this.Campanha = campanha;
        }

        #endregion

        #region Métodos

        public void InicializarOperadores()
        {
            var UsuariosAtivos = new BLL.Usuario().ObterLst(Campanha.Id, DTO.TipoConsulta.PelaCampanha);
            foreach (var usuario in UsuariosAtivos)
            {
                if (usuario.IdCampanha > 0)
                {
                    if (!usuario.Operador) continue;
                    var usuarioRobo = new DTO.UsuarioRobo()
                    {
                        Ativo = usuario.Ativo,
                        CampanhaDescricao = usuario.CampanhaDescricao,
                        Id = usuario.Id,
                        IdCampanha = usuario.IdCampanha,
                        IdGrupo = usuario.IdGrupo,
                        Nome = usuario.Nome,
                        Operador = usuario.Operador,
                        PenultimaSenha = usuario.PenultimaSenha,
                        Ramal = usuario.Ramal,
                        Senha = usuario.Senha,
                        SenhaExpiracao = usuario.SenhaExpiracao,
                        Status = DTO.EstadoUsuario.Logoff,
                        UltimaSenha = usuario.UltimaSenha
                    };

                    Operadores.Add(usuarioRobo);
                }
            }
        }

        public void InicializarCarga(int count_limit = 0)
        {
            if (count_limit == 0)
                Cargas = new BLL.Carga().SelectPelaCampanha(Campanha.Id);
            else
                Cargas = new BLL.Carga().SelectPelaCampanha(Campanha.Id).Take(count_limit).ToList<DTO.Carga>();

            foreach (var carga in Cargas)
            {
                var tel_carga = new BLL.CargaTelefone().ObterPelaCarga(carga.Id);
                foreach (var t in tel_carga)
                    Telefones.Add(
                        new DTO.CargaTelefoneRobo()
                        {
                            Ativo = t.Ativo,
                            TelefoneTratado = "",
                            Ddd = t.Ddd,
                            Id = t.Id,
                            IdCarga = t.IdCarga,
                            IdTipo = t.IdTipo,
                            Prioridade = 0,
                            Status = 1,
                            Telefone = t.Telefone,
                            TelId = t.TelId
                        }
                    );
            }

            if (Cargas.Count > 0)
                IdHistorico = Cargas[0].IdHistorico;
        }

        #endregion
    }
}
