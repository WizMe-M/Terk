using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Terk.DB.Entities;

[Table("order_position")]
public partial class OrderPosition
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("product_count")]
    public byte ProductCount { get; set; }

    [Column("cost", TypeName = "money")]
    public decimal Cost { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderPositions")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("OrderPositions")]
    public virtual Product Product { get; set; } = null!;
}
