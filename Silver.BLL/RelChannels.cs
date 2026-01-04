using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Silver.Common;
using System.Xml.Serialization;
using System.IO;
using Silver.AsteriskClient;

namespace Silver.BLL
{
    public class RelChannels
    {
        private List<DTO.RelChannels> channels;

        public Silver.AsteriskClient.AsteriskCommand comandos_asterisk { get; set; }
        public Silver.AsteriskClient.AsteriskListener escuta_asterisk { get; set; }

        public bool queue_status_complete { get; set; }
        
        public RelChannels()
        { 
        }

        public RelChannels(AsteriskCommand comandos, AsteriskListener escuta)
        {
            comandos_asterisk = comandos;
            escuta_asterisk = escuta;

            escuta_asterisk.Stream_Asterisk = comandos_asterisk.Stream_Asterisk;
            escuta_asterisk.SaidaPadrao = AsteriskClient.SaidaPadraoAsterisk.Delegate;
            escuta_asterisk.OnDGVShowChannels = OnDGVShowChannels;

            escuta_asterisk.IniciarEscuta(comandos_asterisk.Stream_Asterisk);
        }

        public void Incluir(string dgv_show_channel)
        {

        }

        public List<DTO.RelChannels> DGVShowChannels()
        {
            List<DTO.RelCampanhaQueueStatus> retorno = new List<DTO.RelCampanhaQueueStatus>();

            comandos_asterisk.DGVShowChannels();

            int contador = 0;
            while (!queue_status_complete)
            {
                if (contador > 300)
                    queue_status_complete = true;
                else
                    contador++;

                System.Threading.Thread.Sleep(50);
            }

            return channels;
        }

        private void OnDGVShowChannels(string valores)
        {
            channels = new List<DTO.RelChannels>();

            foreach (string linha in valores.Split('\n'))
            {
                if ((linha == "") || (linha.Contains("Chan Grp Context          PortId")))
                    continue;

                DTO.RelChannels channel = new DTO.RelChannels();
                channel.Chan = linha.Substring(0, 5).Trim().ToInt32();
                channel.Grp = linha.Substring(6, 4).Trim().ToInt32();
                channel.Context = linha.Substring(10, 17).Trim();
                channel.PortId = linha.Substring(27, 11).Trim();
                channel.Rsrvd = linha.Substring(38, 8).Trim().ToInt32();
                channel.Alrmd = linha.Substring(46, 6).Trim().ToInt32();
                channel.Lckd = linha.Substring(52, 5).Trim();
                channel.Extension = linha.Substring(57, 10).Trim();
                channel.CardType = linha.Substring(67, 14).Trim();
                channel.Intrf = linha.Substring(81, 9).Trim();
                channels.Add(channel);
            }

            XmlSerializer xml = new XmlSerializer(typeof(List<DTO.RelChannels>));
            StringWriter strWriter = new StringWriter();
            xml.Serialize(strWriter, channels);
            new Silver.DAL.RelChannels().Inserir(strWriter.ToString());

            queue_status_complete = true;
        }

        public List<DTO.RelChannels> UltimoStatus()
        {
            return (List<DTO.RelChannels>)new XmlSerializer(typeof(List<DTO.RelChannels>)).Deserialize(new StringReader(new Silver.DAL.RelChannels().UltimoStatus()));
        }
    }
}
