using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.BLL
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 26/06/2013    
    /// </summary>
    public class ControleSistema
    {
        /// <summary>
        /// Lista todos os eventos registrados na tabela
        /// </summary>
        /// <returns></returns>
        public static List<DTO.ControleSistema> ListarControles()
        {
            return new DAL.ControleSistema().ListarControles(null);
        }

        public static List<DTO.ControleSistema> ListarControle(long id_usuario)
        {
            return new DAL.ControleSistema().ListarControles(id_usuario);
        }

        /// <summary>
        /// Inclui um evento na tabela de controle
        /// </summary>
        /// <param name="evento">Evento solicitado</param>
        /// <param name="valor">Valor para o evento</param>
        public static void IncluirEvento(DTO.EventoControleSistema evento, DTO.ControleSistema controle)
        {
            new DAL.ControleSistema().IncluirEvento(evento, controle);
        }

        public static void IncluirEvento(Silver.DTO.ControleSistema controle_sistema)
        {
            DTO.EventoControleSistema evento = (DTO.EventoControleSistema)Enum.Parse(typeof(DTO.EventoControleSistema), controle_sistema.Evento, true);
            IncluirEvento(evento, controle_sistema);//TODO - Esta parte da aplicação não possui autenticação, por enquanto manter os solicitantes originais
        }

        public static void AtualizarHoraExecucao(int id_controle)
        {
            new DAL.ControleSistema().AtualizarHoraExecucao(id_controle);
        }

        public static void AtualizarStatusExecucao(DTO.SitucaoEventoControleSistema status, long id_controle)
        {
            new DAL.ControleSistema().AtualizarStatusExecucao(status, id_controle);
        }

        public static List<DTO.ControleSistema> ListarControles(long id_usuario)
        {
            return new DAL.ControleSistema().ListarControles(id_usuario);
        }

        public static List<DTO.ControleSistema> ListarTodosControles(long id_usuario)
        {
            return new DAL.ControleSistema().ListarTodosControles(id_usuario);
        }

        public static List<DTO.ControleSistema> ListarTodosControles()
        {
            return new DAL.ControleSistema().ListarTodosControles(null);
        }

        /// <summary>
        /// Obtem a ultima solicitação de processamento de carga da campanha selecionada
        /// </summary>
        /// <param name="id_campanha">Id Campanha que se deseja conhecer a carga</param>
        /// <returns></returns>
        public static DTO.ControleSistema ObterUltimaCarga(long id_campanha)
        {
            return new DAL.ControleSistema().ObterUltimaCarga(id_campanha);
        }

        /// <summary>
        /// Retorna a ultima solicitação de discagem
        /// </summary>
        /// <param name="id_campanha">id da campanha que se deseja conhecer o mailing</param>
        /// <returns></returns>
        public static DTO.ControleSistema ObterUltimoMailing(long id_campanha)
        {
            return new DAL.ControleSistema().ObterUltimoMailing(id_campanha);
        }

        public List<DTO.ControleSistema> ListarPelaCampanha(long id_campanha)
        {
            return new DAL.ControleSistema().ListarPelaCampanha(id_campanha);
        }

        public static void AtualizarPorcentagem(long id_controle_sistema, decimal porcentagem)
        {
            new DAL.ControleSistema().AtualizarPorcentagem(id_controle_sistema, porcentagem);
        }

        public static DTO.ControleSistema ObterControleSistema(long id_controle_sistema)
        {
            return new DAL.ControleSistema().ObterControleSistema(id_controle_sistema);
        }
    }
}
