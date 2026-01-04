using System.Collections.Generic;
using Silver.AsteriskClient.DTO;

namespace Silver.AsteriskClient
{
    public delegate void RamalAtendeu(long Ramal, string LinhaConectada);
    public delegate void RamalDiscando(long Ramal);
    public delegate void RamalChamando(long Ramal);
    public delegate void RamalDesligou(long Ramal, string Causa);
    public delegate void RamalOcupado(long Ramal);
    public delegate void RamalNaoAtendeu(long Ramal);
    public delegate void RamalRegistrado(long Ramal);
    public delegate void RamalDesRegistrado(long Ramal);

    public delegate void DestinoAtendeu(string Telefone);
    public delegate void DestinoDiscando(string Telefone);
    public delegate void DestinoChamando(string Telefone);
    public delegate void DestinoDesligou(string Telefone, string Causa);
    public delegate void DestinoOcupado(string Telefone);
    public delegate void DestinoNaoAtendeu(string Telefone);

    public delegate void EntrouEmPausa(long Ramal);
    public delegate void SaiuDePausa(long Ramal);

    public delegate void RamaisStatus(List<RamalStatus> RamaisStatus);
    public delegate void QueueMember(long Ramal, string fila, QueueMemberStatus status);
    public delegate void QueueParams(QueueParamsEntity queueParamsEntity);

    //public delegate void Erro(AsteriskClientException Erro);

    public delegate void OutputLogs(string msg);
}
