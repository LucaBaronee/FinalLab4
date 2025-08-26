namespace Stock1.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string nombre { get; set; }
        public int codigo { get; set; }
        public string descripcion { get; set; }
        public string? imagen { get; set; }
        public int categoriaId { get; set; }
        public Categoria? categoria { get; set; }
        public List<Stock>? stocks { get; set; }                                                                                       
    }
}
