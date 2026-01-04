using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.RoboDiscagem.BLL
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 11/11/2013
    /// Classe responsável pelo gerenciamento da escuta com o servidor Asterisk
    /// </summary>
    public static class GerenciadorEscuta
    {
        //Lista para gerenciar as aplicações que serão utilizadas como escuta para o discador
        public static List<System.Diagnostics.Process> processos_escuta = new List<System.Diagnostics.Process>();
    }
}
