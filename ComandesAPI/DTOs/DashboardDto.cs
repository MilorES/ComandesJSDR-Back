namespace ComandesAPI.DTOs
{
    /// <summary>
    /// Resum de comandes desglossat per estats
    /// </summary>
    public class ResumComandesDto
    {
        public int TotalComandes { get; set; }
        public int Esborranys { get; set; }
        public int PendentsAprovacio { get; set; }
        public int Aprovades { get; set; }
        public int EnProces { get; set; }
        public int Enviades { get; set; }
        public int Finalitzades { get; set; }
        public int Cancellades { get; set; }
    }

    /// <summary>
    /// Resum de productes del mestre
    /// </summary>
    public class ResumProductesDto
    {
        public int TotalProductes { get; set; }
        public int ProductesActius { get; set; }
        public int ProductesInhabilitats { get; set; }
        public int ProductesSenseStock { get; set; }
    }

    /// <summary>
    /// Productes agrupats per categoria
    /// </summary>
    public class ProductePerCategoriaDto
    {
        public string Categoria { get; set; } = string.Empty;
        public int QuantitatProductes { get; set; }
    }
}
