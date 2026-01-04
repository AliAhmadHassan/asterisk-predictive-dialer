using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Silver.Discador
{
    public enum CtrlType
    {
        CTRL_C_EVENT = 0,
        CTRL_BREAK_EVENT = 1,
        CTRL_CLOSE_EVENT = 2,
        CTRL_LOGOFF_EVENT = 5,
        CTRL_SHUTDOWN_EVENT = 6
    }

    public static class TratamentoEventos
    {
        static EventHandler _handler;

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
        private delegate bool EventHandler(CtrlType sig);

        private static bool Handler(CtrlType sig)
        {
            switch (sig)
            {
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_BREAK_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                default:
                    new BLL.LogDiscador().Incluir(new Silver.DTO.LogDiscador() 
                        { 
                            DataHora = DateTime.Now, 
                            Evento = (int)OperacaoDiscador.FimOperacaoDiscador, 
                            IdCampanha = (int)processoCampanha.Campanha.Id 
                        });

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine();
                    File.WriteAllText(string.Format("DiscadorFinalizado-{0}-{1}.sil", 
                            processoCampanha.Campanha.Nome, 
                            DateTime.Now.ToString("dd.MM.yy_HH.mm.ss")), 
                            "Campanha|" + processoCampanha.Campanha.Id.ToString());
                    break;
            }
            return true;
        }
    }


}
