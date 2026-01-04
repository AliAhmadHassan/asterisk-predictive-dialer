using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.ProxyDiscagem.Teste.DTO
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    ///             Data: 10/07/2014
    /// </summary>
    [Serializable()]
    public class Channels
    {
        private long _Grupo;
        public long Grupo
        {
            get { return _Grupo; }
            set { _Grupo = value; }
        }

        private int _Quantidade;
        public int Quantidade
        {
            get{return _Quantidade;}
            set{_Quantidade = value;}
        }
    }
}
