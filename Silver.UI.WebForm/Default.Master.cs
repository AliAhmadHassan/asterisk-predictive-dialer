using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web.UI;
using System.Text;
using System.IO;
using Silver.Common;

namespace Silver.UI.Web.Presentation.Comum
{
    public partial class Default : System.Web.UI.MasterPage
    {
        public string NomeAplicacao
        {
            set
            {
                lblNomePagina.Text = value;
            }
        }

        public string DescricaoAplicacao
        {
            set
            {
                lblDescricaoPagina.Text = value;
            }
        }

        public string NomeUsuario { get; set; }

        public string Ramal { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            lbl_nome_aplicacao.Text = ConfigurationManager.AppSettings["Application.Name"];
            img_logotipo_aplicacao.ImageUrl = ConfigurationManager.AppSettings["Application.Logotipo.Min.Path"];

            if (Session["Usuario"] != null)
                lit_menu.Text = ObterHTMLMenuOperador((Session["Usuario"] as DTO.Usuario).Id);
        }

        public string ObterHTMLMenuOperador(long operador)
        {
            if (!Directory.Exists(Server.MapPath(ConfigurationManager.AppSettings["Application.Paths.Temp.Imagens.Fisico"])))
                Directory.CreateDirectory(Server.MapPath(ConfigurationManager.AppSettings["Application.Paths.Temp.Imagens.Fisico"]));

            StringBuilder sb_menu = new StringBuilder();
            List<DTO.Menu> menu_operador = new List<DTO.Menu>();

            string[] grupos_id = new[] { "", "links", "files", "tools" };
            string[] grupos_descricao = new[] { "", "Cadastros", "Relatórios", "Telefonia" };

            #region Grupos
            for (int i = 1; i <= 3; i++)
            {
                menu_operador = new BLL.Menu().Obter(operador, i).OrderBy(c => c.Descricao).ToList<DTO.Menu>();
                sb_menu.Append(string.Format("<li id='{0}'>", grupos_id[i]));
                sb_menu.Append("<ul class='free'>");
                sb_menu.Append("<li class='header'>");
                sb_menu.Append("<a href='#' class='dock'>Fixar</a>");
                sb_menu.Append(string.Format("<a href='#' class='undock'>Liberar</a>{0}", grupos_descricao[i]));
                sb_menu.Append("</li>");

                #region Permissão
                foreach (var item in menu_operador)
                {
                    string nome_arquivo_icone = Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["Application.Paths.Temp.Imagens.Fisico"]), item.Id + ".png");
                    string path_virtual_arquivo = string.Empty;

                    if (!File.Exists(nome_arquivo_icone))
                        File.WriteAllBytes(nome_arquivo_icone, item.Icone == null ? new byte[] { 0, 1, 2, 3 } : item.Icone);

                    bool pasta_imagens = false;
                    path_virtual_arquivo = "/";

                    Uri imageUrl = new Uri(Path.GetFileName(nome_arquivo_icone), UriKind.Relative);
                    Uri baseImageUrl = new Uri(ConfigurationManager.AppSettings["Application.Paths.Temp.Imagens.Relativo"], UriKind.Relative);
                    Uri combinedImageUrl = baseImageUrl.Combine(imageUrl);

                    foreach (var parte_nome_arquivo in nome_arquivo_icone.Split('\\'))
                    {
                        if (!pasta_imagens)
                        {
                            if (parte_nome_arquivo.Equals("Imagens"))
                            {
                                path_virtual_arquivo += parte_nome_arquivo + "/";
                                pasta_imagens = true;
                            }
                        }
                        else
                            path_virtual_arquivo += parte_nome_arquivo + "/";
                    }
                    path_virtual_arquivo = path_virtual_arquivo.Remove(path_virtual_arquivo.LastIndexOf('/'), 1);

                    sb_menu.Append("<li>");
                    sb_menu.Append(string.Format("<a href='{0}'>", ResolveUrl(item.Url)));
                    sb_menu.Append(string.Format("<img src='{0}' alt='{1}' title='{1}' width='48px' height='48px' style='text-align:left' /> {1}", ResolveUrl(path_virtual_arquivo), item.Descricao));
                    sb_menu.Append("</a>");
                    sb_menu.Append("</li>");
                }
                #endregion

                sb_menu.Append("</ul>");
                sb_menu.Append("</li>");

            }
            #endregion

            return sb_menu.ToString();
        }
    }
}
