using System;
using System.Collections.Generic;
using Silver.DTO;

namespace Silver.BLL
{
    public class Usuario
    {
        public bool ValidarUsuario(string ramalUsuario, string senhaUsuario)
        {
            long ramal;
            DTO.Usuario u = null;

            if (Int64.TryParse(ramalUsuario, out ramal))
            {
                u = ObterObj(ramal, TipoConsulta.PeloRamal);
            }
            return (u != null && ValidarSenha(u.Ramal, senhaUsuario));
        }

        private bool ValidarSenha(long ramalUsuario, string senhaUsuario)
        {
            return ObterObj(ramalUsuario, TipoConsulta.PeloRamal).Senha.Equals(senhaUsuario);
        }

        public bool ValidarExpiracaoSenha(int codigo)
        {
            return ObterObj(codigo, TipoConsulta.PelaPK).SenhaExpiracao > DateTime.Now;
        }

        public List<DTO.Usuario> Listar()
        {
            return new DAL.Usuario().Select();
        }

        public List<DTO.Usuario> Obter(bool ativo)
        {
            return new DAL.Usuario().Obter(ativo);
        }

        /// <summary>
        /// Retorna somente os operadores da base de usuários
        /// </summary>
        /// <param name="operadores_ativos"></param>
        /// <returns></returns>
        public List<DTO.Usuario> ObterOperadores(bool operadores_ativos)
        {
            return new DAL.Usuario().ObterOperadores(operadores_ativos);
        }

        public List<DTO.Usuario> ObterLst(long codigo, TipoConsulta tipo)
        {
            switch (tipo)
            {
                case TipoConsulta.PeloGrupo:
                    return new DAL.Usuario().SelectPeloGrupo(codigo);
                case TipoConsulta.PelaCampanha:
                    return new DAL.Usuario().SelectPelaCampanha(codigo);
                default:
                    return null;
            }
        }

        public DTO.Usuario ObterObj(long codigo, TipoConsulta tipo)
        {
            DTO.Usuario usuario = null;

            switch (tipo)
            {
                case TipoConsulta.PelaPK:
                    usuario = new DAL.Usuario().SelectPelaPK(codigo);
                    usuario.CampanhaDescricao = new BLL.Campanha().Obter(usuario.IdCampanha).Descricao;
                    usuario.Senha = Criptografia.Descriptografar(usuario.Senha);
                    usuario.UltimaSenha = Criptografia.Descriptografar(usuario.UltimaSenha);
                    usuario.PenultimaSenha = Criptografia.Descriptografar(usuario.PenultimaSenha);
                    return usuario;
                case TipoConsulta.PeloRamal:
                    usuario = new DAL.Usuario().SelectPeloRamal(codigo);
                    usuario.CampanhaDescricao = new BLL.Campanha().Obter(usuario.IdCampanha).Descricao;
                    usuario.Senha = Criptografia.Descriptografar(usuario.Senha);
                    usuario.UltimaSenha = Criptografia.Descriptografar(usuario.UltimaSenha);
                    usuario.PenultimaSenha = Criptografia.Descriptografar(usuario.PenultimaSenha);
                    return usuario;
                default:
                    return usuario;
            }
        }

        public List<DTO.Usuario> Buscar(string busca)
        {
            return new DAL.Usuario().SelectPeloNome(busca);
        }

        public void Cadastrar(DTO.Usuario usuario)
        {
            usuario.Senha = Criptografia.Criptografar(usuario.Senha);
            usuario.UltimaSenha = Criptografia.Criptografar(usuario.UltimaSenha);
            usuario.PenultimaSenha = Criptografia.Criptografar(usuario.PenultimaSenha);
            new DAL.Usuario().Cadastro(usuario);
            ControleSistema.IncluirEvento(EventoControleSistema.Recarregar_Sip, new DTO.ControleSistema { Valor = "0", Situacao = 1, Campanha = usuario.IdCampanha, Solicitante = usuario.Id });
        }

        public void Ativar(int codigo, bool ativar)
        {
            var usuario = ObterObj(codigo, TipoConsulta.PelaPK);
            usuario.Ativo = ativar;
            Cadastrar(usuario);
        }
    }
}
