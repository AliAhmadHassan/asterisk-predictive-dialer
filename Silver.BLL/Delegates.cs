using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Silver.BLL
{
    /// <summary>
    /// Delegate utilizado para saída de mensagens para aplicações winforms
    /// </summary>
    /// <param name="msg"></param>
    public delegate void OutputLogs(string msg);

    /// <summary>
    /// Delegate utilizado para notificar quando houver alteração de status
    /// dentro da classe
    /// </summary>
    /// <param name="status"></param>
    public delegate void EmAlteracaoStatus(DTO.StatusProcessoDiscador status);

    /// <summary>
    /// Delegate utilizado para processar assincronamente as cargas das campanhas
    /// </summary>
    /// <param name="id_campanha">Código da campanha que será processada</param>
    /// <returns></returns>
    public delegate void ProcessarCargaAsync ();


}
