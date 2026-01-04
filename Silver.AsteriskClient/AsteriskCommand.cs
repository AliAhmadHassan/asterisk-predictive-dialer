using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net.Sockets;
using Silver.Common;
using System.IO;

namespace Silver.AsteriskClient
{
    public class AsteriskCommand : AsteriskBase
    {
        public AsteriskCommand()
            : base()
        {

        }

        public AsteriskCommand(string usuario_asterisk, string senha_asterisk, string ip_asterisk, int porta_asterisk)
            : base(usuario_asterisk, senha_asterisk, ip_asterisk, porta_asterisk)
        {
        }

        public void Comando(ComandoAsterisk comando_asterisk)
        {
            string comando = string.Empty;
            switch (comando_asterisk)
            {
                case ComandoAsterisk.QueueReload:
                    EnviarComando(string.Format("Action: Command\nCommand:queue reload all\n\n"));
                    break;
                case ComandoAsterisk.SipReload:
                    EnviarComando(string.Format("Action: Command\nCommand:sip reload\n\n"));
                    break;
                case ComandoAsterisk.DialPlanReload:
                    EnviarComando(string.Format("Action: Command\nCommand:dialplan reload\n\n"));
                    break;
                case ComandoAsterisk.CoreReload:
                    EnviarComando(string.Format("Action: Command\nCommand:core reload\n\n"));
                    break;
            }
        }

        public void Discar(DTO.Discagem discagem)
        {
            EnviarComando(discagem.ToString());
        }

        public void QueueShow(string nome_fila)
        {
            EnviarComando(string.Format("Action: Command\nCommand:Queue show {0}\n\n", nome_fila));
        }

        public void DGVShowChannels()
        {
            EnviarComando(string.Format("Action: Command\nCommand:DGV Show Channels\n\n"));
        }

        public void QueueStatus(string nome_fila)
        {
            EnviarComando(string.Format("Action:QueueStatus\nQueue: {0}\n\n", nome_fila));
        }

        public void IniciarPausaNaFila(long ramal, string fila, string razao = "Razão não informada")
        {
            EnviarComando(string.Format("Action: QueuePause\nInterface: SIP/{0}\nPaused: true\nQueue:{1}\nReason: {2}\n\n", ramal, fila, razao));
        }

        public void FinalizarPausaNaFila(long ramal, string fila, string razao = "Razão não informada")
        {
            var comando = string.Format("Action: QueuePause\nInterface: SIP/{0}\nPaused: false\nQueue:{1}\nReason: {2}\n\n", ramal, fila, razao);
            EnviarComando(comando);
        }

        private void EnviarComando(String message)
        {
            try
            {
                Byte[] data = Encoding.ASCII.GetBytes(message);
                Stream_Asterisk.Write(data, 0, data.Length);
            }
            catch (IOException io_ex)
            {
                base.IniciarSocket();
                EnviarComando(message);
                SaidaLogs(io_ex.Message, LoggerType.ERRO);
            }
            catch (ObjectDisposedException obj_ex)
            {
                base.IniciarSocket();
                EnviarComando(message);
                SaidaLogs(obj_ex.Message, LoggerType.ERRO);
            }
            catch (Exception ex)
            {
                SaidaLogs(ex.Message, LoggerType.ERRO);
                throw;
            }
        }

        public void RamaisStatus()
        {
            var command_asterisk = string.Format("Action: Command\nCommand:{0}\n\n", "sip show inuse");
            EnviarComando(command_asterisk);
        }

        public void StatusFila(string fila)
        {
            //A resposta deste método saira no método 'QueueMember'
            var command_asterisk = string.Format("Action: QueueStatus\nQueue:{0}\n\n", fila);
            EnviarComando(command_asterisk);
        }

        public void AdicionarOperadorFila(long ramal, string fila)
        {
            var command_asterisk = string.Format("Action: QueueAdd\nInterface: SIP/{0}\nPaused: no\nQueue:{1}\nMemberName: {0}\n\n", ramal, fila);
            EnviarComando(command_asterisk);
        }

        public void RemoverOperadorFila(long ramal, string fila)
        {
            var command_asterisk = string.Format("Action: QueueRemove\nQueue: {0}\nInterface: sip/{1}\n\n", fila, ramal);
            EnviarComando(command_asterisk);
        }
    }
}
