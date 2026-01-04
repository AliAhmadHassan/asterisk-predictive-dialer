using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Silver.Common.Logger
{
    public static class LoggerAsync
    {
        private static List<Queue<LoggerMessage>> GerenciadorFila = new List<Queue<LoggerMessage>>();

        public static string pathLog = string.Empty;

        private static bool Processando = false;

        private static Thread thread1;

        public static void WriteLog(LoggerMessage msg, ConsoleColor color = ConsoleColor.White)
        {
            try
            {
                if (GerenciadorFila.Count <= 0)
                {
                    GerenciadorFila.Add(new Queue<LoggerMessage>());
                }
                for (var i = 0; i < GerenciadorFila.Count; i++)
                {
                    if (GerenciadorFila[i].Count >= (int.MaxValue / 2))
                    {
                        GerenciadorFila.Add(new Queue<LoggerMessage>());
                        GerenciadorFila[i + 1].Enqueue(msg);
                    }
                    else
                    {
                        GerenciadorFila[i].Enqueue(msg);
                    }
                }

                if (!Processando)
                {
                    Processando = true;
                    thread1 = new Thread(new ParameterizedThreadStart(ProcessarFilaMensagem));
                    thread1.Start(color);
                }
            }
            catch
            {
            }
        }

        private static object obj_thread = new object();
        private static void ProcessarFilaMensagem(object obj)
        {
            Monitor.Enter(obj_thread);
            try
            {
                ConsoleColor color = ConsoleColor.White;
                if (obj != null)
                    color = (ConsoleColor)obj;

                Processando = true;
                if (string.IsNullOrEmpty(pathLog))
                {
                    throw new Exception("O caminho do arquivo de log(pathLog) deve ser informado");
                }
                using (var fs = new FileStream(pathLog, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        for (var i = 0; i < GerenciadorFila.Count; i++)
                        {
                            while (GerenciadorFila[i].Count > 0)
                            {
                                Thread.Sleep(10);

                                var msg = GerenciadorFila[i].Dequeue();
                                if (msg == null)
                                    continue;

                                var log = string.Format("[{0}] - [{1}] - {2}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff"), msg.TypeLogger.ToString(), msg.MessageLogger);
                                ConsoleColor oldcolor = Console.ForegroundColor;
                                Console.ForegroundColor = color;
                                Console.Out.WriteLine(log);
                                sw.WriteLine(log);
                                Console.ForegroundColor = oldcolor;
                            }

                            GerenciadorFila.Remove(GerenciadorFila[i]);
                        }
                        sw.Close();
                    }
                    fs.Close();
                }
                Processando = false;
                thread1 = null;
            }
            catch (FieldAccessException ex)
            {
                Processando = false;
                throw ex;
            }
            catch
            {
                Processando = false;
            }
            finally
            {
                Monitor.Exit(obj_thread);
            }
        }

        public static ulong GetCountMessage()
        {
            ulong totalMensagens = 0;
            for (var i = 0; i < GerenciadorFila.Count; i++)
            {
                totalMensagens += (ulong)GerenciadorFila[i].Count;
            }
            return totalMensagens;
        }
    }
}
