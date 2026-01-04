using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Silver.DTO
{
    public class AcompanhamentoThread
    {
        public DateTime Inicio { get; set; }

        public TimeSpan Duracao
        {
            get { return DateTime.Now - Inicio; }
        }

        public DateTime Termino { get; set; }

        public Thread ThreadContexto { get; set; }

        public int ThreadId { get { return ThreadContexto.ManagedThreadId; } }

        public string ThreadName { get { return ThreadContexto.Name; } }

        public ThreadPriority ThreadPrioridade { get { try { return ThreadContexto.ThreadState == ThreadState.Running ? ThreadContexto.Priority : ThreadPriority.Lowest; } catch { return ThreadPriority.Normal; }; } }

        public ThreadState ThreadEstado
        {
            get { return ThreadContexto.ThreadState; }
        }
    }
}
