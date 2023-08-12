using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Terk.DB.Entities;

[Table("product")]
[Index("Name", Name = "uq_product_name", IsUnique = true)]
public partial class Product
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(30)]
    public string Name { get; set; } = null!;

    [Column("cost")]
    public double Cost { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<OrderPosition> OrderPositions { get; set; } = new List<OrderPosition>();
}
