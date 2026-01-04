using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.BLL
{
    public class UsuarioLogado
    {
        public virtual DTO.UsuarioLogado Obter(long ramal)
        {
            return new DAL.UsuarioLogado().Obter(ramal);
        }

        public void Atualizar(DTO.UsuarioLogado usuario_logado)
        {
            new DAL.UsuarioLogado().Atualizar(usuario_logado);
        }

        public void Atualizar(long ramal, string url)
        {
            var obj = new DTO.UsuarioLogado {Ramal = ramal, Url = url };
            Atualizar(obj);
        }

        public void Remover(DTO.UsuarioLogado usuario_logado)
        {
            new DAL.UsuarioLogado().Remover(usuario_logado);
        }

        public void Cadastrar(DTO.UsuarioLogado usuario_logado)
        {
            new DAL.UsuarioLogado().Cadastrar(usuario_logado);
        }

        public List<DTO.UsuarioLogado> Listar()
        {
            return new DAL.UsuarioLogado().Select();
        }
    }
}
