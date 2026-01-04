using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.DTO
{
    [Serializable()]
    public class RelCampanha
    {
        private int _dadoC;

        public int dadoC
        {
            get { return _dadoC; }
            set { _dadoC = value; }
        }
        private int _dadoA;

        public int dadoA
        {
            get { return _dadoA; }
            set { _dadoA = value; }
        }

        private List<RelCampanhaDados> _lstDados;

        public List<RelCampanhaDados> lstDados
        {
            get { return _lstDados; }
            set { _lstDados = value; }
        }
    }

    [Serializable()]
    public class RelCampanhaDados
    {
        private int _Ramal;

        public int Ramal
        {
            get { return _Ramal; }
            set { _Ramal = value; }
        }

        private string _Status;

        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        private int _Chamadas;

        public int Chamadas
        {
            get { return _Chamadas; }
            set { _Chamadas = value; }
        }

        private int _Duracao;

        public int Duracao
        {
            get { return _Duracao; }
            set { _Duracao = value; }
        }

        private string _Telefone;

        public string Telefone
        {
            get { return _Telefone; }
            set { _Telefone = value; }
        }
    }

    [Serializable()]
    public class RelCampanhaQueueStatus
    {
        public string Channel { get; set; }

        public string CallerIDNum { get; set; }

        public int Wait { get; set; }
    }

    [Serializable()]
    public class RelCampanhaDB : Base
    {
        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPURelCampanha"
        , ProcedureInserir = "SPIRelCampanha"
        , ProcedureRemover = "SPDRelCampanha"
        , ProcedureListarTodos = "SPSRelCampanha"
        , ProcedureSelecionar = "SPSRelCampanhaPelaCampanha")]
        public long IdCampanha { get; set; }

        public string QueueStatus { get; set; }

        public string QueueShow { get; set; }
    }

}
