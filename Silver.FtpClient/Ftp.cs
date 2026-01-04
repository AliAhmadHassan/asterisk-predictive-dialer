using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.FtpClient;
using System.IO;
using System.Net;
using System.Configuration;

namespace Silver.FtpClient
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 14/06/2013
    /// </summary>
    public class Ftp
    {
        public string Host { get; set; }

        public string Usuario { get; set; }

        public string Senha { get; set; }

        public int Porta { get; set; }

        public void Upload(string path_source_file)
        {
            try
            {
                if (string.IsNullOrEmpty(Host) || string.IsNullOrEmpty(Usuario) || string.IsNullOrEmpty(Senha))
                    throw new Exception("Propriedades de conexão incompletas");

                using (System.Net.FtpClient.FtpClient conn = new System.Net.FtpClient.FtpClient())
                {
                    conn.Host = Host;
                    conn.Credentials = new NetworkCredential(Usuario, Senha);

                    Stream ostream = conn.OpenWrite(Path.GetFileName(path_source_file), FtpDataType.ASCII);
                    if (ostream != null)
                    {
                        const int buffer = 2048;
                        byte[] contentRead = new byte[buffer];
                        int bytesRead;

                        using (FileStream fs = new FileInfo(path_source_file).OpenRead())
                        {
                            do
                            {
                                bytesRead = fs.Read(contentRead, 0, buffer);
                                ostream.Write(contentRead, 0, bytesRead);
                            }
                            while (!(bytesRead < buffer));

                            fs.Close();
                        }
                        ostream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Falha ao enviar arquivo {0} por FTP. A mensagem do sistema foi:", path_source_file, ex.Message));
            }
        }
    }
}
