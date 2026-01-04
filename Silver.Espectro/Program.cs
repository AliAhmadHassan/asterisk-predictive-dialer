using System;
using System.Timers;
using System.Collections.Generic;

namespace Espectro
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var t = new Timer(1000);
            t.Elapsed += new ElapsedEventHandler(t_Elapsed);
        }

        private static void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //foreach (Silver.DTO.Bilhetagem b in new Silver.BLL.Bilhetagem().ObterLigacoesParaAnalise())
            //{
            //    var w = new AnalisadorWave(b.recordingpath);

            //    b.otiosetime = w.TempoEmSilencio();
            //    b.amdtype = new Silver.BLL.AmdType().Obter(w.TipoAMD()).Id;
            //    b.callanalyzed = true;

            //    new Silver.BLL.Bilhetagem().Cadastrar(b);
            //}
        }
    }
}
