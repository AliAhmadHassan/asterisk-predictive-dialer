using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.BLL
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 03/07/2014
    /// </summary>
    public static class MensagemSistema
    {
        public static void Cadastrar(DTO.MensagemSistema mensagem_sistema)
        {
            new DAL.MensagemSistema().Cadastro(mensagem_sistema);
        }

        public static List<DTO.MensagemSistema> ListarNaoVisualizadas(long id_campanha)
        {
            return new DAL.MensagemSistema().ListarNaoVisualizadas(id_campanha);
        }

        public static List<DTO.MensagemSistema> ListarVisualizados(long id_campanha)
        {
            return new DAL.MensagemSistema().ListarVisualizadas(id_campanha);
        }

        public static void MarcarComoVisualizada(long id_mensagem)
        {
            new DAL.MensagemSistema().MarcarComoVisualizada(id_mensagem);
        }

        public static bool ExisteMensagemNaoVisualizada(long id_campanha, Silver.DTO.TipoMensagemSistema tipo_mensagem)
        {
            return new DAL.MensagemSistema().ExisteMensagemNaoVisualizada(id_campanha, tipo_mensagem);
        }
    }
}
