using System.Collections.Generic;
using System.Text;
using System.Linq;
using System;
using System.IO;
using System.Configuration;
using Silver.Common;

namespace Silver.BLL
{
    public class Menu
    {
        public List<DTO.Menu> Listar()
        {
            return new DAL.Menu().Select();
        }

        public List<DTO.Menu> Obter(bool ativo)
        {
            return new DAL.Menu().Obter(ativo);
        }

        public DTO.Menu Obter(int codigo)
        {
            return new DAL.Menu().SelectPelaPK(codigo);
        }

        public List<DTO.Menu> Buscar(string busca)
        {
            return new DAL.Menu().SelectPeloNome(busca);
        }

        public void Cadastrar(DTO.Menu menu)
        {
            new DAL.Menu().Cadastro(menu);
        }

        public void Ativar(int codigo, bool ativar)
        {
            var menu = Obter(codigo);
            menu.Ativo = ativar;
            Cadastrar(menu);
        }

        public List<DTO.Menu> Obter(long operador, long grupo_menu)
        {
            return new DAL.Menu().Obter(operador, grupo_menu);
        }

       
    }
}
