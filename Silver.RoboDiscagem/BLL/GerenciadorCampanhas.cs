using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.RoboDiscagem.BLL
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 11/11/2013
    /// Classe responsável pelo gerenciamento e execução das campanhas             
    /// </summary>
    public class GerenciadorCampanhas
    {
        /// <summary>
        /// Lista com campanhas em execução
        /// </summary>
        public static SortedList<long, System.Diagnostics.Process> processos_campanhas = new SortedList<long, System.Diagnostics.Process>();

        public static void IniciarCampanha(long id_campanha)
        {

        }
    }
}
