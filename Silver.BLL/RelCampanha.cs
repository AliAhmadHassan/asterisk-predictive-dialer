using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Silver.DTO;
using System.Xml.Serialization;
using System.IO;

namespace Silver.BLL
{
    public class RelCampanha
    {
        public RelCampanha()
        {
            comandos_asterisk = null;
            escuta_asterisk = null;
        }

        public RelCampanha(ref Silver.AsteriskClient.AsteriskCommand comando, ref Silver.AsteriskClient.AsteriskListener escuta)
        {
            comandos_asterisk = comando;
            escuta_asterisk = escuta;
        }

        public string Extrair(string strTexto)
        {
            string strRetorno = "";
            DTO.RelCampanha objSituacao = new DTO.RelCampanha();
            List<DTO.RelCampanhaDados> lstDados = new List<DTO.RelCampanhaDados>();
            DTO.RelCampanhaDados objDados = new DTO.RelCampanhaDados();

            string[] arrTexto = strTexto.Split('\n');
            string strLinha = "";

            for (int i = 0; i < arrTexto.Length; i++)
            {
                strLinha = arrTexto[i];
                if (strLinha.Length < 50)
                    continue;

                if (i == 0)
                {
                    objSituacao.dadoC = Convert.ToInt32(strLinha.Substring(strLinha.LastIndexOf(", C:") + 4, strLinha.LastIndexOf(", A:") - strLinha.LastIndexOf(", C:") - 4));
                    objSituacao.dadoA = Convert.ToInt32(strLinha.Substring(strLinha.LastIndexOf(", A:") + 4, strLinha.LastIndexOf(", SL:") - strLinha.LastIndexOf(", A:") - 4));
                }
                if (strLinha.ToLower().Contains("sip/"))
                {
                    if (i > 0 && strLinha.ToLower().Contains("sip/"))
                    {
                        objDados = new DTO.RelCampanhaDados();
                        objDados.Ramal = Convert.ToInt32(strLinha.Substring(strLinha.IndexOf("/") + 1, 4));

                        if (strLinha.Contains("(Unavailable)"))
                            objDados.Status = "Inacessível";
                        else if (strLinha.Contains("(Busy)"))
                            objDados.Status = "Ocupado";
                        else if (strLinha.Contains("(paused) (Not in use)"))
                            objDados.Status = "Em pausa";
                        else
                            objDados.Status = "Aguardando";

                        if (!strLinha.Contains("has taken no calls yet"))
                        {
                            objDados.Chamadas = Convert.ToInt32(strLinha.Substring(strLinha.IndexOf("has taken") + 10, strLinha.IndexOf("calls (") - 1 - strLinha.IndexOf("has taken") - 10));
                            objDados.Duracao = Convert.ToInt32(strLinha.Substring(strLinha.IndexOf("(last was") + 10, strLinha.IndexOf("secs ago)") - 1 - strLinha.IndexOf("(last was") - 10));
                        }
                        lstDados.Add(objDados);
                    }
                }
            }

            objSituacao.lstDados = lstDados;

            XmlSerializer xml = new XmlSerializer(typeof(DTO.RelCampanha), "RelCampanha");
            StringWriter strWriter = new StringWriter();

            xml.Serialize(strWriter, objSituacao);
            strRetorno = strWriter.ToString();

            return strRetorno;
        }

        public DTO.RelCampanha SelectQueueShow(int Id)
        {
            return new DAL.RelCampanha().SelectQueueShow(Id);
        }

        public List<DTO.RelCampanhaQueueStatus> SelectQueueStatus(int Id)
        {
            return new DAL.RelCampanha().SelectQueueStatus(Id);
        }

        public List<DTO.RelCampanhaDB> Listar()
        {
            return new DAL.RelCampanha().Select();
        }

        public DTO.RelCampanhaDB Obter(long codigo)
        {
            return new DAL.RelCampanha().SelectPelaPK(codigo);
        }

        public void Inserir(DTO.RelCampanhaDB relCampanha)
        {
            new DAL.RelCampanha().Inserir(ref relCampanha, "IdCampanha", "SPIRelCampanha");
        }

        public void Alterar(DTO.RelCampanhaDB relCampanha)
        {
            new DAL.RelCampanha().Alterar(relCampanha, "SPURelCampanha");
        }

        public Silver.AsteriskClient.AsteriskCommand comandos_asterisk {get;set;}
        public Silver.AsteriskClient.AsteriskListener escuta_asterisk { get; set; }
        
        DTO.Campanha campanha_informada;

        private static List<Silver.AsteriskClient.DTO.RelCampanhaQueueStatus> lstRelCampanhaQueueStatus;

        bool queue_status_complete = false;

        public void QueueShowEStatus(DTO.Campanha campanha)
        {
            List<DTO.RelCampanhaQueueStatus> retorno = new List<DTO.RelCampanhaQueueStatus>();
            this.campanha_informada = campanha;

            comandos_asterisk = new AsteriskClient.AsteriskCommand();
            escuta_asterisk = new AsteriskClient.AsteriskListener();
            lstRelCampanhaQueueStatus = new List<Silver.AsteriskClient.DTO.RelCampanhaQueueStatus>();

            escuta_asterisk.Stream_Asterisk = comandos_asterisk.Stream_Asterisk;
            escuta_asterisk.SaidaPadrao = AsteriskClient.SaidaPadraoAsterisk.Delegate;
            escuta_asterisk.OnQueueEntry = OnQueueEntry;
            escuta_asterisk.OnQueueStatusComplete = OnQueueStatusComplete;
            escuta_asterisk.OnQueueShow = OnQueueShow;
            escuta_asterisk.IniciarEscuta(comandos_asterisk.Stream_Asterisk);
            System.Threading.Thread.Sleep(50);

            comandos_asterisk.QueueShow(campanha.Nome.Trim().Replace(' ', '_'));
            System.Threading.Thread.Sleep(50);

            comandos_asterisk.QueueStatus(campanha.Nome.Trim().Replace(' ', '_'));

            int contador = 0;
            while (!queue_status_complete)
            {
                if (contador > 300)
                    queue_status_complete = true;
                else
                    contador++;

                System.Threading.Thread.Sleep(20);
            }
        }

        public void OnQueueShow(string valores)
        {
            DTO.RelCampanhaDB relCampanhaDB = Obter(campanha_informada.Id);

            if (relCampanhaDB == null)
            {
                relCampanhaDB = new RelCampanhaDB();
                relCampanhaDB.IdCampanha = campanha_informada.Id;
                Inserir(relCampanhaDB);
            }
            
            relCampanhaDB.QueueShow = Extrair(valores);
            Alterar(relCampanhaDB);
        }

        public void OnQueueEntry(Silver.AsteriskClient.DTO.RelCampanhaQueueStatus _RelCampanhaQueueStatus)
        {
            lstRelCampanhaQueueStatus.Add(_RelCampanhaQueueStatus);
        }

        public void OnQueueStatusComplete()
        {
            DTO.RelCampanhaDB relCampanhaDB = Obter(campanha_informada.Id);

            if (relCampanhaDB == null)
            {
                relCampanhaDB = new RelCampanhaDB();
                relCampanhaDB.IdCampanha = campanha_informada.Id;
                Inserir(relCampanhaDB);
            }

            XmlSerializer xml = new XmlSerializer(typeof(List<Silver.AsteriskClient.DTO.RelCampanhaQueueStatus>), "RelCampanhaQueueStatus");
            StringWriter strWriter = new StringWriter();
            xml.Serialize(strWriter, lstRelCampanhaQueueStatus);
            relCampanhaDB.QueueStatus = strWriter.ToString();

            Alterar(relCampanhaDB);

            if (escuta_asterisk != null)
                escuta_asterisk.Dispose();

            if (comandos_asterisk != null)
                comandos_asterisk.Dispose();

            if (lstRelCampanhaQueueStatus != null)
            {
                lstRelCampanhaQueueStatus.Clear();
                lstRelCampanhaQueueStatus = null;
            }

            queue_status_complete = true;
        }
    }
}
