using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.BLL
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 20/08/2014
    /// </summary>
    public class MensagemSistemaVisualizado
    {
        public static void Cadastrar(DTO.MensagemSistemaVisualizado visualizacao)
        {
            new Silver.DAL.MensagemSistemaVisualizado().Cadastro(visualizacao);
        }

        public static bool ExisteVisualizacao(long id_mensagem, long id_usuario)
        {
            return new Silver.DAL.MensagemSistemaVisualizado().ExisteVisualizacao(id_mensagem, id_usuario);
        }
    }
}
