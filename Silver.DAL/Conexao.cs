namespace Silver.DAL
{
    public static class Conexao
    {
        public static string strConn = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringSilver"].ConnectionString.ToString();
    }
}
