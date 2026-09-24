using Postgrest.Attributes;
using Postgrest.Models;

namespace PerfumeCatalog.Client.Models;

[Table("categorias")]
public class Categoria : BaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = "";

    [Column("emoji")]
    public string Emoji { get; set; } = "📦";

    [Column("orden")]
    public int Orden { get; set; }

    [Column("activa")]
    public bool Activa { get; set; } = true;
}