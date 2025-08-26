using Microsoft.AspNetCore.Mvc.Rendering;
using Stock1.Models;

namespace Stock1.View_Model
{
    public class ProductoVM
    {
        public List<Producto> productos { get; set; }
        public SelectList listaCategorias { get; set; }
        public string busquedaNombre { get; set; }
        public int? busquedaCodigo { get; set; }
        public int? categoriaId { get; set; }
        public Paginador paginador { get; set; }
    }
}
