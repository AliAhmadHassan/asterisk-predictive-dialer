using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Silver.RoboDiscagem.BLL
{

    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 18/06/2014
    /// </summary>
    public class EscutaDiretorio
    {
        public delegate void DiscagemFinalizada(long id_campanha);
        public event DiscagemFinalizada OnDiscagemFinalizada;

        FileSystemWatcher fsw;

        public void OnCampanhaFinalizada(long id_campanha)
        {
            if (OnDiscagemFinalizada != null)
                OnDiscagemFinalizada(id_campanha);
        }

        public string PathDiretorio { get; set; }

        public EscutaDiretorio(string path_diretorio)
        {
            PathDiretorio = path_diretorio;
        }

        public void Iniciar()
        {
            fsw = new FileSystemWatcher(PathDiretorio);
            fsw.EnableRaisingEvents = true;
            fsw.IncludeSubdirectories = true;

            fsw.Created += fsw_Created;
        }

        void fsw_Created(object sender, FileSystemEventArgs e)
        {
            System.Threading.Thread.Sleep(1500);
            var linhas_arquivo = File.ReadAllLines(e.FullPath);
            if (e.Name.Contains("DiscadorFinalizado"))
            {
                var id_campanha = long.Parse(linhas_arquivo[0].Split('|')[1]);
                if (id_campanha > 0)
                    OnCampanhaFinalizada(id_campanha);

                File.Delete(e.FullPath);
            }
        }
    }
}
