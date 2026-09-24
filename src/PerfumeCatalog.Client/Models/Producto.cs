using Postgrest.Attributes;
using Postgrest.Models;

namespace PerfumeCatalog.Client.Models;

[Table("productos")]
public class Producto : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = "";

    [Column("descripcion")]
    public string Descripcion { get; set; } = "";

    [Column("precio")]
    public decimal Precio { get; set; }

    [Column("imagen_url")]
    public string? ImagenUrl { get; set; }

    [Column("categoria")]
    public string Categoria { get; set; } = "Perfume";

    [Column("disponible")]
    public bool Disponible { get; set; } = true;
    [Column("categoria_id")]
    public int? CategoriaId { get; set; }
}
