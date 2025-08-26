namespace Stock1.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int cantidad { get; set; }
        public DateTime fecha { get; set; }
        public int? productoId { get; set; }
        public Producto? producto { get; set; }
        public int? compradorId { get; set; }
        public Comprador? comprador { get; set; }
        
    }
}
