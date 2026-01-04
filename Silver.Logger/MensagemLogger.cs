using System;
using System.Collections.Generic;
using System.Text;

namespace Silver.Logger
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 20/07/2012
    /// </summary>
    public class MensagemLogger
    {
        public MensagemLogger() { }

        public MensagemLogger(string msg, LoggerType type)
        {
            this._TypeLogger = type;
            this._MessageLogger = msg;
        }

        private LoggerType _TypeLogger;
        public LoggerType TypeLogger
        {
            get { return _TypeLogger; }
            set { _TypeLogger = value; }
        }

        private string _MessageLogger;
        public string MessageLogger
        {
            get { return _MessageLogger; }
            set { _MessageLogger = value; }
        }
    }
}
