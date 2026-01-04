using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Silver.UI.Web.Presentation.Pages.Dashboard
{
    public class Tree<T>
    {
        public List<T> DataSource { get; set; }
        public string Id { get; set; }
        public string Descricao { get; set; }
        public string IdPai { get; set; }

        public TreeNode DataBind()
        {
            return Traverse(DataSource);
        }

        private TreeNode Traverse(List<T> carteiras)
        {
            return Traverse(carteiras[0], carteiras);
        }

        private TreeNode Traverse(T carteira, List<T> Geral)
        {
            carteira.GetType().GetProperty(Descricao).GetValue(carteira, null);
            TreeNode menu = new TreeNode(carteira.GetType().GetProperty(Descricao).GetValue(carteira, null).ToString().Trim().ToUpper(), carteira.GetType().GetProperty(Id).GetValue(carteira, null).ToString());

            List<T> subCarteiras = RetornaSubList(Geral, IdPai, carteira.GetType().GetProperty(Id).GetValue(carteira, null).ToString());
            for (int i = 0; i < subCarteiras.Count; i++)
            {
                menu.ChildNodes.Add(Traverse(subCarteiras[i], Geral));
            }

            return menu;
        }

        private List<T> RetornaSubList(List<T> lista, string Campo, string Valor)
        {
            List<T> Sublista = new List<T>();
            foreach (T aux in lista)
                if (aux.GetType().GetProperty(Campo).GetValue(aux, null).ToString() == Valor)
                    Sublista.Add(aux);

            return Sublista;
        }
    }
}