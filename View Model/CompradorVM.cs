using Stock1.Models;

namespace Stock1.View_Model
{
    public class CompradorVM
    {
        public List<Comprador> compradores { get; set; }
        public string busquedaNombre { get; set; }
        public string busquedaApellido { get; set; }
        public Paginador paginador { get; set; }
    }
}
