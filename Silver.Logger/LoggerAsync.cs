using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

namespace Silver.Logger
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 20/07/2012
    /// </summary>
    public static class LoggerAsync
    {
        private static List<Queue<MensagemLogger>> GerenciadorFila = new List<Queue<MensagemLogger>>();

        public static string pathLog = string.Empty;

        private static Queue<MensagemLogger> MensagensPendentes = new Queue<MensagemLogger>();

        private static bool Processando = false;

        private static System.Threading.Thread thread1;

        public static void WriteLog(MensagemLogger msg)
        {
            try
            {
                if (GerenciadorFila.Count <= 0)
                    GerenciadorFila.Add(new Queue<MensagemLogger>());

                for (int i = 0; i < GerenciadorFila.Count; i++)
                {
                    if (GerenciadorFila[i].Count >= (int.MaxValue / 2))
                    {
                        GerenciadorFila.Add(new Queue<MensagemLogger>());
                        GerenciadorFila[i + 1].Enqueue(msg);
                    }
                    else
                        GerenciadorFila[i].Enqueue(msg);
                }

                //MensagensPendentes.Enqueue(msg);
                if (!Processando)
                {
                    Processando = true;
                    thread1 = new System.Threading.Thread(new ThreadStart(ProcessarFilaMensagem));
                    thread1.Start();
                }
            } catch (Exception)
            {
            }
        }

        private static void ProcessarFilaMensagem()
        {
            try
            {
                Processando = true;
                if (string.IsNullOrEmpty(pathLog))
                    throw new Exception("O caminho do arquivo de log(pathLog) deve ser informado");

                using (FileStream fs = new FileStream(pathLog, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        for (int i = 0; i < GerenciadorFila.Count; i++)
                        {
                            while (GerenciadorFila[i].Count > 0)
                            {
                                System.Threading.Thread.Sleep(10);
                                MensagemLogger msg = GerenciadorFila[i].Dequeue();
                                if (msg == null) continue;
                                sw.WriteLine(string.Format("[{0}] - [{1}] - {2}", DateTime.Now, msg.TypeLogger.ToString(), msg.MessageLogger));
                            }

                            GerenciadorFila.Remove(GerenciadorFila[i]);
                        }
                        sw.Close();
                    }
                    fs.Close();
                }
                Processando = false;
                thread1 = null;

            } catch (FieldAccessException ex)
            {
                Processando = false;
                throw ex;
            } catch (Exception) { Processando = false; }
        }

        public static ulong GetCountMessage()
        {
            ulong totalMensagens = 0;
            for (int i = 0; i < GerenciadorFila.Count; i++)
                totalMensagens += (ulong)GerenciadorFila[i].Count;
            
            return totalMensagens;
        }
    }
}
