namespace ComandesAPI.DTOs
{
    public class ResumMensualComandesDto
    {
        public int Any { get; set; }
        public int Mes { get; set; }
        public string NomMes { get; set; } = string.Empty;
        public int QuantitatComandes { get; set; }
        public decimal TotalImport { get; set; }
    }
}
