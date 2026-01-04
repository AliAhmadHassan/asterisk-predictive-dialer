namespace Silver.AsteriskClient
{
    /// <summary>
    /// Define quais comandos estão implementados no mátodo enviar comando.
    /// </summary>
    public enum ComandoAsterisk
    {
        QueueReload,
        SipReload,
        DialPlanReload,
        CoreReload
    }

    public enum CausesDisconnection
    {
        AST_CAUSE_ACCESS_INFO_DISCARDED = 43,
        AST_CAUSE_BEARERCAPABILITY_NOTAUTH = 57,
        AST_CAUSE_BEARERCAPABILITY_NOTAVAIL = 58,
        AST_CAUSE_BEARERCAPABILITY_NOTIMPL = 65,
        AST_CAUSE_CALL_AWARDED_DELIVERED = 7,
        AST_CAUSE_CALL_REJECTED = 21,
        AST_CAUSE_CHAN_NOT_IMPLEMENTED = 66,
        AST_CAUSE_CHANNEL_UNACCEPTABLE = 6,
        AST_CAUSE_DESTINATION_OUT_OF_ORDER = 27,
        AST_CAUSE_FACILITY_NOT_IMPLEMENTED = 69,
        AST_CAUSE_FACILITY_NOT_SUBSCRIBED = 50,
        AST_CAUSE_FACILITY_REJECTED = 29,
        AST_CAUSE_IE_NONEXIST = 99,
        AST_CAUSE_INCOMING_CALL_BARRED = 54,
        AST_CAUSE_INCOMPATIBLE_DESTINATION = 88,
        AST_CAUSE_INTERWORKING = 127,
        AST_CAUSE_INVALID_CALL_REFERENCE = 81,
        AST_CAUSE_INVALID_IE_CONTENTS = 100,
        AST_CAUSE_INVALID_MSG_UNSPECIFIED = 95,
        AST_CAUSE_INVALID_NUMBER_FORMAT = 28,
        AST_CAUSE_MANDATORY_IE_LENGTH_ERROR = 103,
        AST_CAUSE_MANDATORY_IE_MISSING = 96,
        AST_CAUSE_MESSAGE_TYPE_NONEXIST = 97,
        AST_CAUSE_NETWORK_OUT_OF_ORDER = 38,
        AST_CAUSE_NO_ANSWER = 19,
        AST_CAUSE_NO_ROUTE_DESTINATION = 3,
        AST_CAUSE_NO_ROUTE_TRANSIT_NET = 2,
        AST_CAUSE_NO_USER_RESPONSE = 18,
        AST_CAUSE_NORMAL_CIRCUIT_CONGESTION = 34,
        AST_CAUSE_NORMAL_CLEARING = 16,
        AST_CAUSE_NORMAL_TEMPORARY_FAILURE = 41,
        AST_CAUSE_NORMAL_UNSPECIFIED = 31,
        AST_CAUSE_NUMBER_CHANGED = 22,
        AST_CAUSE_OUTGOING_CALL_BARRED = 52,
        AST_CAUSE_PRE_EMPTED = 45,
        AST_CAUSE_PROTOCOL_ERROR = 111,
        AST_CAUSE_RECOVERY_ON_TIMER_EXPIRE = 102,
        AST_CAUSE_REQUESTED_CHAN_UNAVAIL = 44,
        AST_CAUSE_RESPONSE_TO_STATUS_ENQUIRY = 30,
        AST_CAUSE_SWITCH_CONGESTION = 42,
        AST_CAUSE_UNALLOCATED = 1,
        AST_CAUSE_USER_BUSY = 17,
        AST_CAUSE_WRONG_CALL_STATE = 101,
        AST_CAUSE_WRONG_MESSAGE = 98
    }

    public enum AsteriskModulo
    {
        Ael,
        DialPlan,
        Queue,
        Sip,
        Tudo
    }

    public enum Protocolo
    {
        Algar = 5,
        GVT = 2,
        Mahatel = 4,
        Nexus = 6,
        Pontal = 7,
        Telefonica = 1,
        Transit = 3,
        NaoDefinido_SemCanais,
        Nenhum = 0
    }

    public enum PeerStatus
    {
        Aguardando,
        Atendendo,
        Chamando,
        Desconectado
    }

    public enum QueueMemberStatus
    {
        /*! Device is valid but channel didn't know state */
        AST_DEVICE_UNKNOWN = 0,
        /*! Device is not used */
        AST_DEVICE_NOT_INUSE = 1,
        /*! Device is in use */
        AST_DEVICE_INUSE = 2,
        /*! Device is busy */
        AST_DEVICE_BUSY = 3,
        /*! Device is invalid */
        AST_DEVICE_INVALID = 4,
        /*! Device is unavailable */
        AST_DEVICE_UNAVAILABLE = 5,
        /*! Device is ringing */
        AST_DEVICE_RINGING = 6,
        /*! Device is ringing *and* in use */
        AST_DEVICE_RINGINUSE = 7,
        /*! Device is on hold */
        AST_DEVICE_ONHOLD = 8,

        AST_DEVICE_PAUSED = 9


    }

    public enum SaidaPadraoAsterisk
    {
        BancoDados = 1,
        Delegate = 2,
        Memoria = 3,
        Debug = 4,
        BancoDadosDelegate = 5,
        BancoDadosDebug = 6,
        Nenhum = 7,
    }

    public enum SaidaLigacao
    {
        Stream,
        FileSystem
    }
}
