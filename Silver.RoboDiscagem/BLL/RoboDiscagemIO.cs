using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Silver.Common;
using System.IO;

namespace Silver.RoboDiscagem.BLL
{
    public enum ProtocoloSaidaArquivo
    {
        FTP,
        SistemArquivo
    }
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    /// Data: 25/04/2013
    ///
    /// Classe especializada em Leitura e escrita de arquivos 
    /// para o Robo de discagem
    /// </summary>
    public class RoboDiscagemIO
    {
        /// <summary>
        /// Gera o arquivo SIP contendo as configuração encontradas na base
        /// </summary>
        /// <returns>
        /// Retorna Verdadeiro(true) caso o arquivo seja gerado com sucesso
        /// </returns>
        public bool GerarArquivoSIP(Action<string> saida_mensagem, ProtocoloSaidaArquivo destino = ProtocoloSaidaArquivo.FTP)
        {
            try
            {
                var arquivo = new StringBuilder();
                var configurations = ConfigurationManager.AppSettings;

                arquivo.AppendLine("[general]");
                foreach (var item in configurations)
                    if (item.ToString().Split('.')[0].Equals("sip"))
                        arquivo.AppendLine(ConfigurationManager.AppSettings[item.ToString()]);

                arquivo.AppendLine(string.Empty);
                var result = configurations.AllKeys.Where(w => w.Contains("template.sip"));

                foreach (var r in result)
                    arquivo.AppendLine(ConfigurationManager.AppSettings[r]);

                arquivo.AppendLine(string.Empty);
                arquivo.AppendLine(string.Empty);

                var usuarios = new Silver.BLL.Usuario().ObterOperadores(true);

                foreach (var item in usuarios.OrderBy(c => c.Ramal))
                {
                    arquivo.AppendLine(string.Format("[{0}](sip)", item.Ramal));
                    arquivo.AppendLine(string.Format("secret={0}", Silver.BLL.Criptografia.Descriptografar(item.Senha)));
                    arquivo.AppendLine(string.Empty);
                }

                var nome_arquivo = "sip.conf";
                var path_arquivo = System.IO.Path.Combine(ConfigurationManager.AppSettings["application.path.sip"], nome_arquivo);

                using (System.IO.FileStream fs = new System.IO.FileStream(path_arquivo, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite))
                using (StreamWriter sw = new StreamWriter(fs))
                    sw.WriteLine(arquivo.ToString());

                //Verifica a forma de envio do arquivo para o servidor Asterisk
                switch (destino)
                {
                    case ProtocoloSaidaArquivo.FTP:
                        UploadArquivo(path_arquivo);
                        break;
                    case ProtocoloSaidaArquivo.SistemArquivo:
                        var nome_arquivo_origem = Path.Combine(ConfigurationManager.AppSettings["application.asterisk.conf"], nome_arquivo);
                        File.Copy(nome_arquivo_origem, Path.Combine(ConfigurationManager.AppSettings["application.asterisk.conf"], nome_arquivo + "." + DateTime.Now.ToString("ddMMyyyy-HHmmss")));
                        File.Delete(nome_arquivo_origem);
                        File.Copy(path_arquivo, Path.Combine(ConfigurationManager.AppSettings["application.asterisk.conf"], nome_arquivo));
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                saida_mensagem(ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Envia os arquivos sip e queue para o servidor asterisk
        /// </summary>
        /// <param name="path_arquivo">The path_arquivo.</param>
        private void UploadArquivo(string path_arquivo)
        {
            new Silver.FtpClient.Ftp()
            {
                Host = ConfigurationManager.AppSettings["application.ftp.host"],
                Usuario = ConfigurationManager.AppSettings["application.ftp.user"],
                Senha = ConfigurationManager.AppSettings["application.ftp.password"],
                Porta = ConfigurationManager.AppSettings["application.ftp.port"].ToInt32()
            }.Upload(path_arquivo);
        }

        /// <summary>
        /// Gera o arquivo Queue.conf contendo as configuração encontradas na base
        /// </summary>
        /// <returns>
        /// Retorna Verdadeiro(true) caso o arquivo seja gerado com sucesso
        /// </returns>
        public bool GerarArquivoQueue(Action<string> saida_mensagem, ProtocoloSaidaArquivo destino = ProtocoloSaidaArquivo.FTP)
        {
            try
            {
                var arquivo = new StringBuilder();
                var configurations = ConfigurationManager.AppSettings;
                var valores_padroes = new List<string>();

                foreach (var item in configurations)
                    if (item.ToString().Split('.')[0].Equals("queue"))
                        valores_padroes.Add(ConfigurationManager.AppSettings[item.ToString()]);

                var campamnhas = new Silver.BLL.Campanha().Obter(true);
                var membros = new Silver.BLL.Usuario().Obter(true);

                foreach (var item in campamnhas)
                {
                    arquivo.AppendLine(string.Format("[{0}]", item.Nome.Trim().Replace(' ', '_'))); // Substitui os 'espaços' dos nomes das campanhas por um 'underline'
                    var string_format = new StringBuilder();

                    foreach (var valor in valores_padroes)
                        string_format.AppendLine(valor);

                    arquivo.AppendLine(string.Format(string_format.ToString(), Enum.GetName(typeof(DTO.EstrategiaDiscagem), item.Estrategia)));

                    var membro_campanha = membros.Where(m => m.IdCampanha.Equals(item.Id) && m.Operador.Equals(true));
                    foreach (var m in membro_campanha)
                        arquivo.AppendLine(string.Format("member => sip/{0}", m.Ramal));

                    arquivo.AppendLine(string.Empty);
                }

                var nome_arquivo = "queues.conf";
                var path_arquivo = System.IO.Path.Combine(ConfigurationManager.AppSettings["application.path.queue"], nome_arquivo);
                System.IO.File.WriteAllText(path_arquivo, arquivo.ToString(), Encoding.ASCII);
                
                //Verifica a forma de envio do arquivo para o servidor Asterisk
                switch (destino)
                {
                    case ProtocoloSaidaArquivo.FTP:
                        UploadArquivo(path_arquivo);
                        break;
                    case ProtocoloSaidaArquivo.SistemArquivo:
                        var nome_arquivo_origem = Path.Combine(ConfigurationManager.AppSettings["application.asterisk.conf"], nome_arquivo);
                        File.Copy(nome_arquivo_origem, Path.Combine(ConfigurationManager.AppSettings["application.asterisk.conf"], nome_arquivo + "." + DateTime.Now.ToString("ddMMyyyy-HHmmss")));
                        File.Delete(nome_arquivo_origem);
                        File.Copy(path_arquivo, Path.Combine(ConfigurationManager.AppSettings["application.asterisk.conf"], nome_arquivo));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                saida_mensagem(ex.Message);
                return false;
            }

            return true;
        }

    }
}
