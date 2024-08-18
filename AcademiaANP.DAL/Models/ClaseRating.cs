using ANP_Academy.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class ClaseRating
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Clase")]
    public int IdClase { get; set; }

    [ForeignKey("Usuario")]
    public string UserId { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    public DateTime Fecha { get; set; }

    public virtual Clase Clase { get; set; }
    public virtual Usuario Usuario { get; set; }
}
