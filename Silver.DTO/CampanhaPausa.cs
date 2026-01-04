namespace Silver.DTO
{
    public class CampanhaPausa
    {
        public CampanhaPausa()
        {
        }

        public CampanhaPausa(long idCampanha, long idPausa)
        {
            IdCampanha = idCampanha;
            IdPausa = idPausa;
        }

        public long IdCampanha { get; set; }
        public long IdPausa { get; set; }
    }
}
