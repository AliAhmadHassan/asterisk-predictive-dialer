namespace Silver.DTO
{
    public enum EstadoUsuario
    {
        Aguardando,
        Andamento,
        Atendimento,
        Logoff,
        Pausa
    }

    public enum LoggerType
    {
        ERRO,
        INFO
    }

    public enum TipoID
    {
        Campanha,
        Pausa
    }

    public enum TipoExclusao
    {
        Ambos,
        PelaCampanha,
        PelaPausa,
        PeloGrupo,
        PeloMenu
    }

    public enum TipoConsulta
    {
        PelaCampanha,
        PelaPK,
        PeloGrupo,
        PeloMenu,
        PeloRamal
    }

    public enum TipoMensagemSistema
    { 
        E1_SemCanal,
        E1_Alerta,
        E1_SemSinal,
        E1_Erro,

        Usuario_Logon,
        Usuario_Logoff,
        Usuario_InicioPausa,
        Usuario_RetornoPausa,
        Usuario_Desconectado_Asterisk,
        Usuario_Conectado_Asterisk,
        Usuario_Erro,
        
        Discador_Inicio,
        Discador_Fim,
        Discador_Erro,

        Campanha_Cadastrada,
        Campanha_Atualizada,
        Campanha_Bloqueada,
        Campanha_Iniciada,
        Campanha_Finalizada,
        Campanha_Erro
    }

    public enum SilverStatus : int
    {
        Aguardando = 1,
        Atendido = 2,
        Ocupado = 3,
        NaoAtende = 4,
        AMD = 5,
        Desconhecido = 6,
        EmAndamento = 7,
        SemSucessoNaDiscagem = 8,
        Finalizado = 9,
        Importado = 10,
        Invalido = 11,
        CargaAntiga = 12,
        Todos = 13,
        Cliente_Atendeu_Em_Outro_Numero = 14,
        Discagem_Concluida = 15,
        Mailing_Processado = 16,
        Processando_Mailing = 17,
        Discado = 18
    }

    public enum StatusProcessoDiscador
    {
        Iniciado = 1,
        Executando = 2,
        Parado = 3,
        Finalizado = 4,
        Erro = 5,
        Cancelado_Pelo_Administrador = 6,
        Cancelado_Pelo_Operador = 7,
        Documento_Incompativel = 8
    }

    public enum EventoControleSistema
    {
        Iniciar_Campanha = 1,
        Parar_Campanha = 2,
        Continuar_Campanha = 3,
        Recarregar_Campanha = 4,
        Recarregar_Carga = 5,
        Recarregar_Telefone = 6,
        Recarregar_Sip = 7,
        Recarregar_Queue = 8,
        Processar_Carga = 9,
        Situacao_Fila = 10,
        Situacao_E1 = 11,
        Iniciar_Pausa = 12,
        Finalizar_Pausa = 13
    }

    public enum SitucaoEventoControleSistema : long
    {
        Aguardando = 1,
        Executando = 2,
        Finalizado = 3,
        Erro = 4,
        Documento_Incompativel = 5,
        Parado = 6
    }

    public enum StatusTelefoneDashboard
    {
        Atendido,
        NaoAtendido,
        Ocupado,
        NaoExiste
    }

    public enum EstrategiaDiscagem
    {
        ringall,
        ramdom,
        linear,
        leastrecent,
        fewestcalls,
        rrmemory,
        wrandom
    }

    /// <summary>
    /// Os valores deste enumerador deve corresponder a tabela de 
    /// tipos de eventos do sistema
    /// </summary>
    public enum EventoRamal
    {
        RamalAtendeu = 6,
        RamalDesligou = 7
    }
}
