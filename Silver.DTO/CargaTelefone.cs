using System;

namespace Silver.DTO
{
    public partial class CargaTelefone : Base
    {
        public CargaTelefone()
        {
            Id = 0;
            Status = 1;
        }

        [AtributoBind(ChavePrimaria = true
        , ProcedureAlterar = "SPUcargatelefone"
        , ProcedureInserir = "SPIcargatelefone"
        , ProcedureRemover = "SPDcargatelefone"
        , ProcedureListarTodos = "SPScargatelefone"
        , ProcedureSelecionar = "SPScargatelefonePelaPK")]
        public long Id { get; set; }

        public long IdCarga { get; set; }
        public string TelId { get; set; }

        private string ddd;
        public string Ddd
        {
            get { return ddd; }
            set
            {
                try
                {
                    ddd = Convert.ToInt32(value).ToString();
                }
                catch
                {
                    ddd = "0";
                }
            }
        }

        public string Telefone { get; set; }
        public long IdTipo { get; set; }
        public int Status { get; set; }
        public bool Ativo { get; set; }
        public long Ramal { get; set; }

    }

    public class CargaTelefoneRobo : CargaTelefone
    {
        public int Prioridade { get; set; }
        public string TelefoneTratado { get; set; }
    }
}
