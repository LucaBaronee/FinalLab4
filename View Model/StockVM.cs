using Stock1.Models;
namespace Stock1.View_Model
{
    public class StockVM
    {
        public List<Stock> stocks { get; set; }
        public int busquedaId { get; set; } 
        public Paginador paginador { get; set; }
    }
}
