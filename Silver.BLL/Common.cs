using System.Data;

namespace Silver.BLL
{
    /// <summary>
    /// Desenvolvida por: Francisco Silva
    /// Data: 08/04/2013
    ///
    /// OBS: Esta classe será utilizada apenas para consultas rápidas para
    /// preencher User Controls ou quando as classes de serviços 
    /// do sistema não atenderem na "forma padrão" o que se deseja consultar.
    /// </summary>
    public class Common
    {
        public DataTable GetDataTable(string query)
        {
            return new DAL.Common().Select(query);
        }
    }
}
